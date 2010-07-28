using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Collections.ObjectModel;
using XBrowserProject.HtmlParser.HtmlTokenizer;

namespace XBrowserProject.HtmlParser
{
    public class CharacterReference
    {
        private enum CharacterReferenceType
        {
            None,
            Numeric,
            HexNumeric,
            Named
        }

        private static Dictionary<int, string> knownCharacterCodeSubstitutions;
        private static Dictionary<string, string> namedCharacterReferences;
        private static List<char> invalidStartCharacters;

        private CharacterReferenceType referenceType = CharacterReferenceType.None;
        private string characterReferenceText = string.Empty;
        private HtmlTextReader internalReader;

        public CharacterReference(HtmlTextReader reader)
        {
            InitializeInvalidStartCharactersList();
            InitializeKnownCharacterCodeSubstitutionsList();
            InitializeNamedCharacterReferencesList();
            internalReader = reader;
        }

        public bool Parse(char additionalAllowedCharacter, bool isInAttribute)
        {
            bool foundCharacterReference = false;
            if (IsPositionAtValidCharacterReference(additionalAllowedCharacter))
            {
                string reference = ReadCharacterReference();
                if (!string.IsNullOrEmpty(reference))
                {
                    if (referenceType == CharacterReferenceType.Numeric || referenceType == CharacterReferenceType.HexNumeric)
                    {
                        GetCharacterForCodePoint(reference);
                    }
                    else if (referenceType == CharacterReferenceType.Named)
                    {
                        GetCharacterForNamedCharacterReference(reference, isInAttribute);
                    }

                    // If, after getting the reference for the appropriate type,
                    // we have a reference type of "None", a matching reference
                    // was not found, and we should unconsume the consumed characters.
                    if (referenceType != CharacterReferenceType.None)
                    {
                        foundCharacterReference = true;
                    }
                }
            }

            return foundCharacterReference;
        }

        public event EventHandler<ParseErrorEventArgs> ParseError;

        public string CharacterReferenceText
        {
            get { return characterReferenceText; }
        }

        protected void OnParseError(ParseErrorEventArgs e)
        {
            if (ParseError != null)
            {
                ParseError(this, e);
            }
        }

        private bool IsPositionAtValidCharacterReference(char additionalAllowedCharacter)
        {
            int currentCharValue = internalReader.Peek();
            return (currentCharValue >= 0) &&
                (additionalAllowedCharacter == char.MinValue || (additionalAllowedCharacter != char.MinValue && (char)currentCharValue != additionalAllowedCharacter)) &&
                !invalidStartCharacters.Contains((char)currentCharValue);
        }

        private string ReadCharacterReference()
        {
            string reference = string.Empty;
            char currentChar = (char)internalReader.Peek();
            if (currentChar == HtmlCharacterUtilities.HashMark)
            {
                // Consume the hash mark.
                internalReader.Read();
                reference = ReadNumericCharacterReference();
            }
            else
            {
                reference = ReadNamedCharacterReference();
            }

            return reference;
        }

        private string ReadNumericCharacterReference()
        {
            string numericReference = string.Empty;
            bool parseNumberAsHex = false;
            if (internalReader.Peek() >= 0)
            {
                char hexMarker = (char)internalReader.Peek();
                if (hexMarker == 'x' || hexMarker == 'X')
                {
                    parseNumberAsHex = true;
                    internalReader.Read();
                }

                int currentCharValue = internalReader.Peek();
                if (currentCharValue >= 0)
                {
                    while (currentCharValue >= 0 && (char)currentCharValue != HtmlCharacterUtilities.Semicolon && IsValidDigit((char)currentCharValue, parseNumberAsHex))
                    {
                        // While the current character is a valid numeric character or a semicolon,
                        // consume the character, add it to the reference, then peek at the next one.
                        internalReader.Read();
                        numericReference += (char)currentCharValue;
                        currentCharValue = internalReader.Peek();
                    }
                }

                if (string.IsNullOrEmpty(numericReference))
                {
                    OnParseError(new ParseErrorEventArgs("No digits after ampersand", "Revert position to immediately after ampersand", internalReader));
                }
                else
                {
                    characterReferenceText = "#";
                    referenceType = CharacterReferenceType.Numeric;

                    if (parseNumberAsHex)
                    {
                        characterReferenceText += "x";
                        referenceType = CharacterReferenceType.HexNumeric;
                    }

                    characterReferenceText += numericReference;
                    if ((char)currentCharValue != HtmlCharacterUtilities.Semicolon)
                    {
                        OnParseError(new ParseErrorEventArgs("Numeric entity not followed by semicolon", "none", internalReader));
                    }
                    else
                    {
                        internalReader.Read();
                        characterReferenceText += (char)currentCharValue;
                    }
                }
            }
            else
            {
                OnParseError(new ParseErrorEventArgs("Position moved past end of input", "none", internalReader));
            }

            return numericReference;
        }

        private string ReadNamedCharacterReference()
        {
            int currentCharValue = internalReader.Peek();

            // Construct the named character reference to match. All tokens in the table
            // are only constructed of characters from 0-9, a-z or A-Z. If we hit a character
            // other than that, we know we've gone too far.
            string nameToMatch = string.Empty;
            while (currentCharValue >= 0 && (HtmlCharacterUtilities.IsLowerCaseLetter((char)currentCharValue) || HtmlCharacterUtilities.IsUpperCaseLetter((char)currentCharValue) || HtmlCharacterUtilities.IsDigit((char)currentCharValue)))
            {
                internalReader.Read();
                nameToMatch += (char)currentCharValue;
                currentCharValue = internalReader.Peek();
            }

            if ((char)currentCharValue == HtmlCharacterUtilities.Semicolon)
            {
                internalReader.Read();
                nameToMatch += (char)currentCharValue;
            }

            if (!string.IsNullOrEmpty(nameToMatch))
            {
                referenceType = CharacterReferenceType.Named;
            }
            else
            {
                OnParseError(new ParseErrorEventArgs("No name found to match", "unconsuming", internalReader));
            }

            return nameToMatch;
        }

        private string GetCharacterForNamedCharacterReference(string referenceName, bool characterReferenceIsInAttribute)
        {
            string valueToReturn = string.Empty;
            bool charReferenceLocated = namedCharacterReferences.TryGetValue(referenceName, out valueToReturn);
            if (charReferenceLocated)
            {
                if (!referenceName.EndsWith(";"))
                {
                    OnParseError(new ParseErrorEventArgs("Named character references should end with semicolons", "none", internalReader));
                }

                characterReferenceText = valueToReturn;
            }
            else
            {
                // We didn't find a match. Try for a partial match. Start walking the token.
                int substringPosition = 1;
                while (!charReferenceLocated && substringPosition <= referenceName.Length)
                {
                    string partialToken = referenceName.Substring(0, substringPosition);
                    charReferenceLocated = namedCharacterReferences.TryGetValue(partialToken, out valueToReturn);
                    substringPosition++;
                    if (charReferenceLocated)
                    {
                        // If we are here, there is the possibility of there being characters after the 
                        // matching token candidate character reference, which necessitates special
                        // handling, so we peek at the next character. We know that the last match 
                        // character is not a semicolon because the named character reference reading 
                        // algorithm terminates on a semicolon, and we would have had to match the entire
                        // token above.
                        // From the HTML5 spec: If the character reference is being consumed as part of an attribute,
                        // and the last character matched is not a U+003B SEMICOLON character (;), and the next
                        // character is in the range U+0030 DIGIT ZERO (0) to U+0039 DIGIT NINE (9), 
                        // U+0041 LATIN CAPITAL LETTER A to U+005A LATIN CAPITAL LETTER Z, or 
                        // U+0061 LATIN SMALL LETTER A to U+007A LATIN SMALL LETTER Z, then, for historical 
                        // reasons, all the characters that were matched after the U+0026 AMPERSAND character (&) 
                        // must be unconsumed, and nothing is returned.
                        if (characterReferenceIsInAttribute && substringPosition <= referenceName.Length)
                        {
                            char charFollowingMatch = referenceName[substringPosition - 1];
                            if (HtmlCharacterUtilities.IsDigit(charFollowingMatch) ||
                                HtmlCharacterUtilities.IsLowerCaseLetter(charFollowingMatch) ||
                                HtmlCharacterUtilities.IsUpperCaseLetter(charFollowingMatch))
                            {
                                referenceType = CharacterReferenceType.None;
                                valueToReturn = string.Empty;
                            }
                        }
                        else
                        {
                            characterReferenceText = referenceName.Replace(partialToken, valueToReturn);
                        }
                    }
                }

                if (!charReferenceLocated)
                {
                    OnParseError(new ParseErrorEventArgs("No match in character reference table", "Returning nothing, no characters consumed", internalReader));
                    referenceType = CharacterReferenceType.None;
                }
                else
                {
                    OnParseError(new ParseErrorEventArgs("Character reference should end in a semicolon", "none", internalReader));
                }
            }

            return valueToReturn;
        }

        private string GetCharacterForCodePoint(string numberString)
        {
            string currentChar = string.Empty;

            NumberStyles numericStyle = NumberStyles.Integer;
            if (referenceType == CharacterReferenceType.HexNumeric)
            {
                numericStyle = NumberStyles.HexNumber;
            }

            int numberValue;
            bool successfulParse = int.TryParse(numberString, numericStyle, CultureInfo.InvariantCulture, out numberValue);
            if (!successfulParse)
            {
                numberValue = int.MaxValue;
            }

            if (successfulParse && knownCharacterCodeSubstitutions.TryGetValue(numberValue, out currentChar))
            {
                OnParseError(new ParseErrorEventArgs("Character code &" + characterReferenceText + " is invalid", "Replaced with " + currentChar.ToString(), internalReader));
            }
            else if (HtmlCharacterUtilities.CharacterCodePointRequiresReplacementCharacter(numberValue))
            {
                // XXX: This is a bit of a cheat. If the parse was unsuccessful, numberValue will
                // be int.MaxValue, which will fall into this branch.
                OnParseError(new ParseErrorEventArgs("Character code &" + characterReferenceText + " is invalid", "Replaced with standard replacement character (U+FFFD)", internalReader));
                currentChar = char.ConvertFromUtf32(0xFFFD);
            }
            else
            {
                if (HtmlCharacterUtilities.CharacterCodePointIsUnprintable(numberValue))
                {
                    OnParseError(new ParseErrorEventArgs("Character code &" + characterReferenceText + " is unprintable", "none", internalReader));
                }

                currentChar = char.ConvertFromUtf32(numberValue);
            }

            characterReferenceText = currentChar;
            return currentChar;
        }

        private bool IsValidDigit(char character, bool parseAsHex)
        {
            bool isDigit = HtmlCharacterUtilities.IsDigit(character);
            if (parseAsHex)
            {
                isDigit = HtmlCharacterUtilities.IsHexDigit(character);
            }

            return isDigit;
        }

        private static void InitializeInvalidStartCharactersList()
        {
            if (invalidStartCharacters == null)
            {
                invalidStartCharacters = new List<char>();
                invalidStartCharacters.Add(HtmlCharacterUtilities.Tab); // U+0009 CHARACTER TABULATION
                invalidStartCharacters.Add(HtmlCharacterUtilities.LineFeed); // U+000A LINE FEED (LF)
                invalidStartCharacters.Add(HtmlCharacterUtilities.FormFeed); // U+000C FORM FEED (FF)
                invalidStartCharacters.Add(HtmlCharacterUtilities.Space); // U+0020 SPACE
                invalidStartCharacters.Add(HtmlCharacterUtilities.LessThanSign); // U+003C LESS-THAN SIGN
                invalidStartCharacters.Add(HtmlCharacterUtilities.Ampersand); // U+0026 AMPERSAND
            }
        }

        private static void InitializeKnownCharacterCodeSubstitutionsList()
        {
            if (knownCharacterCodeSubstitutions == null)
            {
                knownCharacterCodeSubstitutions = new Dictionary<int, string>();
                knownCharacterCodeSubstitutions.Add(0x00, char.ConvertFromUtf32(0xFFFD));  // REPLACEMENT CHARACTER
                knownCharacterCodeSubstitutions.Add(0x0D, char.ConvertFromUtf32(0x000D));  // LINE FEED (LF)
                knownCharacterCodeSubstitutions.Add(0x80, char.ConvertFromUtf32(0x20AC));  // EURO SIGN (€)
                knownCharacterCodeSubstitutions.Add(0x81, char.ConvertFromUtf32(0x0081));  // <control>
                knownCharacterCodeSubstitutions.Add(0x82, char.ConvertFromUtf32(0x201A));  // SINGLE LOW-9 QUOTATION MARK (‚)
                knownCharacterCodeSubstitutions.Add(0x83, char.ConvertFromUtf32(0x0192));  // LATIN SMALL LETTER F WITH HOOK (ƒ)
                knownCharacterCodeSubstitutions.Add(0x84, char.ConvertFromUtf32(0x201E));  // DOUBLE LOW-9 QUOTATION MARK („)
                knownCharacterCodeSubstitutions.Add(0x85, char.ConvertFromUtf32(0x2026));  // HORIZONTAL ELLIPSIS (…)
                knownCharacterCodeSubstitutions.Add(0x86, char.ConvertFromUtf32(0x2020));  // DAGGER (†)
                knownCharacterCodeSubstitutions.Add(0x87, char.ConvertFromUtf32(0x2021));  // DOUBLE DAGGER (‡)
                knownCharacterCodeSubstitutions.Add(0x88, char.ConvertFromUtf32(0x02C6));  // MODIFIER LETTER CIRCUMFLEX ACCENT (ˆ)
                knownCharacterCodeSubstitutions.Add(0x89, char.ConvertFromUtf32(0x2030));  // PER MILLE SIGN (‰)
                knownCharacterCodeSubstitutions.Add(0x8A, char.ConvertFromUtf32(0x0160));  // LATIN CAPITAL LETTER S WITH CARON (Š)
                knownCharacterCodeSubstitutions.Add(0x8B, char.ConvertFromUtf32(0x2039));  // SINGLE LEFT-POINTING ANGLE QUOTATION MARK (‹)
                knownCharacterCodeSubstitutions.Add(0x8C, char.ConvertFromUtf32(0x0152));  // LATIN CAPITAL LIGATURE OE (Œ)
                knownCharacterCodeSubstitutions.Add(0x8D, char.ConvertFromUtf32(0x008D));  // <control>
                knownCharacterCodeSubstitutions.Add(0x8E, char.ConvertFromUtf32(0x017D));  // LATIN CAPITAL LETTER Z WITH CARON (Ž)
                knownCharacterCodeSubstitutions.Add(0x8F, char.ConvertFromUtf32(0x008F));  // <control>
                knownCharacterCodeSubstitutions.Add(0x90, char.ConvertFromUtf32(0x0090));  // <control>
                knownCharacterCodeSubstitutions.Add(0x91, char.ConvertFromUtf32(0x2018));  // LEFT SINGLE QUOTATION MARK (‘)
                knownCharacterCodeSubstitutions.Add(0x92, char.ConvertFromUtf32(0x2019));  // RIGHT SINGLE QUOTATION MARK (’)
                knownCharacterCodeSubstitutions.Add(0x93, char.ConvertFromUtf32(0x201C));  // LEFT DOUBLE QUOTATION MARK (“)
                knownCharacterCodeSubstitutions.Add(0x94, char.ConvertFromUtf32(0x201D));  // RIGHT DOUBLE QUOTATION MARK (”)
                knownCharacterCodeSubstitutions.Add(0x95, char.ConvertFromUtf32(0x2022));  // BULLET (•)
                knownCharacterCodeSubstitutions.Add(0x96, char.ConvertFromUtf32(0x2013));  // EN DASH (–)
                knownCharacterCodeSubstitutions.Add(0x97, char.ConvertFromUtf32(0x2014));  // EM DASH (—)
                knownCharacterCodeSubstitutions.Add(0x98, char.ConvertFromUtf32(0x02DC));  // SMALL TILDE (˜)
                knownCharacterCodeSubstitutions.Add(0x99, char.ConvertFromUtf32(0x2122));  // TRADE MARK SIGN (™)
                knownCharacterCodeSubstitutions.Add(0x9A, char.ConvertFromUtf32(0x0161));  // LATIN SMALL LETTER S WITH CARON (š)
                knownCharacterCodeSubstitutions.Add(0x9B, char.ConvertFromUtf32(0x203A));  // SINGLE RIGHT-POINTING ANGLE QUOTATION MARK (›)
                knownCharacterCodeSubstitutions.Add(0x9C, char.ConvertFromUtf32(0x0153));  // LATIN SMALL LIGATURE OE (œ)
                knownCharacterCodeSubstitutions.Add(0x9D, char.ConvertFromUtf32(0x009D));  // <control>
                knownCharacterCodeSubstitutions.Add(0x9E, char.ConvertFromUtf32(0x017E));  // LATIN SMALL LETTER Z WITH CARON (ž)
                knownCharacterCodeSubstitutions.Add(0x9F, char.ConvertFromUtf32(0x0178));  // LATIN CAPITAL LETTER Y WITH DIAERESIS
            }
        }

        private static void InitializeNamedCharacterReferencesList()
        {
            if (namedCharacterReferences == null)
            {
                namedCharacterReferences = new Dictionary<string, string>();
                namedCharacterReferences.Add("AElig;", char.ConvertFromUtf32(0x000C6));
                namedCharacterReferences.Add("AElig", char.ConvertFromUtf32(0x000C6));
                namedCharacterReferences.Add("AMP;", char.ConvertFromUtf32(0x00026));
                namedCharacterReferences.Add("AMP", char.ConvertFromUtf32(0x00026));
                namedCharacterReferences.Add("Aacute;", char.ConvertFromUtf32(0x000C1));
                namedCharacterReferences.Add("Aacute", char.ConvertFromUtf32(0x000C1));
                namedCharacterReferences.Add("Abreve;", char.ConvertFromUtf32(0x00102));
                namedCharacterReferences.Add("Acirc;", char.ConvertFromUtf32(0x000C2));
                namedCharacterReferences.Add("Acirc", char.ConvertFromUtf32(0x000C2));
                namedCharacterReferences.Add("Acy;", char.ConvertFromUtf32(0x00410));
                namedCharacterReferences.Add("Afr;", char.ConvertFromUtf32(0x1D504));
                namedCharacterReferences.Add("Agrave;", char.ConvertFromUtf32(0x000C0));
                namedCharacterReferences.Add("Agrave", char.ConvertFromUtf32(0x000C0));
                namedCharacterReferences.Add("Alpha;", char.ConvertFromUtf32(0x00391));
                namedCharacterReferences.Add("Amacr;", char.ConvertFromUtf32(0x00100));
                namedCharacterReferences.Add("And;", char.ConvertFromUtf32(0x02A53));
                namedCharacterReferences.Add("Aogon;", char.ConvertFromUtf32(0x00104));
                namedCharacterReferences.Add("Aopf;", char.ConvertFromUtf32(0x1D538));
                namedCharacterReferences.Add("ApplyFunction;", char.ConvertFromUtf32(0x02061));
                namedCharacterReferences.Add("Aring;", char.ConvertFromUtf32(0x000C5));
                namedCharacterReferences.Add("Aring", char.ConvertFromUtf32(0x000C5));
                namedCharacterReferences.Add("Ascr;", char.ConvertFromUtf32(0x1D49C));
                namedCharacterReferences.Add("Assign;", char.ConvertFromUtf32(0x02254));
                namedCharacterReferences.Add("Atilde;", char.ConvertFromUtf32(0x000C3));
                namedCharacterReferences.Add("Atilde", char.ConvertFromUtf32(0x000C3));
                namedCharacterReferences.Add("Auml;", char.ConvertFromUtf32(0x000C4));
                namedCharacterReferences.Add("Auml", char.ConvertFromUtf32(0x000C4));
                namedCharacterReferences.Add("Backslash;", char.ConvertFromUtf32(0x02216));
                namedCharacterReferences.Add("Barv;", char.ConvertFromUtf32(0x02AE7));
                namedCharacterReferences.Add("Barwed;", char.ConvertFromUtf32(0x02306));
                namedCharacterReferences.Add("Bcy;", char.ConvertFromUtf32(0x00411));
                namedCharacterReferences.Add("Because;", char.ConvertFromUtf32(0x02235));
                namedCharacterReferences.Add("Bernoullis;", char.ConvertFromUtf32(0x0212C));
                namedCharacterReferences.Add("Beta;", char.ConvertFromUtf32(0x00392));
                namedCharacterReferences.Add("Bfr;", char.ConvertFromUtf32(0x1D505));
                namedCharacterReferences.Add("Bopf;", char.ConvertFromUtf32(0x1D539));
                namedCharacterReferences.Add("Breve;", char.ConvertFromUtf32(0x002D8));
                namedCharacterReferences.Add("Bscr;", char.ConvertFromUtf32(0x0212C));
                namedCharacterReferences.Add("Bumpeq;", char.ConvertFromUtf32(0x0224E));
                namedCharacterReferences.Add("CHcy;", char.ConvertFromUtf32(0x00427));
                namedCharacterReferences.Add("COPY;", char.ConvertFromUtf32(0x000A9));
                namedCharacterReferences.Add("COPY", char.ConvertFromUtf32(0x000A9));
                namedCharacterReferences.Add("Cacute;", char.ConvertFromUtf32(0x00106));
                namedCharacterReferences.Add("Cap;", char.ConvertFromUtf32(0x022D2));
                namedCharacterReferences.Add("CapitalDifferentialD;", char.ConvertFromUtf32(0x02145));
                namedCharacterReferences.Add("Cayleys;", char.ConvertFromUtf32(0x0212D));
                namedCharacterReferences.Add("Ccaron;", char.ConvertFromUtf32(0x0010C));
                namedCharacterReferences.Add("Ccedil;", char.ConvertFromUtf32(0x000C7));
                namedCharacterReferences.Add("Ccedil", char.ConvertFromUtf32(0x000C7));
                namedCharacterReferences.Add("Ccirc;", char.ConvertFromUtf32(0x00108));
                namedCharacterReferences.Add("Cconint;", char.ConvertFromUtf32(0x02230));
                namedCharacterReferences.Add("Cdot;", char.ConvertFromUtf32(0x0010A));
                namedCharacterReferences.Add("Cedilla;", char.ConvertFromUtf32(0x000B8));
                namedCharacterReferences.Add("CenterDot;", char.ConvertFromUtf32(0x000B7));
                namedCharacterReferences.Add("Cfr;", char.ConvertFromUtf32(0x0212D));
                namedCharacterReferences.Add("Chi;", char.ConvertFromUtf32(0x003A7));
                namedCharacterReferences.Add("CircleDot;", char.ConvertFromUtf32(0x02299));
                namedCharacterReferences.Add("CircleMinus;", char.ConvertFromUtf32(0x02296));
                namedCharacterReferences.Add("CirclePlus;", char.ConvertFromUtf32(0x02295));
                namedCharacterReferences.Add("CircleTimes;", char.ConvertFromUtf32(0x02297));
                namedCharacterReferences.Add("ClockwiseContourIntegral;", char.ConvertFromUtf32(0x02232));
                namedCharacterReferences.Add("CloseCurlyDoubleQuote;", char.ConvertFromUtf32(0x0201D));
                namedCharacterReferences.Add("CloseCurlyQuote;", char.ConvertFromUtf32(0x02019));
                namedCharacterReferences.Add("Colon;", char.ConvertFromUtf32(0x02237));
                namedCharacterReferences.Add("Colone;", char.ConvertFromUtf32(0x02A74));
                namedCharacterReferences.Add("Congruent;", char.ConvertFromUtf32(0x02261));
                namedCharacterReferences.Add("Conint;", char.ConvertFromUtf32(0x0222F));
                namedCharacterReferences.Add("ContourIntegral;", char.ConvertFromUtf32(0x0222E));
                namedCharacterReferences.Add("Copf;", char.ConvertFromUtf32(0x02102));
                namedCharacterReferences.Add("Coproduct;", char.ConvertFromUtf32(0x02210));
                namedCharacterReferences.Add("CounterClockwiseContourIntegral;", char.ConvertFromUtf32(0x02233));
                namedCharacterReferences.Add("Cross;", char.ConvertFromUtf32(0x02A2F));
                namedCharacterReferences.Add("Cscr;", char.ConvertFromUtf32(0x1D49E));
                namedCharacterReferences.Add("Cup;", char.ConvertFromUtf32(0x022D3));
                namedCharacterReferences.Add("CupCap;", char.ConvertFromUtf32(0x0224D));
                namedCharacterReferences.Add("DD;", char.ConvertFromUtf32(0x02145));
                namedCharacterReferences.Add("DDotrahd;", char.ConvertFromUtf32(0x02911));
                namedCharacterReferences.Add("DJcy;", char.ConvertFromUtf32(0x00402));
                namedCharacterReferences.Add("DScy;", char.ConvertFromUtf32(0x00405));
                namedCharacterReferences.Add("DZcy;", char.ConvertFromUtf32(0x0040F));
                namedCharacterReferences.Add("Dagger;", char.ConvertFromUtf32(0x02021));
                namedCharacterReferences.Add("Darr;", char.ConvertFromUtf32(0x021A1));
                namedCharacterReferences.Add("Dashv;", char.ConvertFromUtf32(0x02AE4));
                namedCharacterReferences.Add("Dcaron;", char.ConvertFromUtf32(0x0010E));
                namedCharacterReferences.Add("Dcy;", char.ConvertFromUtf32(0x00414));
                namedCharacterReferences.Add("Del;", char.ConvertFromUtf32(0x02207));
                namedCharacterReferences.Add("Delta;", char.ConvertFromUtf32(0x00394));
                namedCharacterReferences.Add("Dfr;", char.ConvertFromUtf32(0x1D507));
                namedCharacterReferences.Add("DiacriticalAcute;", char.ConvertFromUtf32(0x000B4));
                namedCharacterReferences.Add("DiacriticalDot;", char.ConvertFromUtf32(0x002D9));
                namedCharacterReferences.Add("DiacriticalDoubleAcute;", char.ConvertFromUtf32(0x002DD));
                namedCharacterReferences.Add("DiacriticalGrave;", char.ConvertFromUtf32(0x00060));
                namedCharacterReferences.Add("DiacriticalTilde;", char.ConvertFromUtf32(0x002DC));
                namedCharacterReferences.Add("Diamond;", char.ConvertFromUtf32(0x022C4));
                namedCharacterReferences.Add("DifferentialD;", char.ConvertFromUtf32(0x02146));
                namedCharacterReferences.Add("Dopf;", char.ConvertFromUtf32(0x1D53B));
                namedCharacterReferences.Add("Dot;", char.ConvertFromUtf32(0x000A8));
                namedCharacterReferences.Add("DotDot;", char.ConvertFromUtf32(0x020DC));
                namedCharacterReferences.Add("DotEqual;", char.ConvertFromUtf32(0x02250));
                namedCharacterReferences.Add("DoubleContourIntegral;", char.ConvertFromUtf32(0x0222F));
                namedCharacterReferences.Add("DoubleDot;", char.ConvertFromUtf32(0x000A8));
                namedCharacterReferences.Add("DoubleDownArrow;", char.ConvertFromUtf32(0x021D3));
                namedCharacterReferences.Add("DoubleLeftArrow;", char.ConvertFromUtf32(0x021D0));
                namedCharacterReferences.Add("DoubleLeftRightArrow;", char.ConvertFromUtf32(0x021D4));
                namedCharacterReferences.Add("DoubleLeftTee;", char.ConvertFromUtf32(0x02AE4));
                namedCharacterReferences.Add("DoubleLongLeftArrow;", char.ConvertFromUtf32(0x027F8));
                namedCharacterReferences.Add("DoubleLongLeftRightArrow;", char.ConvertFromUtf32(0x027FA));
                namedCharacterReferences.Add("DoubleLongRightArrow;", char.ConvertFromUtf32(0x027F9));
                namedCharacterReferences.Add("DoubleRightArrow;", char.ConvertFromUtf32(0x021D2));
                namedCharacterReferences.Add("DoubleRightTee;", char.ConvertFromUtf32(0x022A8));
                namedCharacterReferences.Add("DoubleUpArrow;", char.ConvertFromUtf32(0x021D1));
                namedCharacterReferences.Add("DoubleUpDownArrow;", char.ConvertFromUtf32(0x021D5));
                namedCharacterReferences.Add("DoubleVerticalBar;", char.ConvertFromUtf32(0x02225));
                namedCharacterReferences.Add("DownArrow;", char.ConvertFromUtf32(0x02193));
                namedCharacterReferences.Add("DownArrowBar;", char.ConvertFromUtf32(0x02913));
                namedCharacterReferences.Add("DownArrowUpArrow;", char.ConvertFromUtf32(0x021F5));
                namedCharacterReferences.Add("DownBreve;", char.ConvertFromUtf32(0x00311));
                namedCharacterReferences.Add("DownLeftRightVector;", char.ConvertFromUtf32(0x02950));
                namedCharacterReferences.Add("DownLeftTeeVector;", char.ConvertFromUtf32(0x0295E));
                namedCharacterReferences.Add("DownLeftVector;", char.ConvertFromUtf32(0x021BD));
                namedCharacterReferences.Add("DownLeftVectorBar;", char.ConvertFromUtf32(0x02956));
                namedCharacterReferences.Add("DownRightTeeVector;", char.ConvertFromUtf32(0x0295F));
                namedCharacterReferences.Add("DownRightVector;", char.ConvertFromUtf32(0x021C1));
                namedCharacterReferences.Add("DownRightVectorBar;", char.ConvertFromUtf32(0x02957));
                namedCharacterReferences.Add("DownTee;", char.ConvertFromUtf32(0x022A4));
                namedCharacterReferences.Add("DownTeeArrow;", char.ConvertFromUtf32(0x021A7));
                namedCharacterReferences.Add("Downarrow;", char.ConvertFromUtf32(0x021D3));
                namedCharacterReferences.Add("Dscr;", char.ConvertFromUtf32(0x1D49F));
                namedCharacterReferences.Add("Dstrok;", char.ConvertFromUtf32(0x00110));
                namedCharacterReferences.Add("ENG;", char.ConvertFromUtf32(0x0014A));
                namedCharacterReferences.Add("ETH;", char.ConvertFromUtf32(0x000D0));
                namedCharacterReferences.Add("ETH", char.ConvertFromUtf32(0x000D0));
                namedCharacterReferences.Add("Eacute;", char.ConvertFromUtf32(0x000C9));
                namedCharacterReferences.Add("Eacute", char.ConvertFromUtf32(0x000C9));
                namedCharacterReferences.Add("Ecaron;", char.ConvertFromUtf32(0x0011A));
                namedCharacterReferences.Add("Ecirc;", char.ConvertFromUtf32(0x000CA));
                namedCharacterReferences.Add("Ecirc", char.ConvertFromUtf32(0x000CA));
                namedCharacterReferences.Add("Ecy;", char.ConvertFromUtf32(0x0042D));
                namedCharacterReferences.Add("Edot;", char.ConvertFromUtf32(0x00116));
                namedCharacterReferences.Add("Efr;", char.ConvertFromUtf32(0x1D508));
                namedCharacterReferences.Add("Egrave;", char.ConvertFromUtf32(0x000C8));
                namedCharacterReferences.Add("Egrave", char.ConvertFromUtf32(0x000C8));
                namedCharacterReferences.Add("Element;", char.ConvertFromUtf32(0x02208));
                namedCharacterReferences.Add("Emacr;", char.ConvertFromUtf32(0x00112));
                namedCharacterReferences.Add("EmptySmallSquare;", char.ConvertFromUtf32(0x025FB));
                namedCharacterReferences.Add("EmptyVerySmallSquare;", char.ConvertFromUtf32(0x025AB));
                namedCharacterReferences.Add("Eogon;", char.ConvertFromUtf32(0x00118));
                namedCharacterReferences.Add("Eopf;", char.ConvertFromUtf32(0x1D53C));
                namedCharacterReferences.Add("Epsilon;", char.ConvertFromUtf32(0x00395));
                namedCharacterReferences.Add("Equal;", char.ConvertFromUtf32(0x02A75));
                namedCharacterReferences.Add("EqualTilde;", char.ConvertFromUtf32(0x02242));
                namedCharacterReferences.Add("Equilibrium;", char.ConvertFromUtf32(0x021CC));
                namedCharacterReferences.Add("Escr;", char.ConvertFromUtf32(0x02130));
                namedCharacterReferences.Add("Esim;", char.ConvertFromUtf32(0x02A73));
                namedCharacterReferences.Add("Eta;", char.ConvertFromUtf32(0x00397));
                namedCharacterReferences.Add("Euml;", char.ConvertFromUtf32(0x000CB));
                namedCharacterReferences.Add("Euml", char.ConvertFromUtf32(0x000CB));
                namedCharacterReferences.Add("Exists;", char.ConvertFromUtf32(0x02203));
                namedCharacterReferences.Add("ExponentialE;", char.ConvertFromUtf32(0x02147));
                namedCharacterReferences.Add("Fcy;", char.ConvertFromUtf32(0x00424));
                namedCharacterReferences.Add("Ffr;", char.ConvertFromUtf32(0x1D509));
                namedCharacterReferences.Add("FilledSmallSquare;", char.ConvertFromUtf32(0x025FC));
                namedCharacterReferences.Add("FilledVerySmallSquare;", char.ConvertFromUtf32(0x025AA));
                namedCharacterReferences.Add("Fopf;", char.ConvertFromUtf32(0x1D53D));
                namedCharacterReferences.Add("ForAll;", char.ConvertFromUtf32(0x02200));
                namedCharacterReferences.Add("Fouriertrf;", char.ConvertFromUtf32(0x02131));
                namedCharacterReferences.Add("Fscr;", char.ConvertFromUtf32(0x02131));
                namedCharacterReferences.Add("GJcy;", char.ConvertFromUtf32(0x00403));
                namedCharacterReferences.Add("GT;", char.ConvertFromUtf32(0x0003E));
                namedCharacterReferences.Add("GT", char.ConvertFromUtf32(0x0003E));
                namedCharacterReferences.Add("Gamma;", char.ConvertFromUtf32(0x00393));
                namedCharacterReferences.Add("Gammad;", char.ConvertFromUtf32(0x003DC));
                namedCharacterReferences.Add("Gbreve;", char.ConvertFromUtf32(0x0011E));
                namedCharacterReferences.Add("Gcedil;", char.ConvertFromUtf32(0x00122));
                namedCharacterReferences.Add("Gcirc;", char.ConvertFromUtf32(0x0011C));
                namedCharacterReferences.Add("Gcy;", char.ConvertFromUtf32(0x00413));
                namedCharacterReferences.Add("Gdot;", char.ConvertFromUtf32(0x00120));
                namedCharacterReferences.Add("Gfr;", char.ConvertFromUtf32(0x1D50A));
                namedCharacterReferences.Add("Gg;", char.ConvertFromUtf32(0x022D9));
                namedCharacterReferences.Add("Gopf;", char.ConvertFromUtf32(0x1D53E));
                namedCharacterReferences.Add("GreaterEqual;", char.ConvertFromUtf32(0x02265));
                namedCharacterReferences.Add("GreaterEqualLess;", char.ConvertFromUtf32(0x022DB));
                namedCharacterReferences.Add("GreaterFullEqual;", char.ConvertFromUtf32(0x02267));
                namedCharacterReferences.Add("GreaterGreater;", char.ConvertFromUtf32(0x02AA2));
                namedCharacterReferences.Add("GreaterLess;", char.ConvertFromUtf32(0x02277));
                namedCharacterReferences.Add("GreaterSlantEqual;", char.ConvertFromUtf32(0x02A7E));
                namedCharacterReferences.Add("GreaterTilde;", char.ConvertFromUtf32(0x02273));
                namedCharacterReferences.Add("Gscr;", char.ConvertFromUtf32(0x1D4A2));
                namedCharacterReferences.Add("Gt;", char.ConvertFromUtf32(0x0226B));
                namedCharacterReferences.Add("HARDcy;", char.ConvertFromUtf32(0x0042A));
                namedCharacterReferences.Add("Hacek;", char.ConvertFromUtf32(0x002C7));
                namedCharacterReferences.Add("Hat;", char.ConvertFromUtf32(0x0005E));
                namedCharacterReferences.Add("Hcirc;", char.ConvertFromUtf32(0x00124));
                namedCharacterReferences.Add("Hfr;", char.ConvertFromUtf32(0x0210C));
                namedCharacterReferences.Add("HilbertSpace;", char.ConvertFromUtf32(0x0210B));
                namedCharacterReferences.Add("Hopf;", char.ConvertFromUtf32(0x0210D));
                namedCharacterReferences.Add("HorizontalLine;", char.ConvertFromUtf32(0x02500));
                namedCharacterReferences.Add("Hscr;", char.ConvertFromUtf32(0x0210B));
                namedCharacterReferences.Add("Hstrok;", char.ConvertFromUtf32(0x00126));
                namedCharacterReferences.Add("HumpDownHump;", char.ConvertFromUtf32(0x0224E));
                namedCharacterReferences.Add("HumpEqual;", char.ConvertFromUtf32(0x0224F));
                namedCharacterReferences.Add("IEcy;", char.ConvertFromUtf32(0x00415));
                namedCharacterReferences.Add("IJlig;", char.ConvertFromUtf32(0x00132));
                namedCharacterReferences.Add("IOcy;", char.ConvertFromUtf32(0x00401));
                namedCharacterReferences.Add("Iacute;", char.ConvertFromUtf32(0x000CD));
                namedCharacterReferences.Add("Iacute", char.ConvertFromUtf32(0x000CD));
                namedCharacterReferences.Add("Icirc;", char.ConvertFromUtf32(0x000CE));
                namedCharacterReferences.Add("Icirc", char.ConvertFromUtf32(0x000CE));
                namedCharacterReferences.Add("Icy;", char.ConvertFromUtf32(0x00418));
                namedCharacterReferences.Add("Idot;", char.ConvertFromUtf32(0x00130));
                namedCharacterReferences.Add("Ifr;", char.ConvertFromUtf32(0x02111));
                namedCharacterReferences.Add("Igrave;", char.ConvertFromUtf32(0x000CC));
                namedCharacterReferences.Add("Igrave", char.ConvertFromUtf32(0x000CC));
                namedCharacterReferences.Add("Im;", char.ConvertFromUtf32(0x02111));
                namedCharacterReferences.Add("Imacr;", char.ConvertFromUtf32(0x0012A));
                namedCharacterReferences.Add("ImaginaryI;", char.ConvertFromUtf32(0x02148));
                namedCharacterReferences.Add("Implies;", char.ConvertFromUtf32(0x021D2));
                namedCharacterReferences.Add("Int;", char.ConvertFromUtf32(0x0222C));
                namedCharacterReferences.Add("Integral;", char.ConvertFromUtf32(0x0222B));
                namedCharacterReferences.Add("Intersection;", char.ConvertFromUtf32(0x022C2));
                namedCharacterReferences.Add("InvisibleComma;", char.ConvertFromUtf32(0x02063));
                namedCharacterReferences.Add("InvisibleTimes;", char.ConvertFromUtf32(0x02062));
                namedCharacterReferences.Add("Iogon;", char.ConvertFromUtf32(0x0012E));
                namedCharacterReferences.Add("Iopf;", char.ConvertFromUtf32(0x1D540));
                namedCharacterReferences.Add("Iota;", char.ConvertFromUtf32(0x00399));
                namedCharacterReferences.Add("Iscr;", char.ConvertFromUtf32(0x02110));
                namedCharacterReferences.Add("Itilde;", char.ConvertFromUtf32(0x00128));
                namedCharacterReferences.Add("Iukcy;", char.ConvertFromUtf32(0x00406));
                namedCharacterReferences.Add("Iuml;", char.ConvertFromUtf32(0x000CF));
                namedCharacterReferences.Add("Iuml", char.ConvertFromUtf32(0x000CF));
                namedCharacterReferences.Add("Jcirc;", char.ConvertFromUtf32(0x00134));
                namedCharacterReferences.Add("Jcy;", char.ConvertFromUtf32(0x00419));
                namedCharacterReferences.Add("Jfr;", char.ConvertFromUtf32(0x1D50D));
                namedCharacterReferences.Add("Jopf;", char.ConvertFromUtf32(0x1D541));
                namedCharacterReferences.Add("Jscr;", char.ConvertFromUtf32(0x1D4A5));
                namedCharacterReferences.Add("Jsercy;", char.ConvertFromUtf32(0x00408));
                namedCharacterReferences.Add("Jukcy;", char.ConvertFromUtf32(0x00404));
                namedCharacterReferences.Add("KHcy;", char.ConvertFromUtf32(0x00425));
                namedCharacterReferences.Add("KJcy;", char.ConvertFromUtf32(0x0040C));
                namedCharacterReferences.Add("Kappa;", char.ConvertFromUtf32(0x0039A));
                namedCharacterReferences.Add("Kcedil;", char.ConvertFromUtf32(0x00136));
                namedCharacterReferences.Add("Kcy;", char.ConvertFromUtf32(0x0041A));
                namedCharacterReferences.Add("Kfr;", char.ConvertFromUtf32(0x1D50E));
                namedCharacterReferences.Add("Kopf;", char.ConvertFromUtf32(0x1D542));
                namedCharacterReferences.Add("Kscr;", char.ConvertFromUtf32(0x1D4A6));
                namedCharacterReferences.Add("LJcy;", char.ConvertFromUtf32(0x00409));
                namedCharacterReferences.Add("LT;", char.ConvertFromUtf32(0x0003C));
                namedCharacterReferences.Add("LT", char.ConvertFromUtf32(0x0003C));
                namedCharacterReferences.Add("Lacute;", char.ConvertFromUtf32(0x00139));
                namedCharacterReferences.Add("Lambda;", char.ConvertFromUtf32(0x0039B));
                namedCharacterReferences.Add("Lang;", char.ConvertFromUtf32(0x027EA));
                namedCharacterReferences.Add("Laplacetrf;", char.ConvertFromUtf32(0x02112));
                namedCharacterReferences.Add("Larr;", char.ConvertFromUtf32(0x0219E));
                namedCharacterReferences.Add("Lcaron;", char.ConvertFromUtf32(0x0013D));
                namedCharacterReferences.Add("Lcedil;", char.ConvertFromUtf32(0x0013B));
                namedCharacterReferences.Add("Lcy;", char.ConvertFromUtf32(0x0041B));
                namedCharacterReferences.Add("LeftAngleBracket;", char.ConvertFromUtf32(0x027E8));
                namedCharacterReferences.Add("LeftArrow;", char.ConvertFromUtf32(0x02190));
                namedCharacterReferences.Add("LeftArrowBar;", char.ConvertFromUtf32(0x021E4));
                namedCharacterReferences.Add("LeftArrowRightArrow;", char.ConvertFromUtf32(0x021C6));
                namedCharacterReferences.Add("LeftCeiling;", char.ConvertFromUtf32(0x02308));
                namedCharacterReferences.Add("LeftDoubleBracket;", char.ConvertFromUtf32(0x027E6));
                namedCharacterReferences.Add("LeftDownTeeVector;", char.ConvertFromUtf32(0x02961));
                namedCharacterReferences.Add("LeftDownVector;", char.ConvertFromUtf32(0x021C3));
                namedCharacterReferences.Add("LeftDownVectorBar;", char.ConvertFromUtf32(0x02959));
                namedCharacterReferences.Add("LeftFloor;", char.ConvertFromUtf32(0x0230A));
                namedCharacterReferences.Add("LeftRightArrow;", char.ConvertFromUtf32(0x02194));
                namedCharacterReferences.Add("LeftRightVector;", char.ConvertFromUtf32(0x0294E));
                namedCharacterReferences.Add("LeftTee;", char.ConvertFromUtf32(0x022A3));
                namedCharacterReferences.Add("LeftTeeArrow;", char.ConvertFromUtf32(0x021A4));
                namedCharacterReferences.Add("LeftTeeVector;", char.ConvertFromUtf32(0x0295A));
                namedCharacterReferences.Add("LeftTriangle;", char.ConvertFromUtf32(0x022B2));
                namedCharacterReferences.Add("LeftTriangleBar;", char.ConvertFromUtf32(0x029CF));
                namedCharacterReferences.Add("LeftTriangleEqual;", char.ConvertFromUtf32(0x022B4));
                namedCharacterReferences.Add("LeftUpDownVector;", char.ConvertFromUtf32(0x02951));
                namedCharacterReferences.Add("LeftUpTeeVector;", char.ConvertFromUtf32(0x02960));
                namedCharacterReferences.Add("LeftUpVector;", char.ConvertFromUtf32(0x021BF));
                namedCharacterReferences.Add("LeftUpVectorBar;", char.ConvertFromUtf32(0x02958));
                namedCharacterReferences.Add("LeftVector;", char.ConvertFromUtf32(0x021BC));
                namedCharacterReferences.Add("LeftVectorBar;", char.ConvertFromUtf32(0x02952));
                namedCharacterReferences.Add("Leftarrow;", char.ConvertFromUtf32(0x021D0));
                namedCharacterReferences.Add("Leftrightarrow;", char.ConvertFromUtf32(0x021D4));
                namedCharacterReferences.Add("LessEqualGreater;", char.ConvertFromUtf32(0x022DA));
                namedCharacterReferences.Add("LessFullEqual;", char.ConvertFromUtf32(0x02266));
                namedCharacterReferences.Add("LessGreater;", char.ConvertFromUtf32(0x02276));
                namedCharacterReferences.Add("LessLess;", char.ConvertFromUtf32(0x02AA1));
                namedCharacterReferences.Add("LessSlantEqual;", char.ConvertFromUtf32(0x02A7D));
                namedCharacterReferences.Add("LessTilde;", char.ConvertFromUtf32(0x02272));
                namedCharacterReferences.Add("Lfr;", char.ConvertFromUtf32(0x1D50F));
                namedCharacterReferences.Add("Ll;", char.ConvertFromUtf32(0x022D8));
                namedCharacterReferences.Add("Lleftarrow;", char.ConvertFromUtf32(0x021DA));
                namedCharacterReferences.Add("Lmidot;", char.ConvertFromUtf32(0x0013F));
                namedCharacterReferences.Add("LongLeftArrow;", char.ConvertFromUtf32(0x027F5));
                namedCharacterReferences.Add("LongLeftRightArrow;", char.ConvertFromUtf32(0x027F7));
                namedCharacterReferences.Add("LongRightArrow;", char.ConvertFromUtf32(0x027F6));
                namedCharacterReferences.Add("Longleftarrow;", char.ConvertFromUtf32(0x027F8));
                namedCharacterReferences.Add("Longleftrightarrow;", char.ConvertFromUtf32(0x027FA));
                namedCharacterReferences.Add("Longrightarrow;", char.ConvertFromUtf32(0x027F9));
                namedCharacterReferences.Add("Lopf;", char.ConvertFromUtf32(0x1D543));
                namedCharacterReferences.Add("LowerLeftArrow;", char.ConvertFromUtf32(0x02199));
                namedCharacterReferences.Add("LowerRightArrow;", char.ConvertFromUtf32(0x02198));
                namedCharacterReferences.Add("Lscr;", char.ConvertFromUtf32(0x02112));
                namedCharacterReferences.Add("Lsh;", char.ConvertFromUtf32(0x021B0));
                namedCharacterReferences.Add("Lstrok;", char.ConvertFromUtf32(0x00141));
                namedCharacterReferences.Add("Lt;", char.ConvertFromUtf32(0x0226A));
                namedCharacterReferences.Add("Map;", char.ConvertFromUtf32(0x02905));
                namedCharacterReferences.Add("Mcy;", char.ConvertFromUtf32(0x0041C));
                namedCharacterReferences.Add("MediumSpace;", char.ConvertFromUtf32(0x0205F));
                namedCharacterReferences.Add("Mellintrf;", char.ConvertFromUtf32(0x02133));
                namedCharacterReferences.Add("Mfr;", char.ConvertFromUtf32(0x1D510));
                namedCharacterReferences.Add("MinusPlus;", char.ConvertFromUtf32(0x02213));
                namedCharacterReferences.Add("Mopf;", char.ConvertFromUtf32(0x1D544));
                namedCharacterReferences.Add("Mscr;", char.ConvertFromUtf32(0x02133));
                namedCharacterReferences.Add("Mu;", char.ConvertFromUtf32(0x0039C));
                namedCharacterReferences.Add("NJcy;", char.ConvertFromUtf32(0x0040A));
                namedCharacterReferences.Add("Nacute;", char.ConvertFromUtf32(0x00143));
                namedCharacterReferences.Add("Ncaron;", char.ConvertFromUtf32(0x00147));
                namedCharacterReferences.Add("Ncedil;", char.ConvertFromUtf32(0x00145));
                namedCharacterReferences.Add("Ncy;", char.ConvertFromUtf32(0x0041D));
                namedCharacterReferences.Add("NegativeMediumSpace;", char.ConvertFromUtf32(0x0200B));
                namedCharacterReferences.Add("NegativeThickSpace;", char.ConvertFromUtf32(0x0200B));
                namedCharacterReferences.Add("NegativeThinSpace;", char.ConvertFromUtf32(0x0200B));
                namedCharacterReferences.Add("NegativeVeryThinSpace;", char.ConvertFromUtf32(0x0200B));
                namedCharacterReferences.Add("NestedGreaterGreater;", char.ConvertFromUtf32(0x0226B));
                namedCharacterReferences.Add("NestedLessLess;", char.ConvertFromUtf32(0x0226A));
                namedCharacterReferences.Add("NewLine;", char.ConvertFromUtf32(0x0000A));
                namedCharacterReferences.Add("Nfr;", char.ConvertFromUtf32(0x1D511));
                namedCharacterReferences.Add("NoBreak;", char.ConvertFromUtf32(0x02060));
                namedCharacterReferences.Add("NonBreakingSpace;", char.ConvertFromUtf32(0x000A0));
                namedCharacterReferences.Add("Nopf;", char.ConvertFromUtf32(0x02115));
                namedCharacterReferences.Add("Not;", char.ConvertFromUtf32(0x02AEC));
                namedCharacterReferences.Add("NotCongruent;", char.ConvertFromUtf32(0x02262));
                namedCharacterReferences.Add("NotCupCap;", char.ConvertFromUtf32(0x0226D));
                namedCharacterReferences.Add("NotDoubleVerticalBar;", char.ConvertFromUtf32(0x02226));
                namedCharacterReferences.Add("NotElement;", char.ConvertFromUtf32(0x02209));
                namedCharacterReferences.Add("NotEqual;", char.ConvertFromUtf32(0x02260));
                namedCharacterReferences.Add("NotExists;", char.ConvertFromUtf32(0x02204));
                namedCharacterReferences.Add("NotGreater;", char.ConvertFromUtf32(0x0226F));
                namedCharacterReferences.Add("NotGreaterEqual;", char.ConvertFromUtf32(0x02271));
                namedCharacterReferences.Add("NotGreaterLess;", char.ConvertFromUtf32(0x02279));
                namedCharacterReferences.Add("NotGreaterTilde;", char.ConvertFromUtf32(0x02275));
                namedCharacterReferences.Add("NotLeftTriangle;", char.ConvertFromUtf32(0x022EA));
                namedCharacterReferences.Add("NotLeftTriangleEqual;", char.ConvertFromUtf32(0x022EC));
                namedCharacterReferences.Add("NotLess;", char.ConvertFromUtf32(0x0226E));
                namedCharacterReferences.Add("NotLessEqual;", char.ConvertFromUtf32(0x02270));
                namedCharacterReferences.Add("NotLessGreater;", char.ConvertFromUtf32(0x02278));
                namedCharacterReferences.Add("NotLessTilde;", char.ConvertFromUtf32(0x02274));
                namedCharacterReferences.Add("NotPrecedes;", char.ConvertFromUtf32(0x02280));
                namedCharacterReferences.Add("NotPrecedesSlantEqual;", char.ConvertFromUtf32(0x022E0));
                namedCharacterReferences.Add("NotReverseElement;", char.ConvertFromUtf32(0x0220C));
                namedCharacterReferences.Add("NotRightTriangle;", char.ConvertFromUtf32(0x022EB));
                namedCharacterReferences.Add("NotRightTriangleEqual;", char.ConvertFromUtf32(0x022ED));
                namedCharacterReferences.Add("NotSquareSubsetEqual;", char.ConvertFromUtf32(0x022E2));
                namedCharacterReferences.Add("NotSquareSupersetEqual;", char.ConvertFromUtf32(0x022E3));
                namedCharacterReferences.Add("NotSubsetEqual;", char.ConvertFromUtf32(0x02288));
                namedCharacterReferences.Add("NotSucceeds;", char.ConvertFromUtf32(0x02281));
                namedCharacterReferences.Add("NotSucceedsSlantEqual;", char.ConvertFromUtf32(0x022E1));
                namedCharacterReferences.Add("NotSupersetEqual;", char.ConvertFromUtf32(0x02289));
                namedCharacterReferences.Add("NotTilde;", char.ConvertFromUtf32(0x02241));
                namedCharacterReferences.Add("NotTildeEqual;", char.ConvertFromUtf32(0x02244));
                namedCharacterReferences.Add("NotTildeFullEqual;", char.ConvertFromUtf32(0x02247));
                namedCharacterReferences.Add("NotTildeTilde;", char.ConvertFromUtf32(0x02249));
                namedCharacterReferences.Add("NotVerticalBar;", char.ConvertFromUtf32(0x02224));
                namedCharacterReferences.Add("Nscr;", char.ConvertFromUtf32(0x1D4A9));
                namedCharacterReferences.Add("Ntilde;", char.ConvertFromUtf32(0x000D1));
                namedCharacterReferences.Add("Ntilde", char.ConvertFromUtf32(0x000D1));
                namedCharacterReferences.Add("Nu;", char.ConvertFromUtf32(0x0039D));
                namedCharacterReferences.Add("OElig;", char.ConvertFromUtf32(0x00152));
                namedCharacterReferences.Add("Oacute;", char.ConvertFromUtf32(0x000D3));
                namedCharacterReferences.Add("Oacute", char.ConvertFromUtf32(0x000D3));
                namedCharacterReferences.Add("Ocirc;", char.ConvertFromUtf32(0x000D4));
                namedCharacterReferences.Add("Ocirc", char.ConvertFromUtf32(0x000D4));
                namedCharacterReferences.Add("Ocy;", char.ConvertFromUtf32(0x0041E));
                namedCharacterReferences.Add("Odblac;", char.ConvertFromUtf32(0x00150));
                namedCharacterReferences.Add("Ofr;", char.ConvertFromUtf32(0x1D512));
                namedCharacterReferences.Add("Ograve;", char.ConvertFromUtf32(0x000D2));
                namedCharacterReferences.Add("Ograve", char.ConvertFromUtf32(0x000D2));
                namedCharacterReferences.Add("Omacr;", char.ConvertFromUtf32(0x0014C));
                namedCharacterReferences.Add("Omega;", char.ConvertFromUtf32(0x003A9));
                namedCharacterReferences.Add("Omicron;", char.ConvertFromUtf32(0x0039F));
                namedCharacterReferences.Add("Oopf;", char.ConvertFromUtf32(0x1D546));
                namedCharacterReferences.Add("OpenCurlyDoubleQuote;", char.ConvertFromUtf32(0x0201C));
                namedCharacterReferences.Add("OpenCurlyQuote;", char.ConvertFromUtf32(0x02018));
                namedCharacterReferences.Add("Or;", char.ConvertFromUtf32(0x02A54));
                namedCharacterReferences.Add("Oscr;", char.ConvertFromUtf32(0x1D4AA));
                namedCharacterReferences.Add("Oslash;", char.ConvertFromUtf32(0x000D8));
                namedCharacterReferences.Add("Oslash", char.ConvertFromUtf32(0x000D8));
                namedCharacterReferences.Add("Otilde;", char.ConvertFromUtf32(0x000D5));
                namedCharacterReferences.Add("Otilde", char.ConvertFromUtf32(0x000D5));
                namedCharacterReferences.Add("Otimes;", char.ConvertFromUtf32(0x02A37));
                namedCharacterReferences.Add("Ouml;", char.ConvertFromUtf32(0x000D6));
                namedCharacterReferences.Add("Ouml", char.ConvertFromUtf32(0x000D6));
                namedCharacterReferences.Add("OverBar;", char.ConvertFromUtf32(0x0203E));
                namedCharacterReferences.Add("OverBrace;", char.ConvertFromUtf32(0x023DE));
                namedCharacterReferences.Add("OverBracket;", char.ConvertFromUtf32(0x023B4));
                namedCharacterReferences.Add("OverParenthesis;", char.ConvertFromUtf32(0x023DC));
                namedCharacterReferences.Add("PartialD;", char.ConvertFromUtf32(0x02202));
                namedCharacterReferences.Add("Pcy;", char.ConvertFromUtf32(0x0041F));
                namedCharacterReferences.Add("Pfr;", char.ConvertFromUtf32(0x1D513));
                namedCharacterReferences.Add("Phi;", char.ConvertFromUtf32(0x003A6));
                namedCharacterReferences.Add("Pi;", char.ConvertFromUtf32(0x003A0));
                namedCharacterReferences.Add("PlusMinus;", char.ConvertFromUtf32(0x000B1));
                namedCharacterReferences.Add("Poincareplane;", char.ConvertFromUtf32(0x0210C));
                namedCharacterReferences.Add("Popf;", char.ConvertFromUtf32(0x02119));
                namedCharacterReferences.Add("Pr;", char.ConvertFromUtf32(0x02ABB));
                namedCharacterReferences.Add("Precedes;", char.ConvertFromUtf32(0x0227A));
                namedCharacterReferences.Add("PrecedesEqual;", char.ConvertFromUtf32(0x02AAF));
                namedCharacterReferences.Add("PrecedesSlantEqual;", char.ConvertFromUtf32(0x0227C));
                namedCharacterReferences.Add("PrecedesTilde;", char.ConvertFromUtf32(0x0227E));
                namedCharacterReferences.Add("Prime;", char.ConvertFromUtf32(0x02033));
                namedCharacterReferences.Add("Product;", char.ConvertFromUtf32(0x0220F));
                namedCharacterReferences.Add("Proportion;", char.ConvertFromUtf32(0x02237));
                namedCharacterReferences.Add("Proportional;", char.ConvertFromUtf32(0x0221D));
                namedCharacterReferences.Add("Pscr;", char.ConvertFromUtf32(0x1D4AB));
                namedCharacterReferences.Add("Psi;", char.ConvertFromUtf32(0x003A8));
                namedCharacterReferences.Add("QUOT;", char.ConvertFromUtf32(0x00022));
                namedCharacterReferences.Add("QUOT", char.ConvertFromUtf32(0x00022));
                namedCharacterReferences.Add("Qfr;", char.ConvertFromUtf32(0x1D514));
                namedCharacterReferences.Add("Qopf;", char.ConvertFromUtf32(0x0211A));
                namedCharacterReferences.Add("Qscr;", char.ConvertFromUtf32(0x1D4AC));
                namedCharacterReferences.Add("RBarr;", char.ConvertFromUtf32(0x02910));
                namedCharacterReferences.Add("REG;", char.ConvertFromUtf32(0x000AE));
                namedCharacterReferences.Add("REG", char.ConvertFromUtf32(0x000AE));
                namedCharacterReferences.Add("Racute;", char.ConvertFromUtf32(0x00154));
                namedCharacterReferences.Add("Rang;", char.ConvertFromUtf32(0x027EB));
                namedCharacterReferences.Add("Rarr;", char.ConvertFromUtf32(0x021A0));
                namedCharacterReferences.Add("Rarrtl;", char.ConvertFromUtf32(0x02916));
                namedCharacterReferences.Add("Rcaron;", char.ConvertFromUtf32(0x00158));
                namedCharacterReferences.Add("Rcedil;", char.ConvertFromUtf32(0x00156));
                namedCharacterReferences.Add("Rcy;", char.ConvertFromUtf32(0x00420));
                namedCharacterReferences.Add("Re;", char.ConvertFromUtf32(0x0211C));
                namedCharacterReferences.Add("ReverseElement;", char.ConvertFromUtf32(0x0220B));
                namedCharacterReferences.Add("ReverseEquilibrium;", char.ConvertFromUtf32(0x021CB));
                namedCharacterReferences.Add("ReverseUpEquilibrium;", char.ConvertFromUtf32(0x0296F));
                namedCharacterReferences.Add("Rfr;", char.ConvertFromUtf32(0x0211C));
                namedCharacterReferences.Add("Rho;", char.ConvertFromUtf32(0x003A1));
                namedCharacterReferences.Add("RightAngleBracket;", char.ConvertFromUtf32(0x027E9));
                namedCharacterReferences.Add("RightArrow;", char.ConvertFromUtf32(0x02192));
                namedCharacterReferences.Add("RightArrowBar;", char.ConvertFromUtf32(0x021E5));
                namedCharacterReferences.Add("RightArrowLeftArrow;", char.ConvertFromUtf32(0x021C4));
                namedCharacterReferences.Add("RightCeiling;", char.ConvertFromUtf32(0x02309));
                namedCharacterReferences.Add("RightDoubleBracket;", char.ConvertFromUtf32(0x027E7));
                namedCharacterReferences.Add("RightDownTeeVector;", char.ConvertFromUtf32(0x0295D));
                namedCharacterReferences.Add("RightDownVector;", char.ConvertFromUtf32(0x021C2));
                namedCharacterReferences.Add("RightDownVectorBar;", char.ConvertFromUtf32(0x02955));
                namedCharacterReferences.Add("RightFloor;", char.ConvertFromUtf32(0x0230B));
                namedCharacterReferences.Add("RightTee;", char.ConvertFromUtf32(0x022A2));
                namedCharacterReferences.Add("RightTeeArrow;", char.ConvertFromUtf32(0x021A6));
                namedCharacterReferences.Add("RightTeeVector;", char.ConvertFromUtf32(0x0295B));
                namedCharacterReferences.Add("RightTriangle;", char.ConvertFromUtf32(0x022B3));
                namedCharacterReferences.Add("RightTriangleBar;", char.ConvertFromUtf32(0x029D0));
                namedCharacterReferences.Add("RightTriangleEqual;", char.ConvertFromUtf32(0x022B5));
                namedCharacterReferences.Add("RightUpDownVector;", char.ConvertFromUtf32(0x0294F));
                namedCharacterReferences.Add("RightUpTeeVector;", char.ConvertFromUtf32(0x0295C));
                namedCharacterReferences.Add("RightUpVector;", char.ConvertFromUtf32(0x021BE));
                namedCharacterReferences.Add("RightUpVectorBar;", char.ConvertFromUtf32(0x02954));
                namedCharacterReferences.Add("RightVector;", char.ConvertFromUtf32(0x021C0));
                namedCharacterReferences.Add("RightVectorBar;", char.ConvertFromUtf32(0x02953));
                namedCharacterReferences.Add("Rightarrow;", char.ConvertFromUtf32(0x021D2));
                namedCharacterReferences.Add("Ropf;", char.ConvertFromUtf32(0x0211D));
                namedCharacterReferences.Add("RoundImplies;", char.ConvertFromUtf32(0x02970));
                namedCharacterReferences.Add("Rrightarrow;", char.ConvertFromUtf32(0x021DB));
                namedCharacterReferences.Add("Rscr;", char.ConvertFromUtf32(0x0211B));
                namedCharacterReferences.Add("Rsh;", char.ConvertFromUtf32(0x021B1));
                namedCharacterReferences.Add("RuleDelayed;", char.ConvertFromUtf32(0x029F4));
                namedCharacterReferences.Add("SHCHcy;", char.ConvertFromUtf32(0x00429));
                namedCharacterReferences.Add("SHcy;", char.ConvertFromUtf32(0x00428));
                namedCharacterReferences.Add("SOFTcy;", char.ConvertFromUtf32(0x0042C));
                namedCharacterReferences.Add("Sacute;", char.ConvertFromUtf32(0x0015A));
                namedCharacterReferences.Add("Sc;", char.ConvertFromUtf32(0x02ABC));
                namedCharacterReferences.Add("Scaron;", char.ConvertFromUtf32(0x00160));
                namedCharacterReferences.Add("Scedil;", char.ConvertFromUtf32(0x0015E));
                namedCharacterReferences.Add("Scirc;", char.ConvertFromUtf32(0x0015C));
                namedCharacterReferences.Add("Scy;", char.ConvertFromUtf32(0x00421));
                namedCharacterReferences.Add("Sfr;", char.ConvertFromUtf32(0x1D516));
                namedCharacterReferences.Add("ShortDownArrow;", char.ConvertFromUtf32(0x02193));
                namedCharacterReferences.Add("ShortLeftArrow;", char.ConvertFromUtf32(0x02190));
                namedCharacterReferences.Add("ShortRightArrow;", char.ConvertFromUtf32(0x02192));
                namedCharacterReferences.Add("ShortUpArrow;", char.ConvertFromUtf32(0x02191));
                namedCharacterReferences.Add("Sigma;", char.ConvertFromUtf32(0x003A3));
                namedCharacterReferences.Add("SmallCircle;", char.ConvertFromUtf32(0x02218));
                namedCharacterReferences.Add("Sopf;", char.ConvertFromUtf32(0x1D54A));
                namedCharacterReferences.Add("Sqrt;", char.ConvertFromUtf32(0x0221A));
                namedCharacterReferences.Add("Square;", char.ConvertFromUtf32(0x025A1));
                namedCharacterReferences.Add("SquareIntersection;", char.ConvertFromUtf32(0x02293));
                namedCharacterReferences.Add("SquareSubset;", char.ConvertFromUtf32(0x0228F));
                namedCharacterReferences.Add("SquareSubsetEqual;", char.ConvertFromUtf32(0x02291));
                namedCharacterReferences.Add("SquareSuperset;", char.ConvertFromUtf32(0x02290));
                namedCharacterReferences.Add("SquareSupersetEqual;", char.ConvertFromUtf32(0x02292));
                namedCharacterReferences.Add("SquareUnion;", char.ConvertFromUtf32(0x02294));
                namedCharacterReferences.Add("Sscr;", char.ConvertFromUtf32(0x1D4AE));
                namedCharacterReferences.Add("Star;", char.ConvertFromUtf32(0x022C6));
                namedCharacterReferences.Add("Sub;", char.ConvertFromUtf32(0x022D0));
                namedCharacterReferences.Add("Subset;", char.ConvertFromUtf32(0x022D0));
                namedCharacterReferences.Add("SubsetEqual;", char.ConvertFromUtf32(0x02286));
                namedCharacterReferences.Add("Succeeds;", char.ConvertFromUtf32(0x0227B));
                namedCharacterReferences.Add("SucceedsEqual;", char.ConvertFromUtf32(0x02AB0));
                namedCharacterReferences.Add("SucceedsSlantEqual;", char.ConvertFromUtf32(0x0227D));
                namedCharacterReferences.Add("SucceedsTilde;", char.ConvertFromUtf32(0x0227F));
                namedCharacterReferences.Add("SuchThat;", char.ConvertFromUtf32(0x0220B));
                namedCharacterReferences.Add("Sum;", char.ConvertFromUtf32(0x02211));
                namedCharacterReferences.Add("Sup;", char.ConvertFromUtf32(0x022D1));
                namedCharacterReferences.Add("Superset;", char.ConvertFromUtf32(0x02283));
                namedCharacterReferences.Add("SupersetEqual;", char.ConvertFromUtf32(0x02287));
                namedCharacterReferences.Add("Supset;", char.ConvertFromUtf32(0x022D1));
                namedCharacterReferences.Add("THORN;", char.ConvertFromUtf32(0x000DE));
                namedCharacterReferences.Add("THORN", char.ConvertFromUtf32(0x000DE));
                namedCharacterReferences.Add("TRADE;", char.ConvertFromUtf32(0x02122));
                namedCharacterReferences.Add("TSHcy;", char.ConvertFromUtf32(0x0040B));
                namedCharacterReferences.Add("TScy;", char.ConvertFromUtf32(0x00426));
                namedCharacterReferences.Add("Tab;", char.ConvertFromUtf32(0x00009));
                namedCharacterReferences.Add("Tau;", char.ConvertFromUtf32(0x003A4));
                namedCharacterReferences.Add("Tcaron;", char.ConvertFromUtf32(0x00164));
                namedCharacterReferences.Add("Tcedil;", char.ConvertFromUtf32(0x00162));
                namedCharacterReferences.Add("Tcy;", char.ConvertFromUtf32(0x00422));
                namedCharacterReferences.Add("Tfr;", char.ConvertFromUtf32(0x1D517));
                namedCharacterReferences.Add("Therefore;", char.ConvertFromUtf32(0x02234));
                namedCharacterReferences.Add("Theta;", char.ConvertFromUtf32(0x00398));
                namedCharacterReferences.Add("ThinSpace;", char.ConvertFromUtf32(0x02009));
                namedCharacterReferences.Add("Tilde;", char.ConvertFromUtf32(0x0223C));
                namedCharacterReferences.Add("TildeEqual;", char.ConvertFromUtf32(0x02243));
                namedCharacterReferences.Add("TildeFullEqual;", char.ConvertFromUtf32(0x02245));
                namedCharacterReferences.Add("TildeTilde;", char.ConvertFromUtf32(0x02248));
                namedCharacterReferences.Add("Topf;", char.ConvertFromUtf32(0x1D54B));
                namedCharacterReferences.Add("TripleDot;", char.ConvertFromUtf32(0x020DB));
                namedCharacterReferences.Add("Tscr;", char.ConvertFromUtf32(0x1D4AF));
                namedCharacterReferences.Add("Tstrok;", char.ConvertFromUtf32(0x00166));
                namedCharacterReferences.Add("Uacute;", char.ConvertFromUtf32(0x000DA));
                namedCharacterReferences.Add("Uacute", char.ConvertFromUtf32(0x000DA));
                namedCharacterReferences.Add("Uarr;", char.ConvertFromUtf32(0x0219F));
                namedCharacterReferences.Add("Uarrocir;", char.ConvertFromUtf32(0x02949));
                namedCharacterReferences.Add("Ubrcy;", char.ConvertFromUtf32(0x0040E));
                namedCharacterReferences.Add("Ubreve;", char.ConvertFromUtf32(0x0016C));
                namedCharacterReferences.Add("Ucirc;", char.ConvertFromUtf32(0x000DB));
                namedCharacterReferences.Add("Ucirc", char.ConvertFromUtf32(0x000DB));
                namedCharacterReferences.Add("Ucy;", char.ConvertFromUtf32(0x00423));
                namedCharacterReferences.Add("Udblac;", char.ConvertFromUtf32(0x00170));
                namedCharacterReferences.Add("Ufr;", char.ConvertFromUtf32(0x1D518));
                namedCharacterReferences.Add("Ugrave;", char.ConvertFromUtf32(0x000D9));
                namedCharacterReferences.Add("Ugrave", char.ConvertFromUtf32(0x000D9));
                namedCharacterReferences.Add("Umacr;", char.ConvertFromUtf32(0x0016A));
                namedCharacterReferences.Add("UnderBar;", char.ConvertFromUtf32(0x0005F));
                namedCharacterReferences.Add("UnderBrace;", char.ConvertFromUtf32(0x023DF));
                namedCharacterReferences.Add("UnderBracket;", char.ConvertFromUtf32(0x023B5));
                namedCharacterReferences.Add("UnderParenthesis;", char.ConvertFromUtf32(0x023DD));
                namedCharacterReferences.Add("Union;", char.ConvertFromUtf32(0x022C3));
                namedCharacterReferences.Add("UnionPlus;", char.ConvertFromUtf32(0x0228E));
                namedCharacterReferences.Add("Uogon;", char.ConvertFromUtf32(0x00172));
                namedCharacterReferences.Add("Uopf;", char.ConvertFromUtf32(0x1D54C));
                namedCharacterReferences.Add("UpArrow;", char.ConvertFromUtf32(0x02191));
                namedCharacterReferences.Add("UpArrowBar;", char.ConvertFromUtf32(0x02912));
                namedCharacterReferences.Add("UpArrowDownArrow;", char.ConvertFromUtf32(0x021C5));
                namedCharacterReferences.Add("UpDownArrow;", char.ConvertFromUtf32(0x02195));
                namedCharacterReferences.Add("UpEquilibrium;", char.ConvertFromUtf32(0x0296E));
                namedCharacterReferences.Add("UpTee;", char.ConvertFromUtf32(0x022A5));
                namedCharacterReferences.Add("UpTeeArrow;", char.ConvertFromUtf32(0x021A5));
                namedCharacterReferences.Add("Uparrow;", char.ConvertFromUtf32(0x021D1));
                namedCharacterReferences.Add("Updownarrow;", char.ConvertFromUtf32(0x021D5));
                namedCharacterReferences.Add("UpperLeftArrow;", char.ConvertFromUtf32(0x02196));
                namedCharacterReferences.Add("UpperRightArrow;", char.ConvertFromUtf32(0x02197));
                namedCharacterReferences.Add("Upsi;", char.ConvertFromUtf32(0x003D2));
                namedCharacterReferences.Add("Upsilon;", char.ConvertFromUtf32(0x003A5));
                namedCharacterReferences.Add("Uring;", char.ConvertFromUtf32(0x0016E));
                namedCharacterReferences.Add("Uscr;", char.ConvertFromUtf32(0x1D4B0));
                namedCharacterReferences.Add("Utilde;", char.ConvertFromUtf32(0x00168));
                namedCharacterReferences.Add("Uuml;", char.ConvertFromUtf32(0x000DC));
                namedCharacterReferences.Add("Uuml", char.ConvertFromUtf32(0x000DC));
                namedCharacterReferences.Add("VDash;", char.ConvertFromUtf32(0x022AB));
                namedCharacterReferences.Add("Vbar;", char.ConvertFromUtf32(0x02AEB));
                namedCharacterReferences.Add("Vcy;", char.ConvertFromUtf32(0x00412));
                namedCharacterReferences.Add("Vdash;", char.ConvertFromUtf32(0x022A9));
                namedCharacterReferences.Add("Vdashl;", char.ConvertFromUtf32(0x02AE6));
                namedCharacterReferences.Add("Vee;", char.ConvertFromUtf32(0x022C1));
                namedCharacterReferences.Add("Verbar;", char.ConvertFromUtf32(0x02016));
                namedCharacterReferences.Add("Vert;", char.ConvertFromUtf32(0x02016));
                namedCharacterReferences.Add("VerticalBar;", char.ConvertFromUtf32(0x02223));
                namedCharacterReferences.Add("VerticalLine;", char.ConvertFromUtf32(0x0007C));
                namedCharacterReferences.Add("VerticalSeparator;", char.ConvertFromUtf32(0x02758));
                namedCharacterReferences.Add("VerticalTilde;", char.ConvertFromUtf32(0x02240));
                namedCharacterReferences.Add("VeryThinSpace;", char.ConvertFromUtf32(0x0200A));
                namedCharacterReferences.Add("Vfr;", char.ConvertFromUtf32(0x1D519));
                namedCharacterReferences.Add("Vopf;", char.ConvertFromUtf32(0x1D54D));
                namedCharacterReferences.Add("Vscr;", char.ConvertFromUtf32(0x1D4B1));
                namedCharacterReferences.Add("Vvdash;", char.ConvertFromUtf32(0x022AA));
                namedCharacterReferences.Add("Wcirc;", char.ConvertFromUtf32(0x00174));
                namedCharacterReferences.Add("Wedge;", char.ConvertFromUtf32(0x022C0));
                namedCharacterReferences.Add("Wfr;", char.ConvertFromUtf32(0x1D51A));
                namedCharacterReferences.Add("Wopf;", char.ConvertFromUtf32(0x1D54E));
                namedCharacterReferences.Add("Wscr;", char.ConvertFromUtf32(0x1D4B2));
                namedCharacterReferences.Add("Xfr;", char.ConvertFromUtf32(0x1D51B));
                namedCharacterReferences.Add("Xi;", char.ConvertFromUtf32(0x0039E));
                namedCharacterReferences.Add("Xopf;", char.ConvertFromUtf32(0x1D54F));
                namedCharacterReferences.Add("Xscr;", char.ConvertFromUtf32(0x1D4B3));
                namedCharacterReferences.Add("YAcy;", char.ConvertFromUtf32(0x0042F));
                namedCharacterReferences.Add("YIcy;", char.ConvertFromUtf32(0x00407));
                namedCharacterReferences.Add("YUcy;", char.ConvertFromUtf32(0x0042E));
                namedCharacterReferences.Add("Yacute;", char.ConvertFromUtf32(0x000DD));
                namedCharacterReferences.Add("Yacute", char.ConvertFromUtf32(0x000DD));
                namedCharacterReferences.Add("Ycirc;", char.ConvertFromUtf32(0x00176));
                namedCharacterReferences.Add("Ycy;", char.ConvertFromUtf32(0x0042B));
                namedCharacterReferences.Add("Yfr;", char.ConvertFromUtf32(0x1D51C));
                namedCharacterReferences.Add("Yopf;", char.ConvertFromUtf32(0x1D550));
                namedCharacterReferences.Add("Yscr;", char.ConvertFromUtf32(0x1D4B4));
                namedCharacterReferences.Add("Yuml;", char.ConvertFromUtf32(0x00178));
                namedCharacterReferences.Add("ZHcy;", char.ConvertFromUtf32(0x00416));
                namedCharacterReferences.Add("Zacute;", char.ConvertFromUtf32(0x00179));
                namedCharacterReferences.Add("Zcaron;", char.ConvertFromUtf32(0x0017D));
                namedCharacterReferences.Add("Zcy;", char.ConvertFromUtf32(0x00417));
                namedCharacterReferences.Add("Zdot;", char.ConvertFromUtf32(0x0017B));
                namedCharacterReferences.Add("ZeroWidthSpace;", char.ConvertFromUtf32(0x0200B));
                namedCharacterReferences.Add("Zeta;", char.ConvertFromUtf32(0x00396));
                namedCharacterReferences.Add("Zfr;", char.ConvertFromUtf32(0x02128));
                namedCharacterReferences.Add("Zopf;", char.ConvertFromUtf32(0x02124));
                namedCharacterReferences.Add("Zscr;", char.ConvertFromUtf32(0x1D4B5));
                namedCharacterReferences.Add("aacute;", char.ConvertFromUtf32(0x000E1));
                namedCharacterReferences.Add("aacute", char.ConvertFromUtf32(0x000E1));
                namedCharacterReferences.Add("abreve;", char.ConvertFromUtf32(0x00103));
                namedCharacterReferences.Add("ac;", char.ConvertFromUtf32(0x0223E));
                namedCharacterReferences.Add("acd;", char.ConvertFromUtf32(0x0223F));
                namedCharacterReferences.Add("acirc;", char.ConvertFromUtf32(0x000E2));
                namedCharacterReferences.Add("acirc", char.ConvertFromUtf32(0x000E2));
                namedCharacterReferences.Add("acute;", char.ConvertFromUtf32(0x000B4));
                namedCharacterReferences.Add("acute", char.ConvertFromUtf32(0x000B4));
                namedCharacterReferences.Add("acy;", char.ConvertFromUtf32(0x00430));
                namedCharacterReferences.Add("aelig;", char.ConvertFromUtf32(0x000E6));
                namedCharacterReferences.Add("aelig", char.ConvertFromUtf32(0x000E6));
                namedCharacterReferences.Add("af;", char.ConvertFromUtf32(0x02061));
                namedCharacterReferences.Add("afr;", char.ConvertFromUtf32(0x1D51E));
                namedCharacterReferences.Add("agrave;", char.ConvertFromUtf32(0x000E0));
                namedCharacterReferences.Add("agrave", char.ConvertFromUtf32(0x000E0));
                namedCharacterReferences.Add("alefsym;", char.ConvertFromUtf32(0x02135));
                namedCharacterReferences.Add("aleph;", char.ConvertFromUtf32(0x02135));
                namedCharacterReferences.Add("alpha;", char.ConvertFromUtf32(0x003B1));
                namedCharacterReferences.Add("amacr;", char.ConvertFromUtf32(0x00101));
                namedCharacterReferences.Add("amalg;", char.ConvertFromUtf32(0x02A3F));
                namedCharacterReferences.Add("amp;", char.ConvertFromUtf32(0x00026));
                namedCharacterReferences.Add("amp", char.ConvertFromUtf32(0x00026));
                namedCharacterReferences.Add("and;", char.ConvertFromUtf32(0x02227));
                namedCharacterReferences.Add("andand;", char.ConvertFromUtf32(0x02A55));
                namedCharacterReferences.Add("andd;", char.ConvertFromUtf32(0x02A5C));
                namedCharacterReferences.Add("andslope;", char.ConvertFromUtf32(0x02A58));
                namedCharacterReferences.Add("andv;", char.ConvertFromUtf32(0x02A5A));
                namedCharacterReferences.Add("ang;", char.ConvertFromUtf32(0x02220));
                namedCharacterReferences.Add("ange;", char.ConvertFromUtf32(0x029A4));
                namedCharacterReferences.Add("angle;", char.ConvertFromUtf32(0x02220));
                namedCharacterReferences.Add("angmsd;", char.ConvertFromUtf32(0x02221));
                namedCharacterReferences.Add("angmsdaa;", char.ConvertFromUtf32(0x029A8));
                namedCharacterReferences.Add("angmsdab;", char.ConvertFromUtf32(0x029A9));
                namedCharacterReferences.Add("angmsdac;", char.ConvertFromUtf32(0x029AA));
                namedCharacterReferences.Add("angmsdad;", char.ConvertFromUtf32(0x029AB));
                namedCharacterReferences.Add("angmsdae;", char.ConvertFromUtf32(0x029AC));
                namedCharacterReferences.Add("angmsdaf;", char.ConvertFromUtf32(0x029AD));
                namedCharacterReferences.Add("angmsdag;", char.ConvertFromUtf32(0x029AE));
                namedCharacterReferences.Add("angmsdah;", char.ConvertFromUtf32(0x029AF));
                namedCharacterReferences.Add("angrt;", char.ConvertFromUtf32(0x0221F));
                namedCharacterReferences.Add("angrtvb;", char.ConvertFromUtf32(0x022BE));
                namedCharacterReferences.Add("angrtvbd;", char.ConvertFromUtf32(0x0299D));
                namedCharacterReferences.Add("angsph;", char.ConvertFromUtf32(0x02222));
                namedCharacterReferences.Add("angst;", char.ConvertFromUtf32(0x000C5));
                namedCharacterReferences.Add("angzarr;", char.ConvertFromUtf32(0x0237C));
                namedCharacterReferences.Add("aogon;", char.ConvertFromUtf32(0x00105));
                namedCharacterReferences.Add("aopf;", char.ConvertFromUtf32(0x1D552));
                namedCharacterReferences.Add("ap;", char.ConvertFromUtf32(0x02248));
                namedCharacterReferences.Add("apE;", char.ConvertFromUtf32(0x02A70));
                namedCharacterReferences.Add("apacir;", char.ConvertFromUtf32(0x02A6F));
                namedCharacterReferences.Add("ape;", char.ConvertFromUtf32(0x0224A));
                namedCharacterReferences.Add("apid;", char.ConvertFromUtf32(0x0224B));
                namedCharacterReferences.Add("apos;", char.ConvertFromUtf32(0x00027));
                namedCharacterReferences.Add("approx;", char.ConvertFromUtf32(0x02248));
                namedCharacterReferences.Add("approxeq;", char.ConvertFromUtf32(0x0224A));
                namedCharacterReferences.Add("aring;", char.ConvertFromUtf32(0x000E5));
                namedCharacterReferences.Add("aring", char.ConvertFromUtf32(0x000E5));
                namedCharacterReferences.Add("ascr;", char.ConvertFromUtf32(0x1D4B6));
                namedCharacterReferences.Add("ast;", char.ConvertFromUtf32(0x0002A));
                namedCharacterReferences.Add("asymp;", char.ConvertFromUtf32(0x02248));
                namedCharacterReferences.Add("asympeq;", char.ConvertFromUtf32(0x0224D));
                namedCharacterReferences.Add("atilde;", char.ConvertFromUtf32(0x000E3));
                namedCharacterReferences.Add("atilde", char.ConvertFromUtf32(0x000E3));
                namedCharacterReferences.Add("auml;", char.ConvertFromUtf32(0x000E4));
                namedCharacterReferences.Add("auml", char.ConvertFromUtf32(0x000E4));
                namedCharacterReferences.Add("awconint;", char.ConvertFromUtf32(0x02233));
                namedCharacterReferences.Add("awint;", char.ConvertFromUtf32(0x02A11));
                namedCharacterReferences.Add("bNot;", char.ConvertFromUtf32(0x02AED));
                namedCharacterReferences.Add("backcong;", char.ConvertFromUtf32(0x0224C));
                namedCharacterReferences.Add("backepsilon;", char.ConvertFromUtf32(0x003F6));
                namedCharacterReferences.Add("backprime;", char.ConvertFromUtf32(0x02035));
                namedCharacterReferences.Add("backsim;", char.ConvertFromUtf32(0x0223D));
                namedCharacterReferences.Add("backsimeq;", char.ConvertFromUtf32(0x022CD));
                namedCharacterReferences.Add("barvee;", char.ConvertFromUtf32(0x022BD));
                namedCharacterReferences.Add("barwed;", char.ConvertFromUtf32(0x02305));
                namedCharacterReferences.Add("barwedge;", char.ConvertFromUtf32(0x02305));
                namedCharacterReferences.Add("bbrk;", char.ConvertFromUtf32(0x023B5));
                namedCharacterReferences.Add("bbrktbrk;", char.ConvertFromUtf32(0x023B6));
                namedCharacterReferences.Add("bcong;", char.ConvertFromUtf32(0x0224C));
                namedCharacterReferences.Add("bcy;", char.ConvertFromUtf32(0x00431));
                namedCharacterReferences.Add("bdquo;", char.ConvertFromUtf32(0x0201E));
                namedCharacterReferences.Add("becaus;", char.ConvertFromUtf32(0x02235));
                namedCharacterReferences.Add("because;", char.ConvertFromUtf32(0x02235));
                namedCharacterReferences.Add("bemptyv;", char.ConvertFromUtf32(0x029B0));
                namedCharacterReferences.Add("bepsi;", char.ConvertFromUtf32(0x003F6));
                namedCharacterReferences.Add("bernou;", char.ConvertFromUtf32(0x0212C));
                namedCharacterReferences.Add("beta;", char.ConvertFromUtf32(0x003B2));
                namedCharacterReferences.Add("beth;", char.ConvertFromUtf32(0x02136));
                namedCharacterReferences.Add("between;", char.ConvertFromUtf32(0x0226C));
                namedCharacterReferences.Add("bfr;", char.ConvertFromUtf32(0x1D51F));
                namedCharacterReferences.Add("bigcap;", char.ConvertFromUtf32(0x022C2));
                namedCharacterReferences.Add("bigcirc;", char.ConvertFromUtf32(0x025EF));
                namedCharacterReferences.Add("bigcup;", char.ConvertFromUtf32(0x022C3));
                namedCharacterReferences.Add("bigodot;", char.ConvertFromUtf32(0x02A00));
                namedCharacterReferences.Add("bigoplus;", char.ConvertFromUtf32(0x02A01));
                namedCharacterReferences.Add("bigotimes;", char.ConvertFromUtf32(0x02A02));
                namedCharacterReferences.Add("bigsqcup;", char.ConvertFromUtf32(0x02A06));
                namedCharacterReferences.Add("bigstar;", char.ConvertFromUtf32(0x02605));
                namedCharacterReferences.Add("bigtriangledown;", char.ConvertFromUtf32(0x025BD));
                namedCharacterReferences.Add("bigtriangleup;", char.ConvertFromUtf32(0x025B3));
                namedCharacterReferences.Add("biguplus;", char.ConvertFromUtf32(0x02A04));
                namedCharacterReferences.Add("bigvee;", char.ConvertFromUtf32(0x022C1));
                namedCharacterReferences.Add("bigwedge;", char.ConvertFromUtf32(0x022C0));
                namedCharacterReferences.Add("bkarow;", char.ConvertFromUtf32(0x0290D));
                namedCharacterReferences.Add("blacklozenge;", char.ConvertFromUtf32(0x029EB));
                namedCharacterReferences.Add("blacksquare;", char.ConvertFromUtf32(0x025AA));
                namedCharacterReferences.Add("blacktriangle;", char.ConvertFromUtf32(0x025B4));
                namedCharacterReferences.Add("blacktriangledown;", char.ConvertFromUtf32(0x025BE));
                namedCharacterReferences.Add("blacktriangleleft;", char.ConvertFromUtf32(0x025C2));
                namedCharacterReferences.Add("blacktriangleright;", char.ConvertFromUtf32(0x025B8));
                namedCharacterReferences.Add("blank;", char.ConvertFromUtf32(0x02423));
                namedCharacterReferences.Add("blk12;", char.ConvertFromUtf32(0x02592));
                namedCharacterReferences.Add("blk14;", char.ConvertFromUtf32(0x02591));
                namedCharacterReferences.Add("blk34;", char.ConvertFromUtf32(0x02593));
                namedCharacterReferences.Add("block;", char.ConvertFromUtf32(0x02588));
                namedCharacterReferences.Add("bnot;", char.ConvertFromUtf32(0x02310));
                namedCharacterReferences.Add("bopf;", char.ConvertFromUtf32(0x1D553));
                namedCharacterReferences.Add("bot;", char.ConvertFromUtf32(0x022A5));
                namedCharacterReferences.Add("bottom;", char.ConvertFromUtf32(0x022A5));
                namedCharacterReferences.Add("bowtie;", char.ConvertFromUtf32(0x022C8));
                namedCharacterReferences.Add("boxDL;", char.ConvertFromUtf32(0x02557));
                namedCharacterReferences.Add("boxDR;", char.ConvertFromUtf32(0x02554));
                namedCharacterReferences.Add("boxDl;", char.ConvertFromUtf32(0x02556));
                namedCharacterReferences.Add("boxDr;", char.ConvertFromUtf32(0x02553));
                namedCharacterReferences.Add("boxH;", char.ConvertFromUtf32(0x02550));
                namedCharacterReferences.Add("boxHD;", char.ConvertFromUtf32(0x02566));
                namedCharacterReferences.Add("boxHU;", char.ConvertFromUtf32(0x02569));
                namedCharacterReferences.Add("boxHd;", char.ConvertFromUtf32(0x02564));
                namedCharacterReferences.Add("boxHu;", char.ConvertFromUtf32(0x02567));
                namedCharacterReferences.Add("boxUL;", char.ConvertFromUtf32(0x0255D));
                namedCharacterReferences.Add("boxUR;", char.ConvertFromUtf32(0x0255A));
                namedCharacterReferences.Add("boxUl;", char.ConvertFromUtf32(0x0255C));
                namedCharacterReferences.Add("boxUr;", char.ConvertFromUtf32(0x02559));
                namedCharacterReferences.Add("boxV;", char.ConvertFromUtf32(0x02551));
                namedCharacterReferences.Add("boxVH;", char.ConvertFromUtf32(0x0256C));
                namedCharacterReferences.Add("boxVL;", char.ConvertFromUtf32(0x02563));
                namedCharacterReferences.Add("boxVR;", char.ConvertFromUtf32(0x02560));
                namedCharacterReferences.Add("boxVh;", char.ConvertFromUtf32(0x0256B));
                namedCharacterReferences.Add("boxVl;", char.ConvertFromUtf32(0x02562));
                namedCharacterReferences.Add("boxVr;", char.ConvertFromUtf32(0x0255F));
                namedCharacterReferences.Add("boxbox;", char.ConvertFromUtf32(0x029C9));
                namedCharacterReferences.Add("boxdL;", char.ConvertFromUtf32(0x02555));
                namedCharacterReferences.Add("boxdR;", char.ConvertFromUtf32(0x02552));
                namedCharacterReferences.Add("boxdl;", char.ConvertFromUtf32(0x02510));
                namedCharacterReferences.Add("boxdr;", char.ConvertFromUtf32(0x0250C));
                namedCharacterReferences.Add("boxh;", char.ConvertFromUtf32(0x02500));
                namedCharacterReferences.Add("boxhD;", char.ConvertFromUtf32(0x02565));
                namedCharacterReferences.Add("boxhU;", char.ConvertFromUtf32(0x02568));
                namedCharacterReferences.Add("boxhd;", char.ConvertFromUtf32(0x0252C));
                namedCharacterReferences.Add("boxhu;", char.ConvertFromUtf32(0x02534));
                namedCharacterReferences.Add("boxminus;", char.ConvertFromUtf32(0x0229F));
                namedCharacterReferences.Add("boxplus;", char.ConvertFromUtf32(0x0229E));
                namedCharacterReferences.Add("boxtimes;", char.ConvertFromUtf32(0x022A0));
                namedCharacterReferences.Add("boxuL;", char.ConvertFromUtf32(0x0255B));
                namedCharacterReferences.Add("boxuR;", char.ConvertFromUtf32(0x02558));
                namedCharacterReferences.Add("boxul;", char.ConvertFromUtf32(0x02518));
                namedCharacterReferences.Add("boxur;", char.ConvertFromUtf32(0x02514));
                namedCharacterReferences.Add("boxv;", char.ConvertFromUtf32(0x02502));
                namedCharacterReferences.Add("boxvH;", char.ConvertFromUtf32(0x0256A));
                namedCharacterReferences.Add("boxvL;", char.ConvertFromUtf32(0x02561));
                namedCharacterReferences.Add("boxvR;", char.ConvertFromUtf32(0x0255E));
                namedCharacterReferences.Add("boxvh;", char.ConvertFromUtf32(0x0253C));
                namedCharacterReferences.Add("boxvl;", char.ConvertFromUtf32(0x02524));
                namedCharacterReferences.Add("boxvr;", char.ConvertFromUtf32(0x0251C));
                namedCharacterReferences.Add("bprime;", char.ConvertFromUtf32(0x02035));
                namedCharacterReferences.Add("breve;", char.ConvertFromUtf32(0x002D8));
                namedCharacterReferences.Add("brvbar;", char.ConvertFromUtf32(0x000A6));
                namedCharacterReferences.Add("brvbar", char.ConvertFromUtf32(0x000A6));
                namedCharacterReferences.Add("bscr;", char.ConvertFromUtf32(0x1D4B7));
                namedCharacterReferences.Add("bsemi;", char.ConvertFromUtf32(0x0204F));
                namedCharacterReferences.Add("bsim;", char.ConvertFromUtf32(0x0223D));
                namedCharacterReferences.Add("bsime;", char.ConvertFromUtf32(0x022CD));
                namedCharacterReferences.Add("bsol;", char.ConvertFromUtf32(0x0005C));
                namedCharacterReferences.Add("bsolb;", char.ConvertFromUtf32(0x029C5));
                namedCharacterReferences.Add("bsolhsub;", char.ConvertFromUtf32(0x027C8));
                namedCharacterReferences.Add("bull;", char.ConvertFromUtf32(0x02022));
                namedCharacterReferences.Add("bullet;", char.ConvertFromUtf32(0x02022));
                namedCharacterReferences.Add("bump;", char.ConvertFromUtf32(0x0224E));
                namedCharacterReferences.Add("bumpE;", char.ConvertFromUtf32(0x02AAE));
                namedCharacterReferences.Add("bumpe;", char.ConvertFromUtf32(0x0224F));
                namedCharacterReferences.Add("bumpeq;", char.ConvertFromUtf32(0x0224F));
                namedCharacterReferences.Add("cacute;", char.ConvertFromUtf32(0x00107));
                namedCharacterReferences.Add("cap;", char.ConvertFromUtf32(0x02229));
                namedCharacterReferences.Add("capand;", char.ConvertFromUtf32(0x02A44));
                namedCharacterReferences.Add("capbrcup;", char.ConvertFromUtf32(0x02A49));
                namedCharacterReferences.Add("capcap;", char.ConvertFromUtf32(0x02A4B));
                namedCharacterReferences.Add("capcup;", char.ConvertFromUtf32(0x02A47));
                namedCharacterReferences.Add("capdot;", char.ConvertFromUtf32(0x02A40));
                namedCharacterReferences.Add("caret;", char.ConvertFromUtf32(0x02041));
                namedCharacterReferences.Add("caron;", char.ConvertFromUtf32(0x002C7));
                namedCharacterReferences.Add("ccaps;", char.ConvertFromUtf32(0x02A4D));
                namedCharacterReferences.Add("ccaron;", char.ConvertFromUtf32(0x0010D));
                namedCharacterReferences.Add("ccedil;", char.ConvertFromUtf32(0x000E7));
                namedCharacterReferences.Add("ccedil", char.ConvertFromUtf32(0x000E7));
                namedCharacterReferences.Add("ccirc;", char.ConvertFromUtf32(0x00109));
                namedCharacterReferences.Add("ccups;", char.ConvertFromUtf32(0x02A4C));
                namedCharacterReferences.Add("ccupssm;", char.ConvertFromUtf32(0x02A50));
                namedCharacterReferences.Add("cdot;", char.ConvertFromUtf32(0x0010B));
                namedCharacterReferences.Add("cedil;", char.ConvertFromUtf32(0x000B8));
                namedCharacterReferences.Add("cedil", char.ConvertFromUtf32(0x000B8));
                namedCharacterReferences.Add("cemptyv;", char.ConvertFromUtf32(0x029B2));
                namedCharacterReferences.Add("cent;", char.ConvertFromUtf32(0x000A2));
                namedCharacterReferences.Add("cent", char.ConvertFromUtf32(0x000A2));
                namedCharacterReferences.Add("centerdot;", char.ConvertFromUtf32(0x000B7));
                namedCharacterReferences.Add("cfr;", char.ConvertFromUtf32(0x1D520));
                namedCharacterReferences.Add("chcy;", char.ConvertFromUtf32(0x00447));
                namedCharacterReferences.Add("check;", char.ConvertFromUtf32(0x02713));
                namedCharacterReferences.Add("checkmark;", char.ConvertFromUtf32(0x02713));
                namedCharacterReferences.Add("chi;", char.ConvertFromUtf32(0x003C7));
                namedCharacterReferences.Add("cir;", char.ConvertFromUtf32(0x025CB));
                namedCharacterReferences.Add("cirE;", char.ConvertFromUtf32(0x029C3));
                namedCharacterReferences.Add("circ;", char.ConvertFromUtf32(0x002C6));
                namedCharacterReferences.Add("circeq;", char.ConvertFromUtf32(0x02257));
                namedCharacterReferences.Add("circlearrowleft;", char.ConvertFromUtf32(0x021BA));
                namedCharacterReferences.Add("circlearrowright;", char.ConvertFromUtf32(0x021BB));
                namedCharacterReferences.Add("circledR;", char.ConvertFromUtf32(0x000AE));
                namedCharacterReferences.Add("circledS;", char.ConvertFromUtf32(0x024C8));
                namedCharacterReferences.Add("circledast;", char.ConvertFromUtf32(0x0229B));
                namedCharacterReferences.Add("circledcirc;", char.ConvertFromUtf32(0x0229A));
                namedCharacterReferences.Add("circleddash;", char.ConvertFromUtf32(0x0229D));
                namedCharacterReferences.Add("cire;", char.ConvertFromUtf32(0x02257));
                namedCharacterReferences.Add("cirfnint;", char.ConvertFromUtf32(0x02A10));
                namedCharacterReferences.Add("cirmid;", char.ConvertFromUtf32(0x02AEF));
                namedCharacterReferences.Add("cirscir;", char.ConvertFromUtf32(0x029C2));
                namedCharacterReferences.Add("clubs;", char.ConvertFromUtf32(0x02663));
                namedCharacterReferences.Add("clubsuit;", char.ConvertFromUtf32(0x02663));
                namedCharacterReferences.Add("colon;", char.ConvertFromUtf32(0x0003A));
                namedCharacterReferences.Add("colone;", char.ConvertFromUtf32(0x02254));
                namedCharacterReferences.Add("coloneq;", char.ConvertFromUtf32(0x02254));
                namedCharacterReferences.Add("comma;", char.ConvertFromUtf32(0x0002C));
                namedCharacterReferences.Add("commat;", char.ConvertFromUtf32(0x00040));
                namedCharacterReferences.Add("comp;", char.ConvertFromUtf32(0x02201));
                namedCharacterReferences.Add("compfn;", char.ConvertFromUtf32(0x02218));
                namedCharacterReferences.Add("complement;", char.ConvertFromUtf32(0x02201));
                namedCharacterReferences.Add("complexes;", char.ConvertFromUtf32(0x02102));
                namedCharacterReferences.Add("cong;", char.ConvertFromUtf32(0x02245));
                namedCharacterReferences.Add("congdot;", char.ConvertFromUtf32(0x02A6D));
                namedCharacterReferences.Add("conint;", char.ConvertFromUtf32(0x0222E));
                namedCharacterReferences.Add("copf;", char.ConvertFromUtf32(0x1D554));
                namedCharacterReferences.Add("coprod;", char.ConvertFromUtf32(0x02210));
                namedCharacterReferences.Add("copy;", char.ConvertFromUtf32(0x000A9));
                namedCharacterReferences.Add("copy", char.ConvertFromUtf32(0x000A9));
                namedCharacterReferences.Add("copysr;", char.ConvertFromUtf32(0x02117));
                namedCharacterReferences.Add("crarr;", char.ConvertFromUtf32(0x021B5));
                namedCharacterReferences.Add("cross;", char.ConvertFromUtf32(0x02717));
                namedCharacterReferences.Add("cscr;", char.ConvertFromUtf32(0x1D4B8));
                namedCharacterReferences.Add("csub;", char.ConvertFromUtf32(0x02ACF));
                namedCharacterReferences.Add("csube;", char.ConvertFromUtf32(0x02AD1));
                namedCharacterReferences.Add("csup;", char.ConvertFromUtf32(0x02AD0));
                namedCharacterReferences.Add("csupe;", char.ConvertFromUtf32(0x02AD2));
                namedCharacterReferences.Add("ctdot;", char.ConvertFromUtf32(0x022EF));
                namedCharacterReferences.Add("cudarrl;", char.ConvertFromUtf32(0x02938));
                namedCharacterReferences.Add("cudarrr;", char.ConvertFromUtf32(0x02935));
                namedCharacterReferences.Add("cuepr;", char.ConvertFromUtf32(0x022DE));
                namedCharacterReferences.Add("cuesc;", char.ConvertFromUtf32(0x022DF));
                namedCharacterReferences.Add("cularr;", char.ConvertFromUtf32(0x021B6));
                namedCharacterReferences.Add("cularrp;", char.ConvertFromUtf32(0x0293D));
                namedCharacterReferences.Add("cup;", char.ConvertFromUtf32(0x0222A));
                namedCharacterReferences.Add("cupbrcap;", char.ConvertFromUtf32(0x02A48));
                namedCharacterReferences.Add("cupcap;", char.ConvertFromUtf32(0x02A46));
                namedCharacterReferences.Add("cupcup;", char.ConvertFromUtf32(0x02A4A));
                namedCharacterReferences.Add("cupdot;", char.ConvertFromUtf32(0x0228D));
                namedCharacterReferences.Add("cupor;", char.ConvertFromUtf32(0x02A45));
                namedCharacterReferences.Add("curarr;", char.ConvertFromUtf32(0x021B7));
                namedCharacterReferences.Add("curarrm;", char.ConvertFromUtf32(0x0293C));
                namedCharacterReferences.Add("curlyeqprec;", char.ConvertFromUtf32(0x022DE));
                namedCharacterReferences.Add("curlyeqsucc;", char.ConvertFromUtf32(0x022DF));
                namedCharacterReferences.Add("curlyvee;", char.ConvertFromUtf32(0x022CE));
                namedCharacterReferences.Add("curlywedge;", char.ConvertFromUtf32(0x022CF));
                namedCharacterReferences.Add("curren;", char.ConvertFromUtf32(0x000A4));
                namedCharacterReferences.Add("curren", char.ConvertFromUtf32(0x000A4));
                namedCharacterReferences.Add("curvearrowleft;", char.ConvertFromUtf32(0x021B6));
                namedCharacterReferences.Add("curvearrowright;", char.ConvertFromUtf32(0x021B7));
                namedCharacterReferences.Add("cuvee;", char.ConvertFromUtf32(0x022CE));
                namedCharacterReferences.Add("cuwed;", char.ConvertFromUtf32(0x022CF));
                namedCharacterReferences.Add("cwconint;", char.ConvertFromUtf32(0x02232));
                namedCharacterReferences.Add("cwint;", char.ConvertFromUtf32(0x02231));
                namedCharacterReferences.Add("cylcty;", char.ConvertFromUtf32(0x0232D));
                namedCharacterReferences.Add("dArr;", char.ConvertFromUtf32(0x021D3));
                namedCharacterReferences.Add("dHar;", char.ConvertFromUtf32(0x02965));
                namedCharacterReferences.Add("dagger;", char.ConvertFromUtf32(0x02020));
                namedCharacterReferences.Add("daleth;", char.ConvertFromUtf32(0x02138));
                namedCharacterReferences.Add("darr;", char.ConvertFromUtf32(0x02193));
                namedCharacterReferences.Add("dash;", char.ConvertFromUtf32(0x02010));
                namedCharacterReferences.Add("dashv;", char.ConvertFromUtf32(0x022A3));
                namedCharacterReferences.Add("dbkarow;", char.ConvertFromUtf32(0x0290F));
                namedCharacterReferences.Add("dblac;", char.ConvertFromUtf32(0x002DD));
                namedCharacterReferences.Add("dcaron;", char.ConvertFromUtf32(0x0010F));
                namedCharacterReferences.Add("dcy;", char.ConvertFromUtf32(0x00434));
                namedCharacterReferences.Add("dd;", char.ConvertFromUtf32(0x02146));
                namedCharacterReferences.Add("ddagger;", char.ConvertFromUtf32(0x02021));
                namedCharacterReferences.Add("ddarr;", char.ConvertFromUtf32(0x021CA));
                namedCharacterReferences.Add("ddotseq;", char.ConvertFromUtf32(0x02A77));
                namedCharacterReferences.Add("deg;", char.ConvertFromUtf32(0x000B0));
                namedCharacterReferences.Add("deg", char.ConvertFromUtf32(0x000B0));
                namedCharacterReferences.Add("delta;", char.ConvertFromUtf32(0x003B4));
                namedCharacterReferences.Add("demptyv;", char.ConvertFromUtf32(0x029B1));
                namedCharacterReferences.Add("dfisht;", char.ConvertFromUtf32(0x0297F));
                namedCharacterReferences.Add("dfr;", char.ConvertFromUtf32(0x1D521));
                namedCharacterReferences.Add("dharl;", char.ConvertFromUtf32(0x021C3));
                namedCharacterReferences.Add("dharr;", char.ConvertFromUtf32(0x021C2));
                namedCharacterReferences.Add("diam;", char.ConvertFromUtf32(0x022C4));
                namedCharacterReferences.Add("diamond;", char.ConvertFromUtf32(0x022C4));
                namedCharacterReferences.Add("diamondsuit;", char.ConvertFromUtf32(0x02666));
                namedCharacterReferences.Add("diams;", char.ConvertFromUtf32(0x02666));
                namedCharacterReferences.Add("die;", char.ConvertFromUtf32(0x000A8));
                namedCharacterReferences.Add("digamma;", char.ConvertFromUtf32(0x003DD));
                namedCharacterReferences.Add("disin;", char.ConvertFromUtf32(0x022F2));
                namedCharacterReferences.Add("div;", char.ConvertFromUtf32(0x000F7));
                namedCharacterReferences.Add("divide;", char.ConvertFromUtf32(0x000F7));
                namedCharacterReferences.Add("divide", char.ConvertFromUtf32(0x000F7));
                namedCharacterReferences.Add("divideontimes;", char.ConvertFromUtf32(0x022C7));
                namedCharacterReferences.Add("divonx;", char.ConvertFromUtf32(0x022C7));
                namedCharacterReferences.Add("djcy;", char.ConvertFromUtf32(0x00452));
                namedCharacterReferences.Add("dlcorn;", char.ConvertFromUtf32(0x0231E));
                namedCharacterReferences.Add("dlcrop;", char.ConvertFromUtf32(0x0230D));
                namedCharacterReferences.Add("dollar;", char.ConvertFromUtf32(0x00024));
                namedCharacterReferences.Add("dopf;", char.ConvertFromUtf32(0x1D555));
                namedCharacterReferences.Add("dot;", char.ConvertFromUtf32(0x002D9));
                namedCharacterReferences.Add("doteq;", char.ConvertFromUtf32(0x02250));
                namedCharacterReferences.Add("doteqdot;", char.ConvertFromUtf32(0x02251));
                namedCharacterReferences.Add("dotminus;", char.ConvertFromUtf32(0x02238));
                namedCharacterReferences.Add("dotplus;", char.ConvertFromUtf32(0x02214));
                namedCharacterReferences.Add("dotsquare;", char.ConvertFromUtf32(0x022A1));
                namedCharacterReferences.Add("doublebarwedge;", char.ConvertFromUtf32(0x02306));
                namedCharacterReferences.Add("downarrow;", char.ConvertFromUtf32(0x02193));
                namedCharacterReferences.Add("downdownarrows;", char.ConvertFromUtf32(0x021CA));
                namedCharacterReferences.Add("downharpoonleft;", char.ConvertFromUtf32(0x021C3));
                namedCharacterReferences.Add("downharpoonright;", char.ConvertFromUtf32(0x021C2));
                namedCharacterReferences.Add("drbkarow;", char.ConvertFromUtf32(0x02910));
                namedCharacterReferences.Add("drcorn;", char.ConvertFromUtf32(0x0231F));
                namedCharacterReferences.Add("drcrop;", char.ConvertFromUtf32(0x0230C));
                namedCharacterReferences.Add("dscr;", char.ConvertFromUtf32(0x1D4B9));
                namedCharacterReferences.Add("dscy;", char.ConvertFromUtf32(0x00455));
                namedCharacterReferences.Add("dsol;", char.ConvertFromUtf32(0x029F6));
                namedCharacterReferences.Add("dstrok;", char.ConvertFromUtf32(0x00111));
                namedCharacterReferences.Add("dtdot;", char.ConvertFromUtf32(0x022F1));
                namedCharacterReferences.Add("dtri;", char.ConvertFromUtf32(0x025BF));
                namedCharacterReferences.Add("dtrif;", char.ConvertFromUtf32(0x025BE));
                namedCharacterReferences.Add("duarr;", char.ConvertFromUtf32(0x021F5));
                namedCharacterReferences.Add("duhar;", char.ConvertFromUtf32(0x0296F));
                namedCharacterReferences.Add("dwangle;", char.ConvertFromUtf32(0x029A6));
                namedCharacterReferences.Add("dzcy;", char.ConvertFromUtf32(0x0045F));
                namedCharacterReferences.Add("dzigrarr;", char.ConvertFromUtf32(0x027FF));
                namedCharacterReferences.Add("eDDot;", char.ConvertFromUtf32(0x02A77));
                namedCharacterReferences.Add("eDot;", char.ConvertFromUtf32(0x02251));
                namedCharacterReferences.Add("eacute;", char.ConvertFromUtf32(0x000E9));
                namedCharacterReferences.Add("eacute", char.ConvertFromUtf32(0x000E9));
                namedCharacterReferences.Add("easter;", char.ConvertFromUtf32(0x02A6E));
                namedCharacterReferences.Add("ecaron;", char.ConvertFromUtf32(0x0011B));
                namedCharacterReferences.Add("ecir;", char.ConvertFromUtf32(0x02256));
                namedCharacterReferences.Add("ecirc;", char.ConvertFromUtf32(0x000EA));
                namedCharacterReferences.Add("ecirc", char.ConvertFromUtf32(0x000EA));
                namedCharacterReferences.Add("ecolon;", char.ConvertFromUtf32(0x02255));
                namedCharacterReferences.Add("ecy;", char.ConvertFromUtf32(0x0044D));
                namedCharacterReferences.Add("edot;", char.ConvertFromUtf32(0x00117));
                namedCharacterReferences.Add("ee;", char.ConvertFromUtf32(0x02147));
                namedCharacterReferences.Add("efDot;", char.ConvertFromUtf32(0x02252));
                namedCharacterReferences.Add("efr;", char.ConvertFromUtf32(0x1D522));
                namedCharacterReferences.Add("eg;", char.ConvertFromUtf32(0x02A9A));
                namedCharacterReferences.Add("egrave;", char.ConvertFromUtf32(0x000E8));
                namedCharacterReferences.Add("egrave", char.ConvertFromUtf32(0x000E8));
                namedCharacterReferences.Add("egs;", char.ConvertFromUtf32(0x02A96));
                namedCharacterReferences.Add("egsdot;", char.ConvertFromUtf32(0x02A98));
                namedCharacterReferences.Add("el;", char.ConvertFromUtf32(0x02A99));
                namedCharacterReferences.Add("elinters;", char.ConvertFromUtf32(0x023E7));
                namedCharacterReferences.Add("ell;", char.ConvertFromUtf32(0x02113));
                namedCharacterReferences.Add("els;", char.ConvertFromUtf32(0x02A95));
                namedCharacterReferences.Add("elsdot;", char.ConvertFromUtf32(0x02A97));
                namedCharacterReferences.Add("emacr;", char.ConvertFromUtf32(0x00113));
                namedCharacterReferences.Add("empty;", char.ConvertFromUtf32(0x02205));
                namedCharacterReferences.Add("emptyset;", char.ConvertFromUtf32(0x02205));
                namedCharacterReferences.Add("emptyv;", char.ConvertFromUtf32(0x02205));
                namedCharacterReferences.Add("emsp13;", char.ConvertFromUtf32(0x02004));
                namedCharacterReferences.Add("emsp14;", char.ConvertFromUtf32(0x02005));
                namedCharacterReferences.Add("emsp;", char.ConvertFromUtf32(0x02003));
                namedCharacterReferences.Add("eng;", char.ConvertFromUtf32(0x0014B));
                namedCharacterReferences.Add("ensp;", char.ConvertFromUtf32(0x02002));
                namedCharacterReferences.Add("eogon;", char.ConvertFromUtf32(0x00119));
                namedCharacterReferences.Add("eopf;", char.ConvertFromUtf32(0x1D556));
                namedCharacterReferences.Add("epar;", char.ConvertFromUtf32(0x022D5));
                namedCharacterReferences.Add("eparsl;", char.ConvertFromUtf32(0x029E3));
                namedCharacterReferences.Add("eplus;", char.ConvertFromUtf32(0x02A71));
                namedCharacterReferences.Add("epsi;", char.ConvertFromUtf32(0x003B5));
                namedCharacterReferences.Add("epsilon;", char.ConvertFromUtf32(0x003B5));
                namedCharacterReferences.Add("epsiv;", char.ConvertFromUtf32(0x003F5));
                namedCharacterReferences.Add("eqcirc;", char.ConvertFromUtf32(0x02256));
                namedCharacterReferences.Add("eqcolon;", char.ConvertFromUtf32(0x02255));
                namedCharacterReferences.Add("eqsim;", char.ConvertFromUtf32(0x02242));
                namedCharacterReferences.Add("eqslantgtr;", char.ConvertFromUtf32(0x02A96));
                namedCharacterReferences.Add("eqslantless;", char.ConvertFromUtf32(0x02A95));
                namedCharacterReferences.Add("equals;", char.ConvertFromUtf32(0x0003D));
                namedCharacterReferences.Add("equest;", char.ConvertFromUtf32(0x0225F));
                namedCharacterReferences.Add("equiv;", char.ConvertFromUtf32(0x02261));
                namedCharacterReferences.Add("equivDD;", char.ConvertFromUtf32(0x02A78));
                namedCharacterReferences.Add("eqvparsl;", char.ConvertFromUtf32(0x029E5));
                namedCharacterReferences.Add("erDot;", char.ConvertFromUtf32(0x02253));
                namedCharacterReferences.Add("erarr;", char.ConvertFromUtf32(0x02971));
                namedCharacterReferences.Add("escr;", char.ConvertFromUtf32(0x0212F));
                namedCharacterReferences.Add("esdot;", char.ConvertFromUtf32(0x02250));
                namedCharacterReferences.Add("esim;", char.ConvertFromUtf32(0x02242));
                namedCharacterReferences.Add("eta;", char.ConvertFromUtf32(0x003B7));
                namedCharacterReferences.Add("eth;", char.ConvertFromUtf32(0x000F0));
                namedCharacterReferences.Add("eth", char.ConvertFromUtf32(0x000F0));
                namedCharacterReferences.Add("euml;", char.ConvertFromUtf32(0x000EB));
                namedCharacterReferences.Add("euml", char.ConvertFromUtf32(0x000EB));
                namedCharacterReferences.Add("euro;", char.ConvertFromUtf32(0x020AC));
                namedCharacterReferences.Add("excl;", char.ConvertFromUtf32(0x00021));
                namedCharacterReferences.Add("exist;", char.ConvertFromUtf32(0x02203));
                namedCharacterReferences.Add("expectation;", char.ConvertFromUtf32(0x02130));
                namedCharacterReferences.Add("exponentiale;", char.ConvertFromUtf32(0x02147));
                namedCharacterReferences.Add("fallingdotseq;", char.ConvertFromUtf32(0x02252));
                namedCharacterReferences.Add("fcy;", char.ConvertFromUtf32(0x00444));
                namedCharacterReferences.Add("female;", char.ConvertFromUtf32(0x02640));
                namedCharacterReferences.Add("ffilig;", char.ConvertFromUtf32(0x0FB03));
                namedCharacterReferences.Add("fflig;", char.ConvertFromUtf32(0x0FB00));
                namedCharacterReferences.Add("ffllig;", char.ConvertFromUtf32(0x0FB04));
                namedCharacterReferences.Add("ffr;", char.ConvertFromUtf32(0x1D523));
                namedCharacterReferences.Add("filig;", char.ConvertFromUtf32(0x0FB01));
                namedCharacterReferences.Add("flat;", char.ConvertFromUtf32(0x0266D));
                namedCharacterReferences.Add("fllig;", char.ConvertFromUtf32(0x0FB02));
                namedCharacterReferences.Add("fltns;", char.ConvertFromUtf32(0x025B1));
                namedCharacterReferences.Add("fnof;", char.ConvertFromUtf32(0x00192));
                namedCharacterReferences.Add("fopf;", char.ConvertFromUtf32(0x1D557));
                namedCharacterReferences.Add("forall;", char.ConvertFromUtf32(0x02200));
                namedCharacterReferences.Add("fork;", char.ConvertFromUtf32(0x022D4));
                namedCharacterReferences.Add("forkv;", char.ConvertFromUtf32(0x02AD9));
                namedCharacterReferences.Add("fpartint;", char.ConvertFromUtf32(0x02A0D));
                namedCharacterReferences.Add("frac12;", char.ConvertFromUtf32(0x000BD));
                namedCharacterReferences.Add("frac12", char.ConvertFromUtf32(0x000BD));
                namedCharacterReferences.Add("frac13;", char.ConvertFromUtf32(0x02153));
                namedCharacterReferences.Add("frac14;", char.ConvertFromUtf32(0x000BC));
                namedCharacterReferences.Add("frac14", char.ConvertFromUtf32(0x000BC));
                namedCharacterReferences.Add("frac15;", char.ConvertFromUtf32(0x02155));
                namedCharacterReferences.Add("frac16;", char.ConvertFromUtf32(0x02159));
                namedCharacterReferences.Add("frac18;", char.ConvertFromUtf32(0x0215B));
                namedCharacterReferences.Add("frac23;", char.ConvertFromUtf32(0x02154));
                namedCharacterReferences.Add("frac25;", char.ConvertFromUtf32(0x02156));
                namedCharacterReferences.Add("frac34;", char.ConvertFromUtf32(0x000BE));
                namedCharacterReferences.Add("frac34", char.ConvertFromUtf32(0x000BE));
                namedCharacterReferences.Add("frac35;", char.ConvertFromUtf32(0x02157));
                namedCharacterReferences.Add("frac38;", char.ConvertFromUtf32(0x0215C));
                namedCharacterReferences.Add("frac45;", char.ConvertFromUtf32(0x02158));
                namedCharacterReferences.Add("frac56;", char.ConvertFromUtf32(0x0215A));
                namedCharacterReferences.Add("frac58;", char.ConvertFromUtf32(0x0215D));
                namedCharacterReferences.Add("frac78;", char.ConvertFromUtf32(0x0215E));
                namedCharacterReferences.Add("frasl;", char.ConvertFromUtf32(0x02044));
                namedCharacterReferences.Add("frown;", char.ConvertFromUtf32(0x02322));
                namedCharacterReferences.Add("fscr;", char.ConvertFromUtf32(0x1D4BB));
                namedCharacterReferences.Add("gE;", char.ConvertFromUtf32(0x02267));
                namedCharacterReferences.Add("gEl;", char.ConvertFromUtf32(0x02A8C));
                namedCharacterReferences.Add("gacute;", char.ConvertFromUtf32(0x001F5));
                namedCharacterReferences.Add("gamma;", char.ConvertFromUtf32(0x003B3));
                namedCharacterReferences.Add("gammad;", char.ConvertFromUtf32(0x003DD));
                namedCharacterReferences.Add("gap;", char.ConvertFromUtf32(0x02A86));
                namedCharacterReferences.Add("gbreve;", char.ConvertFromUtf32(0x0011F));
                namedCharacterReferences.Add("gcirc;", char.ConvertFromUtf32(0x0011D));
                namedCharacterReferences.Add("gcy;", char.ConvertFromUtf32(0x00433));
                namedCharacterReferences.Add("gdot;", char.ConvertFromUtf32(0x00121));
                namedCharacterReferences.Add("ge;", char.ConvertFromUtf32(0x02265));
                namedCharacterReferences.Add("gel;", char.ConvertFromUtf32(0x022DB));
                namedCharacterReferences.Add("geq;", char.ConvertFromUtf32(0x02265));
                namedCharacterReferences.Add("geqq;", char.ConvertFromUtf32(0x02267));
                namedCharacterReferences.Add("geqslant;", char.ConvertFromUtf32(0x02A7E));
                namedCharacterReferences.Add("ges;", char.ConvertFromUtf32(0x02A7E));
                namedCharacterReferences.Add("gescc;", char.ConvertFromUtf32(0x02AA9));
                namedCharacterReferences.Add("gesdot;", char.ConvertFromUtf32(0x02A80));
                namedCharacterReferences.Add("gesdoto;", char.ConvertFromUtf32(0x02A82));
                namedCharacterReferences.Add("gesdotol;", char.ConvertFromUtf32(0x02A84));
                namedCharacterReferences.Add("gesles;", char.ConvertFromUtf32(0x02A94));
                namedCharacterReferences.Add("gfr;", char.ConvertFromUtf32(0x1D524));
                namedCharacterReferences.Add("gg;", char.ConvertFromUtf32(0x0226B));
                namedCharacterReferences.Add("ggg;", char.ConvertFromUtf32(0x022D9));
                namedCharacterReferences.Add("gimel;", char.ConvertFromUtf32(0x02137));
                namedCharacterReferences.Add("gjcy;", char.ConvertFromUtf32(0x00453));
                namedCharacterReferences.Add("gl;", char.ConvertFromUtf32(0x02277));
                namedCharacterReferences.Add("glE;", char.ConvertFromUtf32(0x02A92));
                namedCharacterReferences.Add("gla;", char.ConvertFromUtf32(0x02AA5));
                namedCharacterReferences.Add("glj;", char.ConvertFromUtf32(0x02AA4));
                namedCharacterReferences.Add("gnE;", char.ConvertFromUtf32(0x02269));
                namedCharacterReferences.Add("gnap;", char.ConvertFromUtf32(0x02A8A));
                namedCharacterReferences.Add("gnapprox;", char.ConvertFromUtf32(0x02A8A));
                namedCharacterReferences.Add("gne;", char.ConvertFromUtf32(0x02A88));
                namedCharacterReferences.Add("gneq;", char.ConvertFromUtf32(0x02A88));
                namedCharacterReferences.Add("gneqq;", char.ConvertFromUtf32(0x02269));
                namedCharacterReferences.Add("gnsim;", char.ConvertFromUtf32(0x022E7));
                namedCharacterReferences.Add("gopf;", char.ConvertFromUtf32(0x1D558));
                namedCharacterReferences.Add("grave;", char.ConvertFromUtf32(0x00060));
                namedCharacterReferences.Add("gscr;", char.ConvertFromUtf32(0x0210A));
                namedCharacterReferences.Add("gsim;", char.ConvertFromUtf32(0x02273));
                namedCharacterReferences.Add("gsime;", char.ConvertFromUtf32(0x02A8E));
                namedCharacterReferences.Add("gsiml;", char.ConvertFromUtf32(0x02A90));
                namedCharacterReferences.Add("gt;", char.ConvertFromUtf32(0x0003E));
                namedCharacterReferences.Add("gt", char.ConvertFromUtf32(0x0003E));
                namedCharacterReferences.Add("gtcc;", char.ConvertFromUtf32(0x02AA7));
                namedCharacterReferences.Add("gtcir;", char.ConvertFromUtf32(0x02A7A));
                namedCharacterReferences.Add("gtdot;", char.ConvertFromUtf32(0x022D7));
                namedCharacterReferences.Add("gtlPar;", char.ConvertFromUtf32(0x02995));
                namedCharacterReferences.Add("gtquest;", char.ConvertFromUtf32(0x02A7C));
                namedCharacterReferences.Add("gtrapprox;", char.ConvertFromUtf32(0x02A86));
                namedCharacterReferences.Add("gtrarr;", char.ConvertFromUtf32(0x02978));
                namedCharacterReferences.Add("gtrdot;", char.ConvertFromUtf32(0x022D7));
                namedCharacterReferences.Add("gtreqless;", char.ConvertFromUtf32(0x022DB));
                namedCharacterReferences.Add("gtreqqless;", char.ConvertFromUtf32(0x02A8C));
                namedCharacterReferences.Add("gtrless;", char.ConvertFromUtf32(0x02277));
                namedCharacterReferences.Add("gtrsim;", char.ConvertFromUtf32(0x02273));
                namedCharacterReferences.Add("hArr;", char.ConvertFromUtf32(0x021D4));
                namedCharacterReferences.Add("hairsp;", char.ConvertFromUtf32(0x0200A));
                namedCharacterReferences.Add("half;", char.ConvertFromUtf32(0x000BD));
                namedCharacterReferences.Add("hamilt;", char.ConvertFromUtf32(0x0210B));
                namedCharacterReferences.Add("hardcy;", char.ConvertFromUtf32(0x0044A));
                namedCharacterReferences.Add("harr;", char.ConvertFromUtf32(0x02194));
                namedCharacterReferences.Add("harrcir;", char.ConvertFromUtf32(0x02948));
                namedCharacterReferences.Add("harrw;", char.ConvertFromUtf32(0x021AD));
                namedCharacterReferences.Add("hbar;", char.ConvertFromUtf32(0x0210F));
                namedCharacterReferences.Add("hcirc;", char.ConvertFromUtf32(0x00125));
                namedCharacterReferences.Add("hearts;", char.ConvertFromUtf32(0x02665));
                namedCharacterReferences.Add("heartsuit;", char.ConvertFromUtf32(0x02665));
                namedCharacterReferences.Add("hellip;", char.ConvertFromUtf32(0x02026));
                namedCharacterReferences.Add("hercon;", char.ConvertFromUtf32(0x022B9));
                namedCharacterReferences.Add("hfr;", char.ConvertFromUtf32(0x1D525));
                namedCharacterReferences.Add("hksearow;", char.ConvertFromUtf32(0x02925));
                namedCharacterReferences.Add("hkswarow;", char.ConvertFromUtf32(0x02926));
                namedCharacterReferences.Add("hoarr;", char.ConvertFromUtf32(0x021FF));
                namedCharacterReferences.Add("homtht;", char.ConvertFromUtf32(0x0223B));
                namedCharacterReferences.Add("hookleftarrow;", char.ConvertFromUtf32(0x021A9));
                namedCharacterReferences.Add("hookrightarrow;", char.ConvertFromUtf32(0x021AA));
                namedCharacterReferences.Add("hopf;", char.ConvertFromUtf32(0x1D559));
                namedCharacterReferences.Add("horbar;", char.ConvertFromUtf32(0x02015));
                namedCharacterReferences.Add("hscr;", char.ConvertFromUtf32(0x1D4BD));
                namedCharacterReferences.Add("hslash;", char.ConvertFromUtf32(0x0210F));
                namedCharacterReferences.Add("hstrok;", char.ConvertFromUtf32(0x00127));
                namedCharacterReferences.Add("hybull;", char.ConvertFromUtf32(0x02043));
                namedCharacterReferences.Add("hyphen;", char.ConvertFromUtf32(0x02010));
                namedCharacterReferences.Add("iacute;", char.ConvertFromUtf32(0x000ED));
                namedCharacterReferences.Add("iacute", char.ConvertFromUtf32(0x000ED));
                namedCharacterReferences.Add("ic;", char.ConvertFromUtf32(0x02063));
                namedCharacterReferences.Add("icirc;", char.ConvertFromUtf32(0x000EE));
                namedCharacterReferences.Add("icirc", char.ConvertFromUtf32(0x000EE));
                namedCharacterReferences.Add("icy;", char.ConvertFromUtf32(0x00438));
                namedCharacterReferences.Add("iecy;", char.ConvertFromUtf32(0x00435));
                namedCharacterReferences.Add("iexcl;", char.ConvertFromUtf32(0x000A1));
                namedCharacterReferences.Add("iexcl", char.ConvertFromUtf32(0x000A1));
                namedCharacterReferences.Add("iff;", char.ConvertFromUtf32(0x021D4));
                namedCharacterReferences.Add("ifr;", char.ConvertFromUtf32(0x1D526));
                namedCharacterReferences.Add("igrave;", char.ConvertFromUtf32(0x000EC));
                namedCharacterReferences.Add("igrave", char.ConvertFromUtf32(0x000EC));
                namedCharacterReferences.Add("ii;", char.ConvertFromUtf32(0x02148));
                namedCharacterReferences.Add("iiiint;", char.ConvertFromUtf32(0x02A0C));
                namedCharacterReferences.Add("iiint;", char.ConvertFromUtf32(0x0222D));
                namedCharacterReferences.Add("iinfin;", char.ConvertFromUtf32(0x029DC));
                namedCharacterReferences.Add("iiota;", char.ConvertFromUtf32(0x02129));
                namedCharacterReferences.Add("ijlig;", char.ConvertFromUtf32(0x00133));
                namedCharacterReferences.Add("imacr;", char.ConvertFromUtf32(0x0012B));
                namedCharacterReferences.Add("image;", char.ConvertFromUtf32(0x02111));
                namedCharacterReferences.Add("imagline;", char.ConvertFromUtf32(0x02110));
                namedCharacterReferences.Add("imagpart;", char.ConvertFromUtf32(0x02111));
                namedCharacterReferences.Add("imath;", char.ConvertFromUtf32(0x00131));
                namedCharacterReferences.Add("imof;", char.ConvertFromUtf32(0x022B7));
                namedCharacterReferences.Add("imped;", char.ConvertFromUtf32(0x001B5));
                namedCharacterReferences.Add("in;", char.ConvertFromUtf32(0x02208));
                namedCharacterReferences.Add("incare;", char.ConvertFromUtf32(0x02105));
                namedCharacterReferences.Add("infin;", char.ConvertFromUtf32(0x0221E));
                namedCharacterReferences.Add("infintie;", char.ConvertFromUtf32(0x029DD));
                namedCharacterReferences.Add("inodot;", char.ConvertFromUtf32(0x00131));
                namedCharacterReferences.Add("int;", char.ConvertFromUtf32(0x0222B));
                namedCharacterReferences.Add("intcal;", char.ConvertFromUtf32(0x022BA));
                namedCharacterReferences.Add("integers;", char.ConvertFromUtf32(0x02124));
                namedCharacterReferences.Add("intercal;", char.ConvertFromUtf32(0x022BA));
                namedCharacterReferences.Add("intlarhk;", char.ConvertFromUtf32(0x02A17));
                namedCharacterReferences.Add("intprod;", char.ConvertFromUtf32(0x02A3C));
                namedCharacterReferences.Add("iocy;", char.ConvertFromUtf32(0x00451));
                namedCharacterReferences.Add("iogon;", char.ConvertFromUtf32(0x0012F));
                namedCharacterReferences.Add("iopf;", char.ConvertFromUtf32(0x1D55A));
                namedCharacterReferences.Add("iota;", char.ConvertFromUtf32(0x003B9));
                namedCharacterReferences.Add("iprod;", char.ConvertFromUtf32(0x02A3C));
                namedCharacterReferences.Add("iquest;", char.ConvertFromUtf32(0x000BF));
                namedCharacterReferences.Add("iquest", char.ConvertFromUtf32(0x000BF));
                namedCharacterReferences.Add("iscr;", char.ConvertFromUtf32(0x1D4BE));
                namedCharacterReferences.Add("isin;", char.ConvertFromUtf32(0x02208));
                namedCharacterReferences.Add("isinE;", char.ConvertFromUtf32(0x022F9));
                namedCharacterReferences.Add("isindot;", char.ConvertFromUtf32(0x022F5));
                namedCharacterReferences.Add("isins;", char.ConvertFromUtf32(0x022F4));
                namedCharacterReferences.Add("isinsv;", char.ConvertFromUtf32(0x022F3));
                namedCharacterReferences.Add("isinv;", char.ConvertFromUtf32(0x02208));
                namedCharacterReferences.Add("it;", char.ConvertFromUtf32(0x02062));
                namedCharacterReferences.Add("itilde;", char.ConvertFromUtf32(0x00129));
                namedCharacterReferences.Add("iukcy;", char.ConvertFromUtf32(0x00456));
                namedCharacterReferences.Add("iuml;", char.ConvertFromUtf32(0x000EF));
                namedCharacterReferences.Add("iuml", char.ConvertFromUtf32(0x000EF));
                namedCharacterReferences.Add("jcirc;", char.ConvertFromUtf32(0x00135));
                namedCharacterReferences.Add("jcy;", char.ConvertFromUtf32(0x00439));
                namedCharacterReferences.Add("jfr;", char.ConvertFromUtf32(0x1D527));
                namedCharacterReferences.Add("jmath;", char.ConvertFromUtf32(0x00237));
                namedCharacterReferences.Add("jopf;", char.ConvertFromUtf32(0x1D55B));
                namedCharacterReferences.Add("jscr;", char.ConvertFromUtf32(0x1D4BF));
                namedCharacterReferences.Add("jsercy;", char.ConvertFromUtf32(0x00458));
                namedCharacterReferences.Add("jukcy;", char.ConvertFromUtf32(0x00454));
                namedCharacterReferences.Add("kappa;", char.ConvertFromUtf32(0x003BA));
                namedCharacterReferences.Add("kappav;", char.ConvertFromUtf32(0x003F0));
                namedCharacterReferences.Add("kcedil;", char.ConvertFromUtf32(0x00137));
                namedCharacterReferences.Add("kcy;", char.ConvertFromUtf32(0x0043A));
                namedCharacterReferences.Add("kfr;", char.ConvertFromUtf32(0x1D528));
                namedCharacterReferences.Add("kgreen;", char.ConvertFromUtf32(0x00138));
                namedCharacterReferences.Add("khcy;", char.ConvertFromUtf32(0x00445));
                namedCharacterReferences.Add("kjcy;", char.ConvertFromUtf32(0x0045C));
                namedCharacterReferences.Add("kopf;", char.ConvertFromUtf32(0x1D55C));
                namedCharacterReferences.Add("kscr;", char.ConvertFromUtf32(0x1D4C0));
                namedCharacterReferences.Add("lAarr;", char.ConvertFromUtf32(0x021DA));
                namedCharacterReferences.Add("lArr;", char.ConvertFromUtf32(0x021D0));
                namedCharacterReferences.Add("lAtail;", char.ConvertFromUtf32(0x0291B));
                namedCharacterReferences.Add("lBarr;", char.ConvertFromUtf32(0x0290E));
                namedCharacterReferences.Add("lE;", char.ConvertFromUtf32(0x02266));
                namedCharacterReferences.Add("lEg;", char.ConvertFromUtf32(0x02A8B));
                namedCharacterReferences.Add("lHar;", char.ConvertFromUtf32(0x02962));
                namedCharacterReferences.Add("lacute;", char.ConvertFromUtf32(0x0013A));
                namedCharacterReferences.Add("laemptyv;", char.ConvertFromUtf32(0x029B4));
                namedCharacterReferences.Add("lagran;", char.ConvertFromUtf32(0x02112));
                namedCharacterReferences.Add("lambda;", char.ConvertFromUtf32(0x003BB));
                namedCharacterReferences.Add("lang;", char.ConvertFromUtf32(0x027E8));
                namedCharacterReferences.Add("langd;", char.ConvertFromUtf32(0x02991));
                namedCharacterReferences.Add("langle;", char.ConvertFromUtf32(0x027E8));
                namedCharacterReferences.Add("lap;", char.ConvertFromUtf32(0x02A85));
                namedCharacterReferences.Add("laquo;", char.ConvertFromUtf32(0x000AB));
                namedCharacterReferences.Add("laquo", char.ConvertFromUtf32(0x000AB));
                namedCharacterReferences.Add("larr;", char.ConvertFromUtf32(0x02190));
                namedCharacterReferences.Add("larrb;", char.ConvertFromUtf32(0x021E4));
                namedCharacterReferences.Add("larrbfs;", char.ConvertFromUtf32(0x0291F));
                namedCharacterReferences.Add("larrfs;", char.ConvertFromUtf32(0x0291D));
                namedCharacterReferences.Add("larrhk;", char.ConvertFromUtf32(0x021A9));
                namedCharacterReferences.Add("larrlp;", char.ConvertFromUtf32(0x021AB));
                namedCharacterReferences.Add("larrpl;", char.ConvertFromUtf32(0x02939));
                namedCharacterReferences.Add("larrsim;", char.ConvertFromUtf32(0x02973));
                namedCharacterReferences.Add("larrtl;", char.ConvertFromUtf32(0x021A2));
                namedCharacterReferences.Add("lat;", char.ConvertFromUtf32(0x02AAB));
                namedCharacterReferences.Add("latail;", char.ConvertFromUtf32(0x02919));
                namedCharacterReferences.Add("late;", char.ConvertFromUtf32(0x02AAD));
                namedCharacterReferences.Add("lbarr;", char.ConvertFromUtf32(0x0290C));
                namedCharacterReferences.Add("lbbrk;", char.ConvertFromUtf32(0x02772));
                namedCharacterReferences.Add("lbrace;", char.ConvertFromUtf32(0x0007B));
                namedCharacterReferences.Add("lbrack;", char.ConvertFromUtf32(0x0005B));
                namedCharacterReferences.Add("lbrke;", char.ConvertFromUtf32(0x0298B));
                namedCharacterReferences.Add("lbrksld;", char.ConvertFromUtf32(0x0298F));
                namedCharacterReferences.Add("lbrkslu;", char.ConvertFromUtf32(0x0298D));
                namedCharacterReferences.Add("lcaron;", char.ConvertFromUtf32(0x0013E));
                namedCharacterReferences.Add("lcedil;", char.ConvertFromUtf32(0x0013C));
                namedCharacterReferences.Add("lceil;", char.ConvertFromUtf32(0x02308));
                namedCharacterReferences.Add("lcub;", char.ConvertFromUtf32(0x0007B));
                namedCharacterReferences.Add("lcy;", char.ConvertFromUtf32(0x0043B));
                namedCharacterReferences.Add("ldca;", char.ConvertFromUtf32(0x02936));
                namedCharacterReferences.Add("ldquo;", char.ConvertFromUtf32(0x0201C));
                namedCharacterReferences.Add("ldquor;", char.ConvertFromUtf32(0x0201E));
                namedCharacterReferences.Add("ldrdhar;", char.ConvertFromUtf32(0x02967));
                namedCharacterReferences.Add("ldrushar;", char.ConvertFromUtf32(0x0294B));
                namedCharacterReferences.Add("ldsh;", char.ConvertFromUtf32(0x021B2));
                namedCharacterReferences.Add("le;", char.ConvertFromUtf32(0x02264));
                namedCharacterReferences.Add("leftarrow;", char.ConvertFromUtf32(0x02190));
                namedCharacterReferences.Add("leftarrowtail;", char.ConvertFromUtf32(0x021A2));
                namedCharacterReferences.Add("leftharpoondown;", char.ConvertFromUtf32(0x021BD));
                namedCharacterReferences.Add("leftharpoonup;", char.ConvertFromUtf32(0x021BC));
                namedCharacterReferences.Add("leftleftarrows;", char.ConvertFromUtf32(0x021C7));
                namedCharacterReferences.Add("leftrightarrow;", char.ConvertFromUtf32(0x02194));
                namedCharacterReferences.Add("leftrightarrows;", char.ConvertFromUtf32(0x021C6));
                namedCharacterReferences.Add("leftrightharpoons;", char.ConvertFromUtf32(0x021CB));
                namedCharacterReferences.Add("leftrightsquigarrow;", char.ConvertFromUtf32(0x021AD));
                namedCharacterReferences.Add("leftthreetimes;", char.ConvertFromUtf32(0x022CB));
                namedCharacterReferences.Add("leg;", char.ConvertFromUtf32(0x022DA));
                namedCharacterReferences.Add("leq;", char.ConvertFromUtf32(0x02264));
                namedCharacterReferences.Add("leqq;", char.ConvertFromUtf32(0x02266));
                namedCharacterReferences.Add("leqslant;", char.ConvertFromUtf32(0x02A7D));
                namedCharacterReferences.Add("les;", char.ConvertFromUtf32(0x02A7D));
                namedCharacterReferences.Add("lescc;", char.ConvertFromUtf32(0x02AA8));
                namedCharacterReferences.Add("lesdot;", char.ConvertFromUtf32(0x02A7F));
                namedCharacterReferences.Add("lesdoto;", char.ConvertFromUtf32(0x02A81));
                namedCharacterReferences.Add("lesdotor;", char.ConvertFromUtf32(0x02A83));
                namedCharacterReferences.Add("lesges;", char.ConvertFromUtf32(0x02A93));
                namedCharacterReferences.Add("lessapprox;", char.ConvertFromUtf32(0x02A85));
                namedCharacterReferences.Add("lessdot;", char.ConvertFromUtf32(0x022D6));
                namedCharacterReferences.Add("lesseqgtr;", char.ConvertFromUtf32(0x022DA));
                namedCharacterReferences.Add("lesseqqgtr;", char.ConvertFromUtf32(0x02A8B));
                namedCharacterReferences.Add("lessgtr;", char.ConvertFromUtf32(0x02276));
                namedCharacterReferences.Add("lesssim;", char.ConvertFromUtf32(0x02272));
                namedCharacterReferences.Add("lfisht;", char.ConvertFromUtf32(0x0297C));
                namedCharacterReferences.Add("lfloor;", char.ConvertFromUtf32(0x0230A));
                namedCharacterReferences.Add("lfr;", char.ConvertFromUtf32(0x1D529));
                namedCharacterReferences.Add("lg;", char.ConvertFromUtf32(0x02276));
                namedCharacterReferences.Add("lgE;", char.ConvertFromUtf32(0x02A91));
                namedCharacterReferences.Add("lhard;", char.ConvertFromUtf32(0x021BD));
                namedCharacterReferences.Add("lharu;", char.ConvertFromUtf32(0x021BC));
                namedCharacterReferences.Add("lharul;", char.ConvertFromUtf32(0x0296A));
                namedCharacterReferences.Add("lhblk;", char.ConvertFromUtf32(0x02584));
                namedCharacterReferences.Add("ljcy;", char.ConvertFromUtf32(0x00459));
                namedCharacterReferences.Add("ll;", char.ConvertFromUtf32(0x0226A));
                namedCharacterReferences.Add("llarr;", char.ConvertFromUtf32(0x021C7));
                namedCharacterReferences.Add("llcorner;", char.ConvertFromUtf32(0x0231E));
                namedCharacterReferences.Add("llhard;", char.ConvertFromUtf32(0x0296B));
                namedCharacterReferences.Add("lltri;", char.ConvertFromUtf32(0x025FA));
                namedCharacterReferences.Add("lmidot;", char.ConvertFromUtf32(0x00140));
                namedCharacterReferences.Add("lmoust;", char.ConvertFromUtf32(0x023B0));
                namedCharacterReferences.Add("lmoustache;", char.ConvertFromUtf32(0x023B0));
                namedCharacterReferences.Add("lnE;", char.ConvertFromUtf32(0x02268));
                namedCharacterReferences.Add("lnap;", char.ConvertFromUtf32(0x02A89));
                namedCharacterReferences.Add("lnapprox;", char.ConvertFromUtf32(0x02A89));
                namedCharacterReferences.Add("lne;", char.ConvertFromUtf32(0x02A87));
                namedCharacterReferences.Add("lneq;", char.ConvertFromUtf32(0x02A87));
                namedCharacterReferences.Add("lneqq;", char.ConvertFromUtf32(0x02268));
                namedCharacterReferences.Add("lnsim;", char.ConvertFromUtf32(0x022E6));
                namedCharacterReferences.Add("loang;", char.ConvertFromUtf32(0x027EC));
                namedCharacterReferences.Add("loarr;", char.ConvertFromUtf32(0x021FD));
                namedCharacterReferences.Add("lobrk;", char.ConvertFromUtf32(0x027E6));
                namedCharacterReferences.Add("longleftarrow;", char.ConvertFromUtf32(0x027F5));
                namedCharacterReferences.Add("longleftrightarrow;", char.ConvertFromUtf32(0x027F7));
                namedCharacterReferences.Add("longmapsto;", char.ConvertFromUtf32(0x027FC));
                namedCharacterReferences.Add("longrightarrow;", char.ConvertFromUtf32(0x027F6));
                namedCharacterReferences.Add("looparrowleft;", char.ConvertFromUtf32(0x021AB));
                namedCharacterReferences.Add("looparrowright;", char.ConvertFromUtf32(0x021AC));
                namedCharacterReferences.Add("lopar;", char.ConvertFromUtf32(0x02985));
                namedCharacterReferences.Add("lopf;", char.ConvertFromUtf32(0x1D55D));
                namedCharacterReferences.Add("loplus;", char.ConvertFromUtf32(0x02A2D));
                namedCharacterReferences.Add("lotimes;", char.ConvertFromUtf32(0x02A34));
                namedCharacterReferences.Add("lowast;", char.ConvertFromUtf32(0x02217));
                namedCharacterReferences.Add("lowbar;", char.ConvertFromUtf32(0x0005F));
                namedCharacterReferences.Add("loz;", char.ConvertFromUtf32(0x025CA));
                namedCharacterReferences.Add("lozenge;", char.ConvertFromUtf32(0x025CA));
                namedCharacterReferences.Add("lozf;", char.ConvertFromUtf32(0x029EB));
                namedCharacterReferences.Add("lpar;", char.ConvertFromUtf32(0x00028));
                namedCharacterReferences.Add("lparlt;", char.ConvertFromUtf32(0x02993));
                namedCharacterReferences.Add("lrarr;", char.ConvertFromUtf32(0x021C6));
                namedCharacterReferences.Add("lrcorner;", char.ConvertFromUtf32(0x0231F));
                namedCharacterReferences.Add("lrhar;", char.ConvertFromUtf32(0x021CB));
                namedCharacterReferences.Add("lrhard;", char.ConvertFromUtf32(0x0296D));
                namedCharacterReferences.Add("lrm;", char.ConvertFromUtf32(0x0200E));
                namedCharacterReferences.Add("lrtri;", char.ConvertFromUtf32(0x022BF));
                namedCharacterReferences.Add("lsaquo;", char.ConvertFromUtf32(0x02039));
                namedCharacterReferences.Add("lscr;", char.ConvertFromUtf32(0x1D4C1));
                namedCharacterReferences.Add("lsh;", char.ConvertFromUtf32(0x021B0));
                namedCharacterReferences.Add("lsim;", char.ConvertFromUtf32(0x02272));
                namedCharacterReferences.Add("lsime;", char.ConvertFromUtf32(0x02A8D));
                namedCharacterReferences.Add("lsimg;", char.ConvertFromUtf32(0x02A8F));
                namedCharacterReferences.Add("lsqb;", char.ConvertFromUtf32(0x0005B));
                namedCharacterReferences.Add("lsquo;", char.ConvertFromUtf32(0x02018));
                namedCharacterReferences.Add("lsquor;", char.ConvertFromUtf32(0x0201A));
                namedCharacterReferences.Add("lstrok;", char.ConvertFromUtf32(0x00142));
                namedCharacterReferences.Add("lt;", char.ConvertFromUtf32(0x0003C));
                namedCharacterReferences.Add("lt", char.ConvertFromUtf32(0x0003C));
                namedCharacterReferences.Add("ltcc;", char.ConvertFromUtf32(0x02AA6));
                namedCharacterReferences.Add("ltcir;", char.ConvertFromUtf32(0x02A79));
                namedCharacterReferences.Add("ltdot;", char.ConvertFromUtf32(0x022D6));
                namedCharacterReferences.Add("lthree;", char.ConvertFromUtf32(0x022CB));
                namedCharacterReferences.Add("ltimes;", char.ConvertFromUtf32(0x022C9));
                namedCharacterReferences.Add("ltlarr;", char.ConvertFromUtf32(0x02976));
                namedCharacterReferences.Add("ltquest;", char.ConvertFromUtf32(0x02A7B));
                namedCharacterReferences.Add("ltrPar;", char.ConvertFromUtf32(0x02996));
                namedCharacterReferences.Add("ltri;", char.ConvertFromUtf32(0x025C3));
                namedCharacterReferences.Add("ltrie;", char.ConvertFromUtf32(0x022B4));
                namedCharacterReferences.Add("ltrif;", char.ConvertFromUtf32(0x025C2));
                namedCharacterReferences.Add("lurdshar;", char.ConvertFromUtf32(0x0294A));
                namedCharacterReferences.Add("luruhar;", char.ConvertFromUtf32(0x02966));
                namedCharacterReferences.Add("mDDot;", char.ConvertFromUtf32(0x0223A));
                namedCharacterReferences.Add("macr;", char.ConvertFromUtf32(0x000AF));
                namedCharacterReferences.Add("macr", char.ConvertFromUtf32(0x000AF));
                namedCharacterReferences.Add("male;", char.ConvertFromUtf32(0x02642));
                namedCharacterReferences.Add("malt;", char.ConvertFromUtf32(0x02720));
                namedCharacterReferences.Add("maltese;", char.ConvertFromUtf32(0x02720));
                namedCharacterReferences.Add("map;", char.ConvertFromUtf32(0x021A6));
                namedCharacterReferences.Add("mapsto;", char.ConvertFromUtf32(0x021A6));
                namedCharacterReferences.Add("mapstodown;", char.ConvertFromUtf32(0x021A7));
                namedCharacterReferences.Add("mapstoleft;", char.ConvertFromUtf32(0x021A4));
                namedCharacterReferences.Add("mapstoup;", char.ConvertFromUtf32(0x021A5));
                namedCharacterReferences.Add("marker;", char.ConvertFromUtf32(0x025AE));
                namedCharacterReferences.Add("mcomma;", char.ConvertFromUtf32(0x02A29));
                namedCharacterReferences.Add("mcy;", char.ConvertFromUtf32(0x0043C));
                namedCharacterReferences.Add("mdash;", char.ConvertFromUtf32(0x02014));
                namedCharacterReferences.Add("measuredangle;", char.ConvertFromUtf32(0x02221));
                namedCharacterReferences.Add("mfr;", char.ConvertFromUtf32(0x1D52A));
                namedCharacterReferences.Add("mho;", char.ConvertFromUtf32(0x02127));
                namedCharacterReferences.Add("micro;", char.ConvertFromUtf32(0x000B5));
                namedCharacterReferences.Add("micro", char.ConvertFromUtf32(0x000B5));
                namedCharacterReferences.Add("mid;", char.ConvertFromUtf32(0x02223));
                namedCharacterReferences.Add("midast;", char.ConvertFromUtf32(0x0002A));
                namedCharacterReferences.Add("midcir;", char.ConvertFromUtf32(0x02AF0));
                namedCharacterReferences.Add("middot;", char.ConvertFromUtf32(0x000B7));
                namedCharacterReferences.Add("middot", char.ConvertFromUtf32(0x000B7));
                namedCharacterReferences.Add("minus;", char.ConvertFromUtf32(0x02212));
                namedCharacterReferences.Add("minusb;", char.ConvertFromUtf32(0x0229F));
                namedCharacterReferences.Add("minusd;", char.ConvertFromUtf32(0x02238));
                namedCharacterReferences.Add("minusdu;", char.ConvertFromUtf32(0x02A2A));
                namedCharacterReferences.Add("mlcp;", char.ConvertFromUtf32(0x02ADB));
                namedCharacterReferences.Add("mldr;", char.ConvertFromUtf32(0x02026));
                namedCharacterReferences.Add("mnplus;", char.ConvertFromUtf32(0x02213));
                namedCharacterReferences.Add("models;", char.ConvertFromUtf32(0x022A7));
                namedCharacterReferences.Add("mopf;", char.ConvertFromUtf32(0x1D55E));
                namedCharacterReferences.Add("mp;", char.ConvertFromUtf32(0x02213));
                namedCharacterReferences.Add("mscr;", char.ConvertFromUtf32(0x1D4C2));
                namedCharacterReferences.Add("mstpos;", char.ConvertFromUtf32(0x0223E));
                namedCharacterReferences.Add("mu;", char.ConvertFromUtf32(0x003BC));
                namedCharacterReferences.Add("multimap;", char.ConvertFromUtf32(0x022B8));
                namedCharacterReferences.Add("mumap;", char.ConvertFromUtf32(0x022B8));
                namedCharacterReferences.Add("nLeftarrow;", char.ConvertFromUtf32(0x021CD));
                namedCharacterReferences.Add("nLeftrightarrow;", char.ConvertFromUtf32(0x021CE));
                namedCharacterReferences.Add("nRightarrow;", char.ConvertFromUtf32(0x021CF));
                namedCharacterReferences.Add("nVDash;", char.ConvertFromUtf32(0x022AF));
                namedCharacterReferences.Add("nVdash;", char.ConvertFromUtf32(0x022AE));
                namedCharacterReferences.Add("nabla;", char.ConvertFromUtf32(0x02207));
                namedCharacterReferences.Add("nacute;", char.ConvertFromUtf32(0x00144));
                namedCharacterReferences.Add("nap;", char.ConvertFromUtf32(0x02249));
                namedCharacterReferences.Add("napos;", char.ConvertFromUtf32(0x00149));
                namedCharacterReferences.Add("napprox;", char.ConvertFromUtf32(0x02249));
                namedCharacterReferences.Add("natur;", char.ConvertFromUtf32(0x0266E));
                namedCharacterReferences.Add("natural;", char.ConvertFromUtf32(0x0266E));
                namedCharacterReferences.Add("naturals;", char.ConvertFromUtf32(0x02115));
                namedCharacterReferences.Add("nbsp;", char.ConvertFromUtf32(0x000A0));
                namedCharacterReferences.Add("nbsp", char.ConvertFromUtf32(0x000A0));
                namedCharacterReferences.Add("ncap;", char.ConvertFromUtf32(0x02A43));
                namedCharacterReferences.Add("ncaron;", char.ConvertFromUtf32(0x00148));
                namedCharacterReferences.Add("ncedil;", char.ConvertFromUtf32(0x00146));
                namedCharacterReferences.Add("ncong;", char.ConvertFromUtf32(0x02247));
                namedCharacterReferences.Add("ncup;", char.ConvertFromUtf32(0x02A42));
                namedCharacterReferences.Add("ncy;", char.ConvertFromUtf32(0x0043D));
                namedCharacterReferences.Add("ndash;", char.ConvertFromUtf32(0x02013));
                namedCharacterReferences.Add("ne;", char.ConvertFromUtf32(0x02260));
                namedCharacterReferences.Add("neArr;", char.ConvertFromUtf32(0x021D7));
                namedCharacterReferences.Add("nearhk;", char.ConvertFromUtf32(0x02924));
                namedCharacterReferences.Add("nearr;", char.ConvertFromUtf32(0x02197));
                namedCharacterReferences.Add("nearrow;", char.ConvertFromUtf32(0x02197));
                namedCharacterReferences.Add("nequiv;", char.ConvertFromUtf32(0x02262));
                namedCharacterReferences.Add("nesear;", char.ConvertFromUtf32(0x02928));
                namedCharacterReferences.Add("nexist;", char.ConvertFromUtf32(0x02204));
                namedCharacterReferences.Add("nexists;", char.ConvertFromUtf32(0x02204));
                namedCharacterReferences.Add("nfr;", char.ConvertFromUtf32(0x1D52B));
                namedCharacterReferences.Add("nge;", char.ConvertFromUtf32(0x02271));
                namedCharacterReferences.Add("ngeq;", char.ConvertFromUtf32(0x02271));
                namedCharacterReferences.Add("ngsim;", char.ConvertFromUtf32(0x02275));
                namedCharacterReferences.Add("ngt;", char.ConvertFromUtf32(0x0226F));
                namedCharacterReferences.Add("ngtr;", char.ConvertFromUtf32(0x0226F));
                namedCharacterReferences.Add("nhArr;", char.ConvertFromUtf32(0x021CE));
                namedCharacterReferences.Add("nharr;", char.ConvertFromUtf32(0x021AE));
                namedCharacterReferences.Add("nhpar;", char.ConvertFromUtf32(0x02AF2));
                namedCharacterReferences.Add("ni;", char.ConvertFromUtf32(0x0220B));
                namedCharacterReferences.Add("nis;", char.ConvertFromUtf32(0x022FC));
                namedCharacterReferences.Add("nisd;", char.ConvertFromUtf32(0x022FA));
                namedCharacterReferences.Add("niv;", char.ConvertFromUtf32(0x0220B));
                namedCharacterReferences.Add("njcy;", char.ConvertFromUtf32(0x0045A));
                namedCharacterReferences.Add("nlArr;", char.ConvertFromUtf32(0x021CD));
                namedCharacterReferences.Add("nlarr;", char.ConvertFromUtf32(0x0219A));
                namedCharacterReferences.Add("nldr;", char.ConvertFromUtf32(0x02025));
                namedCharacterReferences.Add("nle;", char.ConvertFromUtf32(0x02270));
                namedCharacterReferences.Add("nleftarrow;", char.ConvertFromUtf32(0x0219A));
                namedCharacterReferences.Add("nleftrightarrow;", char.ConvertFromUtf32(0x021AE));
                namedCharacterReferences.Add("nleq;", char.ConvertFromUtf32(0x02270));
                namedCharacterReferences.Add("nless;", char.ConvertFromUtf32(0x0226E));
                namedCharacterReferences.Add("nlsim;", char.ConvertFromUtf32(0x02274));
                namedCharacterReferences.Add("nlt;", char.ConvertFromUtf32(0x0226E));
                namedCharacterReferences.Add("nltri;", char.ConvertFromUtf32(0x022EA));
                namedCharacterReferences.Add("nltrie;", char.ConvertFromUtf32(0x022EC));
                namedCharacterReferences.Add("nmid;", char.ConvertFromUtf32(0x02224));
                namedCharacterReferences.Add("nopf;", char.ConvertFromUtf32(0x1D55F));
                namedCharacterReferences.Add("not;", char.ConvertFromUtf32(0x000AC));
                namedCharacterReferences.Add("not", char.ConvertFromUtf32(0x000AC));
                namedCharacterReferences.Add("notin;", char.ConvertFromUtf32(0x02209));
                namedCharacterReferences.Add("notinva;", char.ConvertFromUtf32(0x02209));
                namedCharacterReferences.Add("notinvb;", char.ConvertFromUtf32(0x022F7));
                namedCharacterReferences.Add("notinvc;", char.ConvertFromUtf32(0x022F6));
                namedCharacterReferences.Add("notni;", char.ConvertFromUtf32(0x0220C));
                namedCharacterReferences.Add("notniva;", char.ConvertFromUtf32(0x0220C));
                namedCharacterReferences.Add("notnivb;", char.ConvertFromUtf32(0x022FE));
                namedCharacterReferences.Add("notnivc;", char.ConvertFromUtf32(0x022FD));
                namedCharacterReferences.Add("npar;", char.ConvertFromUtf32(0x02226));
                namedCharacterReferences.Add("nparallel;", char.ConvertFromUtf32(0x02226));
                namedCharacterReferences.Add("npolint;", char.ConvertFromUtf32(0x02A14));
                namedCharacterReferences.Add("npr;", char.ConvertFromUtf32(0x02280));
                namedCharacterReferences.Add("nprcue;", char.ConvertFromUtf32(0x022E0));
                namedCharacterReferences.Add("nprec;", char.ConvertFromUtf32(0x02280));
                namedCharacterReferences.Add("nrArr;", char.ConvertFromUtf32(0x021CF));
                namedCharacterReferences.Add("nrarr;", char.ConvertFromUtf32(0x0219B));
                namedCharacterReferences.Add("nrightarrow;", char.ConvertFromUtf32(0x0219B));
                namedCharacterReferences.Add("nrtri;", char.ConvertFromUtf32(0x022EB));
                namedCharacterReferences.Add("nrtrie;", char.ConvertFromUtf32(0x022ED));
                namedCharacterReferences.Add("nsc;", char.ConvertFromUtf32(0x02281));
                namedCharacterReferences.Add("nsccue;", char.ConvertFromUtf32(0x022E1));
                namedCharacterReferences.Add("nscr;", char.ConvertFromUtf32(0x1D4C3));
                namedCharacterReferences.Add("nshortmid;", char.ConvertFromUtf32(0x02224));
                namedCharacterReferences.Add("nshortparallel;", char.ConvertFromUtf32(0x02226));
                namedCharacterReferences.Add("nsim;", char.ConvertFromUtf32(0x02241));
                namedCharacterReferences.Add("nsime;", char.ConvertFromUtf32(0x02244));
                namedCharacterReferences.Add("nsimeq;", char.ConvertFromUtf32(0x02244));
                namedCharacterReferences.Add("nsmid;", char.ConvertFromUtf32(0x02224));
                namedCharacterReferences.Add("nspar;", char.ConvertFromUtf32(0x02226));
                namedCharacterReferences.Add("nsqsube;", char.ConvertFromUtf32(0x022E2));
                namedCharacterReferences.Add("nsqsupe;", char.ConvertFromUtf32(0x022E3));
                namedCharacterReferences.Add("nsub;", char.ConvertFromUtf32(0x02284));
                namedCharacterReferences.Add("nsube;", char.ConvertFromUtf32(0x02288));
                namedCharacterReferences.Add("nsubseteq;", char.ConvertFromUtf32(0x02288));
                namedCharacterReferences.Add("nsucc;", char.ConvertFromUtf32(0x02281));
                namedCharacterReferences.Add("nsup;", char.ConvertFromUtf32(0x02285));
                namedCharacterReferences.Add("nsupe;", char.ConvertFromUtf32(0x02289));
                namedCharacterReferences.Add("nsupseteq;", char.ConvertFromUtf32(0x02289));
                namedCharacterReferences.Add("ntgl;", char.ConvertFromUtf32(0x02279));
                namedCharacterReferences.Add("ntilde;", char.ConvertFromUtf32(0x000F1));
                namedCharacterReferences.Add("ntilde", char.ConvertFromUtf32(0x000F1));
                namedCharacterReferences.Add("ntlg;", char.ConvertFromUtf32(0x02278));
                namedCharacterReferences.Add("ntriangleleft;", char.ConvertFromUtf32(0x022EA));
                namedCharacterReferences.Add("ntrianglelefteq;", char.ConvertFromUtf32(0x022EC));
                namedCharacterReferences.Add("ntriangleright;", char.ConvertFromUtf32(0x022EB));
                namedCharacterReferences.Add("ntrianglerighteq;", char.ConvertFromUtf32(0x022ED));
                namedCharacterReferences.Add("nu;", char.ConvertFromUtf32(0x003BD));
                namedCharacterReferences.Add("num;", char.ConvertFromUtf32(0x00023));
                namedCharacterReferences.Add("numero;", char.ConvertFromUtf32(0x02116));
                namedCharacterReferences.Add("numsp;", char.ConvertFromUtf32(0x02007));
                namedCharacterReferences.Add("nvDash;", char.ConvertFromUtf32(0x022AD));
                namedCharacterReferences.Add("nvHarr;", char.ConvertFromUtf32(0x02904));
                namedCharacterReferences.Add("nvdash;", char.ConvertFromUtf32(0x022AC));
                namedCharacterReferences.Add("nvinfin;", char.ConvertFromUtf32(0x029DE));
                namedCharacterReferences.Add("nvlArr;", char.ConvertFromUtf32(0x02902));
                namedCharacterReferences.Add("nvrArr;", char.ConvertFromUtf32(0x02903));
                namedCharacterReferences.Add("nwArr;", char.ConvertFromUtf32(0x021D6));
                namedCharacterReferences.Add("nwarhk;", char.ConvertFromUtf32(0x02923));
                namedCharacterReferences.Add("nwarr;", char.ConvertFromUtf32(0x02196));
                namedCharacterReferences.Add("nwarrow;", char.ConvertFromUtf32(0x02196));
                namedCharacterReferences.Add("nwnear;", char.ConvertFromUtf32(0x02927));
                namedCharacterReferences.Add("oS;", char.ConvertFromUtf32(0x024C8));
                namedCharacterReferences.Add("oacute;", char.ConvertFromUtf32(0x000F3));
                namedCharacterReferences.Add("oacute", char.ConvertFromUtf32(0x000F3));
                namedCharacterReferences.Add("oast;", char.ConvertFromUtf32(0x0229B));
                namedCharacterReferences.Add("ocir;", char.ConvertFromUtf32(0x0229A));
                namedCharacterReferences.Add("ocirc;", char.ConvertFromUtf32(0x000F4));
                namedCharacterReferences.Add("ocirc", char.ConvertFromUtf32(0x000F4));
                namedCharacterReferences.Add("ocy;", char.ConvertFromUtf32(0x0043E));
                namedCharacterReferences.Add("odash;", char.ConvertFromUtf32(0x0229D));
                namedCharacterReferences.Add("odblac;", char.ConvertFromUtf32(0x00151));
                namedCharacterReferences.Add("odiv;", char.ConvertFromUtf32(0x02A38));
                namedCharacterReferences.Add("odot;", char.ConvertFromUtf32(0x02299));
                namedCharacterReferences.Add("odsold;", char.ConvertFromUtf32(0x029BC));
                namedCharacterReferences.Add("oelig;", char.ConvertFromUtf32(0x00153));
                namedCharacterReferences.Add("ofcir;", char.ConvertFromUtf32(0x029BF));
                namedCharacterReferences.Add("ofr;", char.ConvertFromUtf32(0x1D52C));
                namedCharacterReferences.Add("ogon;", char.ConvertFromUtf32(0x002DB));
                namedCharacterReferences.Add("ograve;", char.ConvertFromUtf32(0x000F2));
                namedCharacterReferences.Add("ograve", char.ConvertFromUtf32(0x000F2));
                namedCharacterReferences.Add("ogt;", char.ConvertFromUtf32(0x029C1));
                namedCharacterReferences.Add("ohbar;", char.ConvertFromUtf32(0x029B5));
                namedCharacterReferences.Add("ohm;", char.ConvertFromUtf32(0x003A9));
                namedCharacterReferences.Add("oint;", char.ConvertFromUtf32(0x0222E));
                namedCharacterReferences.Add("olarr;", char.ConvertFromUtf32(0x021BA));
                namedCharacterReferences.Add("olcir;", char.ConvertFromUtf32(0x029BE));
                namedCharacterReferences.Add("olcross;", char.ConvertFromUtf32(0x029BB));
                namedCharacterReferences.Add("oline;", char.ConvertFromUtf32(0x0203E));
                namedCharacterReferences.Add("olt;", char.ConvertFromUtf32(0x029C0));
                namedCharacterReferences.Add("omacr;", char.ConvertFromUtf32(0x0014D));
                namedCharacterReferences.Add("omega;", char.ConvertFromUtf32(0x003C9));
                namedCharacterReferences.Add("omicron;", char.ConvertFromUtf32(0x003BF));
                namedCharacterReferences.Add("omid;", char.ConvertFromUtf32(0x029B6));
                namedCharacterReferences.Add("ominus;", char.ConvertFromUtf32(0x02296));
                namedCharacterReferences.Add("oopf;", char.ConvertFromUtf32(0x1D560));
                namedCharacterReferences.Add("opar;", char.ConvertFromUtf32(0x029B7));
                namedCharacterReferences.Add("operp;", char.ConvertFromUtf32(0x029B9));
                namedCharacterReferences.Add("oplus;", char.ConvertFromUtf32(0x02295));
                namedCharacterReferences.Add("or;", char.ConvertFromUtf32(0x02228));
                namedCharacterReferences.Add("orarr;", char.ConvertFromUtf32(0x021BB));
                namedCharacterReferences.Add("ord;", char.ConvertFromUtf32(0x02A5D));
                namedCharacterReferences.Add("order;", char.ConvertFromUtf32(0x02134));
                namedCharacterReferences.Add("orderof;", char.ConvertFromUtf32(0x02134));
                namedCharacterReferences.Add("ordf;", char.ConvertFromUtf32(0x000AA));
                namedCharacterReferences.Add("ordf", char.ConvertFromUtf32(0x000AA));
                namedCharacterReferences.Add("ordm;", char.ConvertFromUtf32(0x000BA));
                namedCharacterReferences.Add("ordm", char.ConvertFromUtf32(0x000BA));
                namedCharacterReferences.Add("origof;", char.ConvertFromUtf32(0x022B6));
                namedCharacterReferences.Add("oror;", char.ConvertFromUtf32(0x02A56));
                namedCharacterReferences.Add("orslope;", char.ConvertFromUtf32(0x02A57));
                namedCharacterReferences.Add("orv;", char.ConvertFromUtf32(0x02A5B));
                namedCharacterReferences.Add("oscr;", char.ConvertFromUtf32(0x02134));
                namedCharacterReferences.Add("oslash;", char.ConvertFromUtf32(0x000F8));
                namedCharacterReferences.Add("oslash", char.ConvertFromUtf32(0x000F8));
                namedCharacterReferences.Add("osol;", char.ConvertFromUtf32(0x02298));
                namedCharacterReferences.Add("otilde;", char.ConvertFromUtf32(0x000F5));
                namedCharacterReferences.Add("otilde", char.ConvertFromUtf32(0x000F5));
                namedCharacterReferences.Add("otimes;", char.ConvertFromUtf32(0x02297));
                namedCharacterReferences.Add("otimesas;", char.ConvertFromUtf32(0x02A36));
                namedCharacterReferences.Add("ouml;", char.ConvertFromUtf32(0x000F6));
                namedCharacterReferences.Add("ouml", char.ConvertFromUtf32(0x000F6));
                namedCharacterReferences.Add("ovbar;", char.ConvertFromUtf32(0x0233D));
                namedCharacterReferences.Add("par;", char.ConvertFromUtf32(0x02225));
                namedCharacterReferences.Add("para;", char.ConvertFromUtf32(0x000B6));
                namedCharacterReferences.Add("para", char.ConvertFromUtf32(0x000B6));
                namedCharacterReferences.Add("parallel;", char.ConvertFromUtf32(0x02225));
                namedCharacterReferences.Add("parsim;", char.ConvertFromUtf32(0x02AF3));
                namedCharacterReferences.Add("parsl;", char.ConvertFromUtf32(0x02AFD));
                namedCharacterReferences.Add("part;", char.ConvertFromUtf32(0x02202));
                namedCharacterReferences.Add("pcy;", char.ConvertFromUtf32(0x0043F));
                namedCharacterReferences.Add("percnt;", char.ConvertFromUtf32(0x00025));
                namedCharacterReferences.Add("period;", char.ConvertFromUtf32(0x0002E));
                namedCharacterReferences.Add("permil;", char.ConvertFromUtf32(0x02030));
                namedCharacterReferences.Add("perp;", char.ConvertFromUtf32(0x022A5));
                namedCharacterReferences.Add("pertenk;", char.ConvertFromUtf32(0x02031));
                namedCharacterReferences.Add("pfr;", char.ConvertFromUtf32(0x1D52D));
                namedCharacterReferences.Add("phi;", char.ConvertFromUtf32(0x003C6));
                namedCharacterReferences.Add("phiv;", char.ConvertFromUtf32(0x003D5));
                namedCharacterReferences.Add("phmmat;", char.ConvertFromUtf32(0x02133));
                namedCharacterReferences.Add("phone;", char.ConvertFromUtf32(0x0260E));
                namedCharacterReferences.Add("pi;", char.ConvertFromUtf32(0x003C0));
                namedCharacterReferences.Add("pitchfork;", char.ConvertFromUtf32(0x022D4));
                namedCharacterReferences.Add("piv;", char.ConvertFromUtf32(0x003D6));
                namedCharacterReferences.Add("planck;", char.ConvertFromUtf32(0x0210F));
                namedCharacterReferences.Add("planckh;", char.ConvertFromUtf32(0x0210E));
                namedCharacterReferences.Add("plankv;", char.ConvertFromUtf32(0x0210F));
                namedCharacterReferences.Add("plus;", char.ConvertFromUtf32(0x0002B));
                namedCharacterReferences.Add("plusacir;", char.ConvertFromUtf32(0x02A23));
                namedCharacterReferences.Add("plusb;", char.ConvertFromUtf32(0x0229E));
                namedCharacterReferences.Add("pluscir;", char.ConvertFromUtf32(0x02A22));
                namedCharacterReferences.Add("plusdo;", char.ConvertFromUtf32(0x02214));
                namedCharacterReferences.Add("plusdu;", char.ConvertFromUtf32(0x02A25));
                namedCharacterReferences.Add("pluse;", char.ConvertFromUtf32(0x02A72));
                namedCharacterReferences.Add("plusmn;", char.ConvertFromUtf32(0x000B1));
                namedCharacterReferences.Add("plusmn", char.ConvertFromUtf32(0x000B1));
                namedCharacterReferences.Add("plussim;", char.ConvertFromUtf32(0x02A26));
                namedCharacterReferences.Add("plustwo;", char.ConvertFromUtf32(0x02A27));
                namedCharacterReferences.Add("pm;", char.ConvertFromUtf32(0x000B1));
                namedCharacterReferences.Add("pointint;", char.ConvertFromUtf32(0x02A15));
                namedCharacterReferences.Add("popf;", char.ConvertFromUtf32(0x1D561));
                namedCharacterReferences.Add("pound;", char.ConvertFromUtf32(0x000A3));
                namedCharacterReferences.Add("pound", char.ConvertFromUtf32(0x000A3));
                namedCharacterReferences.Add("pr;", char.ConvertFromUtf32(0x0227A));
                namedCharacterReferences.Add("prE;", char.ConvertFromUtf32(0x02AB3));
                namedCharacterReferences.Add("prap;", char.ConvertFromUtf32(0x02AB7));
                namedCharacterReferences.Add("prcue;", char.ConvertFromUtf32(0x0227C));
                namedCharacterReferences.Add("pre;", char.ConvertFromUtf32(0x02AAF));
                namedCharacterReferences.Add("prec;", char.ConvertFromUtf32(0x0227A));
                namedCharacterReferences.Add("precapprox;", char.ConvertFromUtf32(0x02AB7));
                namedCharacterReferences.Add("preccurlyeq;", char.ConvertFromUtf32(0x0227C));
                namedCharacterReferences.Add("preceq;", char.ConvertFromUtf32(0x02AAF));
                namedCharacterReferences.Add("precnapprox;", char.ConvertFromUtf32(0x02AB9));
                namedCharacterReferences.Add("precneqq;", char.ConvertFromUtf32(0x02AB5));
                namedCharacterReferences.Add("precnsim;", char.ConvertFromUtf32(0x022E8));
                namedCharacterReferences.Add("precsim;", char.ConvertFromUtf32(0x0227E));
                namedCharacterReferences.Add("prime;", char.ConvertFromUtf32(0x02032));
                namedCharacterReferences.Add("primes;", char.ConvertFromUtf32(0x02119));
                namedCharacterReferences.Add("prnE;", char.ConvertFromUtf32(0x02AB5));
                namedCharacterReferences.Add("prnap;", char.ConvertFromUtf32(0x02AB9));
                namedCharacterReferences.Add("prnsim;", char.ConvertFromUtf32(0x022E8));
                namedCharacterReferences.Add("prod;", char.ConvertFromUtf32(0x0220F));
                namedCharacterReferences.Add("profalar;", char.ConvertFromUtf32(0x0232E));
                namedCharacterReferences.Add("profline;", char.ConvertFromUtf32(0x02312));
                namedCharacterReferences.Add("profsurf;", char.ConvertFromUtf32(0x02313));
                namedCharacterReferences.Add("prop;", char.ConvertFromUtf32(0x0221D));
                namedCharacterReferences.Add("propto;", char.ConvertFromUtf32(0x0221D));
                namedCharacterReferences.Add("prsim;", char.ConvertFromUtf32(0x0227E));
                namedCharacterReferences.Add("prurel;", char.ConvertFromUtf32(0x022B0));
                namedCharacterReferences.Add("pscr;", char.ConvertFromUtf32(0x1D4C5));
                namedCharacterReferences.Add("psi;", char.ConvertFromUtf32(0x003C8));
                namedCharacterReferences.Add("puncsp;", char.ConvertFromUtf32(0x02008));
                namedCharacterReferences.Add("qfr;", char.ConvertFromUtf32(0x1D52E));
                namedCharacterReferences.Add("qint;", char.ConvertFromUtf32(0x02A0C));
                namedCharacterReferences.Add("qopf;", char.ConvertFromUtf32(0x1D562));
                namedCharacterReferences.Add("qprime;", char.ConvertFromUtf32(0x02057));
                namedCharacterReferences.Add("qscr;", char.ConvertFromUtf32(0x1D4C6));
                namedCharacterReferences.Add("quaternions;", char.ConvertFromUtf32(0x0210D));
                namedCharacterReferences.Add("quatint;", char.ConvertFromUtf32(0x02A16));
                namedCharacterReferences.Add("quest;", char.ConvertFromUtf32(0x0003F));
                namedCharacterReferences.Add("questeq;", char.ConvertFromUtf32(0x0225F));
                namedCharacterReferences.Add("quot;", char.ConvertFromUtf32(0x00022));
                namedCharacterReferences.Add("quot", char.ConvertFromUtf32(0x00022));
                namedCharacterReferences.Add("rAarr;", char.ConvertFromUtf32(0x021DB));
                namedCharacterReferences.Add("rArr;", char.ConvertFromUtf32(0x021D2));
                namedCharacterReferences.Add("rAtail;", char.ConvertFromUtf32(0x0291C));
                namedCharacterReferences.Add("rBarr;", char.ConvertFromUtf32(0x0290F));
                namedCharacterReferences.Add("rHar;", char.ConvertFromUtf32(0x02964));
                namedCharacterReferences.Add("racute;", char.ConvertFromUtf32(0x00155));
                namedCharacterReferences.Add("radic;", char.ConvertFromUtf32(0x0221A));
                namedCharacterReferences.Add("raemptyv;", char.ConvertFromUtf32(0x029B3));
                namedCharacterReferences.Add("rang;", char.ConvertFromUtf32(0x027E9));
                namedCharacterReferences.Add("rangd;", char.ConvertFromUtf32(0x02992));
                namedCharacterReferences.Add("range;", char.ConvertFromUtf32(0x029A5));
                namedCharacterReferences.Add("rangle;", char.ConvertFromUtf32(0x027E9));
                namedCharacterReferences.Add("raquo;", char.ConvertFromUtf32(0x000BB));
                namedCharacterReferences.Add("raquo", char.ConvertFromUtf32(0x000BB));
                namedCharacterReferences.Add("rarr;", char.ConvertFromUtf32(0x02192));
                namedCharacterReferences.Add("rarrap;", char.ConvertFromUtf32(0x02975));
                namedCharacterReferences.Add("rarrb;", char.ConvertFromUtf32(0x021E5));
                namedCharacterReferences.Add("rarrbfs;", char.ConvertFromUtf32(0x02920));
                namedCharacterReferences.Add("rarrc;", char.ConvertFromUtf32(0x02933));
                namedCharacterReferences.Add("rarrfs;", char.ConvertFromUtf32(0x0291E));
                namedCharacterReferences.Add("rarrhk;", char.ConvertFromUtf32(0x021AA));
                namedCharacterReferences.Add("rarrlp;", char.ConvertFromUtf32(0x021AC));
                namedCharacterReferences.Add("rarrpl;", char.ConvertFromUtf32(0x02945));
                namedCharacterReferences.Add("rarrsim;", char.ConvertFromUtf32(0x02974));
                namedCharacterReferences.Add("rarrtl;", char.ConvertFromUtf32(0x021A3));
                namedCharacterReferences.Add("rarrw;", char.ConvertFromUtf32(0x0219D));
                namedCharacterReferences.Add("ratail;", char.ConvertFromUtf32(0x0291A));
                namedCharacterReferences.Add("ratio;", char.ConvertFromUtf32(0x02236));
                namedCharacterReferences.Add("rationals;", char.ConvertFromUtf32(0x0211A));
                namedCharacterReferences.Add("rbarr;", char.ConvertFromUtf32(0x0290D));
                namedCharacterReferences.Add("rbbrk;", char.ConvertFromUtf32(0x02773));
                namedCharacterReferences.Add("rbrace;", char.ConvertFromUtf32(0x0007D));
                namedCharacterReferences.Add("rbrack;", char.ConvertFromUtf32(0x0005D));
                namedCharacterReferences.Add("rbrke;", char.ConvertFromUtf32(0x0298C));
                namedCharacterReferences.Add("rbrksld;", char.ConvertFromUtf32(0x0298E));
                namedCharacterReferences.Add("rbrkslu;", char.ConvertFromUtf32(0x02990));
                namedCharacterReferences.Add("rcaron;", char.ConvertFromUtf32(0x00159));
                namedCharacterReferences.Add("rcedil;", char.ConvertFromUtf32(0x00157));
                namedCharacterReferences.Add("rceil;", char.ConvertFromUtf32(0x02309));
                namedCharacterReferences.Add("rcub;", char.ConvertFromUtf32(0x0007D));
                namedCharacterReferences.Add("rcy;", char.ConvertFromUtf32(0x00440));
                namedCharacterReferences.Add("rdca;", char.ConvertFromUtf32(0x02937));
                namedCharacterReferences.Add("rdldhar;", char.ConvertFromUtf32(0x02969));
                namedCharacterReferences.Add("rdquo;", char.ConvertFromUtf32(0x0201D));
                namedCharacterReferences.Add("rdquor;", char.ConvertFromUtf32(0x0201D));
                namedCharacterReferences.Add("rdsh;", char.ConvertFromUtf32(0x021B3));
                namedCharacterReferences.Add("real;", char.ConvertFromUtf32(0x0211C));
                namedCharacterReferences.Add("realine;", char.ConvertFromUtf32(0x0211B));
                namedCharacterReferences.Add("realpart;", char.ConvertFromUtf32(0x0211C));
                namedCharacterReferences.Add("reals;", char.ConvertFromUtf32(0x0211D));
                namedCharacterReferences.Add("rect;", char.ConvertFromUtf32(0x025AD));
                namedCharacterReferences.Add("reg;", char.ConvertFromUtf32(0x000AE));
                namedCharacterReferences.Add("reg", char.ConvertFromUtf32(0x000AE));
                namedCharacterReferences.Add("rfisht;", char.ConvertFromUtf32(0x0297D));
                namedCharacterReferences.Add("rfloor;", char.ConvertFromUtf32(0x0230B));
                namedCharacterReferences.Add("rfr;", char.ConvertFromUtf32(0x1D52F));
                namedCharacterReferences.Add("rhard;", char.ConvertFromUtf32(0x021C1));
                namedCharacterReferences.Add("rharu;", char.ConvertFromUtf32(0x021C0));
                namedCharacterReferences.Add("rharul;", char.ConvertFromUtf32(0x0296C));
                namedCharacterReferences.Add("rho;", char.ConvertFromUtf32(0x003C1));
                namedCharacterReferences.Add("rhov;", char.ConvertFromUtf32(0x003F1));
                namedCharacterReferences.Add("rightarrow;", char.ConvertFromUtf32(0x02192));
                namedCharacterReferences.Add("rightarrowtail;", char.ConvertFromUtf32(0x021A3));
                namedCharacterReferences.Add("rightharpoondown;", char.ConvertFromUtf32(0x021C1));
                namedCharacterReferences.Add("rightharpoonup;", char.ConvertFromUtf32(0x021C0));
                namedCharacterReferences.Add("rightleftarrows;", char.ConvertFromUtf32(0x021C4));
                namedCharacterReferences.Add("rightleftharpoons;", char.ConvertFromUtf32(0x021CC));
                namedCharacterReferences.Add("rightrightarrows;", char.ConvertFromUtf32(0x021C9));
                namedCharacterReferences.Add("rightsquigarrow;", char.ConvertFromUtf32(0x0219D));
                namedCharacterReferences.Add("rightthreetimes;", char.ConvertFromUtf32(0x022CC));
                namedCharacterReferences.Add("ring;", char.ConvertFromUtf32(0x002DA));
                namedCharacterReferences.Add("risingdotseq;", char.ConvertFromUtf32(0x02253));
                namedCharacterReferences.Add("rlarr;", char.ConvertFromUtf32(0x021C4));
                namedCharacterReferences.Add("rlhar;", char.ConvertFromUtf32(0x021CC));
                namedCharacterReferences.Add("rlm;", char.ConvertFromUtf32(0x0200F));
                namedCharacterReferences.Add("rmoust;", char.ConvertFromUtf32(0x023B1));
                namedCharacterReferences.Add("rmoustache;", char.ConvertFromUtf32(0x023B1));
                namedCharacterReferences.Add("rnmid;", char.ConvertFromUtf32(0x02AEE));
                namedCharacterReferences.Add("roang;", char.ConvertFromUtf32(0x027ED));
                namedCharacterReferences.Add("roarr;", char.ConvertFromUtf32(0x021FE));
                namedCharacterReferences.Add("robrk;", char.ConvertFromUtf32(0x027E7));
                namedCharacterReferences.Add("ropar;", char.ConvertFromUtf32(0x02986));
                namedCharacterReferences.Add("ropf;", char.ConvertFromUtf32(0x1D563));
                namedCharacterReferences.Add("roplus;", char.ConvertFromUtf32(0x02A2E));
                namedCharacterReferences.Add("rotimes;", char.ConvertFromUtf32(0x02A35));
                namedCharacterReferences.Add("rpar;", char.ConvertFromUtf32(0x00029));
                namedCharacterReferences.Add("rpargt;", char.ConvertFromUtf32(0x02994));
                namedCharacterReferences.Add("rppolint;", char.ConvertFromUtf32(0x02A12));
                namedCharacterReferences.Add("rrarr;", char.ConvertFromUtf32(0x021C9));
                namedCharacterReferences.Add("rsaquo;", char.ConvertFromUtf32(0x0203A));
                namedCharacterReferences.Add("rscr;", char.ConvertFromUtf32(0x1D4C7));
                namedCharacterReferences.Add("rsh;", char.ConvertFromUtf32(0x021B1));
                namedCharacterReferences.Add("rsqb;", char.ConvertFromUtf32(0x0005D));
                namedCharacterReferences.Add("rsquo;", char.ConvertFromUtf32(0x02019));
                namedCharacterReferences.Add("rsquor;", char.ConvertFromUtf32(0x02019));
                namedCharacterReferences.Add("rthree;", char.ConvertFromUtf32(0x022CC));
                namedCharacterReferences.Add("rtimes;", char.ConvertFromUtf32(0x022CA));
                namedCharacterReferences.Add("rtri;", char.ConvertFromUtf32(0x025B9));
                namedCharacterReferences.Add("rtrie;", char.ConvertFromUtf32(0x022B5));
                namedCharacterReferences.Add("rtrif;", char.ConvertFromUtf32(0x025B8));
                namedCharacterReferences.Add("rtriltri;", char.ConvertFromUtf32(0x029CE));
                namedCharacterReferences.Add("ruluhar;", char.ConvertFromUtf32(0x02968));
                namedCharacterReferences.Add("rx;", char.ConvertFromUtf32(0x0211E));
                namedCharacterReferences.Add("sacute;", char.ConvertFromUtf32(0x0015B));
                namedCharacterReferences.Add("sbquo;", char.ConvertFromUtf32(0x0201A));
                namedCharacterReferences.Add("sc;", char.ConvertFromUtf32(0x0227B));
                namedCharacterReferences.Add("scE;", char.ConvertFromUtf32(0x02AB4));
                namedCharacterReferences.Add("scap;", char.ConvertFromUtf32(0x02AB8));
                namedCharacterReferences.Add("scaron;", char.ConvertFromUtf32(0x00161));
                namedCharacterReferences.Add("sccue;", char.ConvertFromUtf32(0x0227D));
                namedCharacterReferences.Add("sce;", char.ConvertFromUtf32(0x02AB0));
                namedCharacterReferences.Add("scedil;", char.ConvertFromUtf32(0x0015F));
                namedCharacterReferences.Add("scirc;", char.ConvertFromUtf32(0x0015D));
                namedCharacterReferences.Add("scnE;", char.ConvertFromUtf32(0x02AB6));
                namedCharacterReferences.Add("scnap;", char.ConvertFromUtf32(0x02ABA));
                namedCharacterReferences.Add("scnsim;", char.ConvertFromUtf32(0x022E9));
                namedCharacterReferences.Add("scpolint;", char.ConvertFromUtf32(0x02A13));
                namedCharacterReferences.Add("scsim;", char.ConvertFromUtf32(0x0227F));
                namedCharacterReferences.Add("scy;", char.ConvertFromUtf32(0x00441));
                namedCharacterReferences.Add("sdot;", char.ConvertFromUtf32(0x022C5));
                namedCharacterReferences.Add("sdotb;", char.ConvertFromUtf32(0x022A1));
                namedCharacterReferences.Add("sdote;", char.ConvertFromUtf32(0x02A66));
                namedCharacterReferences.Add("seArr;", char.ConvertFromUtf32(0x021D8));
                namedCharacterReferences.Add("searhk;", char.ConvertFromUtf32(0x02925));
                namedCharacterReferences.Add("searr;", char.ConvertFromUtf32(0x02198));
                namedCharacterReferences.Add("searrow;", char.ConvertFromUtf32(0x02198));
                namedCharacterReferences.Add("sect;", char.ConvertFromUtf32(0x000A7));
                namedCharacterReferences.Add("sect", char.ConvertFromUtf32(0x000A7));
                namedCharacterReferences.Add("semi;", char.ConvertFromUtf32(0x0003B));
                namedCharacterReferences.Add("seswar;", char.ConvertFromUtf32(0x02929));
                namedCharacterReferences.Add("setminus;", char.ConvertFromUtf32(0x02216));
                namedCharacterReferences.Add("setmn;", char.ConvertFromUtf32(0x02216));
                namedCharacterReferences.Add("sext;", char.ConvertFromUtf32(0x02736));
                namedCharacterReferences.Add("sfr;", char.ConvertFromUtf32(0x1D530));
                namedCharacterReferences.Add("sfrown;", char.ConvertFromUtf32(0x02322));
                namedCharacterReferences.Add("sharp;", char.ConvertFromUtf32(0x0266F));
                namedCharacterReferences.Add("shchcy;", char.ConvertFromUtf32(0x00449));
                namedCharacterReferences.Add("shcy;", char.ConvertFromUtf32(0x00448));
                namedCharacterReferences.Add("shortmid;", char.ConvertFromUtf32(0x02223));
                namedCharacterReferences.Add("shortparallel;", char.ConvertFromUtf32(0x02225));
                namedCharacterReferences.Add("shy;", char.ConvertFromUtf32(0x000AD));
                namedCharacterReferences.Add("shy", char.ConvertFromUtf32(0x000AD));
                namedCharacterReferences.Add("sigma;", char.ConvertFromUtf32(0x003C3));
                namedCharacterReferences.Add("sigmaf;", char.ConvertFromUtf32(0x003C2));
                namedCharacterReferences.Add("sigmav;", char.ConvertFromUtf32(0x003C2));
                namedCharacterReferences.Add("sim;", char.ConvertFromUtf32(0x0223C));
                namedCharacterReferences.Add("simdot;", char.ConvertFromUtf32(0x02A6A));
                namedCharacterReferences.Add("sime;", char.ConvertFromUtf32(0x02243));
                namedCharacterReferences.Add("simeq;", char.ConvertFromUtf32(0x02243));
                namedCharacterReferences.Add("simg;", char.ConvertFromUtf32(0x02A9E));
                namedCharacterReferences.Add("simgE;", char.ConvertFromUtf32(0x02AA0));
                namedCharacterReferences.Add("siml;", char.ConvertFromUtf32(0x02A9D));
                namedCharacterReferences.Add("simlE;", char.ConvertFromUtf32(0x02A9F));
                namedCharacterReferences.Add("simne;", char.ConvertFromUtf32(0x02246));
                namedCharacterReferences.Add("simplus;", char.ConvertFromUtf32(0x02A24));
                namedCharacterReferences.Add("simrarr;", char.ConvertFromUtf32(0x02972));
                namedCharacterReferences.Add("slarr;", char.ConvertFromUtf32(0x02190));
                namedCharacterReferences.Add("smallsetminus;", char.ConvertFromUtf32(0x02216));
                namedCharacterReferences.Add("smashp;", char.ConvertFromUtf32(0x02A33));
                namedCharacterReferences.Add("smeparsl;", char.ConvertFromUtf32(0x029E4));
                namedCharacterReferences.Add("smid;", char.ConvertFromUtf32(0x02223));
                namedCharacterReferences.Add("smile;", char.ConvertFromUtf32(0x02323));
                namedCharacterReferences.Add("smt;", char.ConvertFromUtf32(0x02AAA));
                namedCharacterReferences.Add("smte;", char.ConvertFromUtf32(0x02AAC));
                namedCharacterReferences.Add("softcy;", char.ConvertFromUtf32(0x0044C));
                namedCharacterReferences.Add("sol;", char.ConvertFromUtf32(0x0002F));
                namedCharacterReferences.Add("solb;", char.ConvertFromUtf32(0x029C4));
                namedCharacterReferences.Add("solbar;", char.ConvertFromUtf32(0x0233F));
                namedCharacterReferences.Add("sopf;", char.ConvertFromUtf32(0x1D564));
                namedCharacterReferences.Add("spades;", char.ConvertFromUtf32(0x02660));
                namedCharacterReferences.Add("spadesuit;", char.ConvertFromUtf32(0x02660));
                namedCharacterReferences.Add("spar;", char.ConvertFromUtf32(0x02225));
                namedCharacterReferences.Add("sqcap;", char.ConvertFromUtf32(0x02293));
                namedCharacterReferences.Add("sqcup;", char.ConvertFromUtf32(0x02294));
                namedCharacterReferences.Add("sqsub;", char.ConvertFromUtf32(0x0228F));
                namedCharacterReferences.Add("sqsube;", char.ConvertFromUtf32(0x02291));
                namedCharacterReferences.Add("sqsubset;", char.ConvertFromUtf32(0x0228F));
                namedCharacterReferences.Add("sqsubseteq;", char.ConvertFromUtf32(0x02291));
                namedCharacterReferences.Add("sqsup;", char.ConvertFromUtf32(0x02290));
                namedCharacterReferences.Add("sqsupe;", char.ConvertFromUtf32(0x02292));
                namedCharacterReferences.Add("sqsupset;", char.ConvertFromUtf32(0x02290));
                namedCharacterReferences.Add("sqsupseteq;", char.ConvertFromUtf32(0x02292));
                namedCharacterReferences.Add("squ;", char.ConvertFromUtf32(0x025A1));
                namedCharacterReferences.Add("square;", char.ConvertFromUtf32(0x025A1));
                namedCharacterReferences.Add("squarf;", char.ConvertFromUtf32(0x025AA));
                namedCharacterReferences.Add("squf;", char.ConvertFromUtf32(0x025AA));
                namedCharacterReferences.Add("srarr;", char.ConvertFromUtf32(0x02192));
                namedCharacterReferences.Add("sscr;", char.ConvertFromUtf32(0x1D4C8));
                namedCharacterReferences.Add("ssetmn;", char.ConvertFromUtf32(0x02216));
                namedCharacterReferences.Add("ssmile;", char.ConvertFromUtf32(0x02323));
                namedCharacterReferences.Add("sstarf;", char.ConvertFromUtf32(0x022C6));
                namedCharacterReferences.Add("star;", char.ConvertFromUtf32(0x02606));
                namedCharacterReferences.Add("starf;", char.ConvertFromUtf32(0x02605));
                namedCharacterReferences.Add("straightepsilon;", char.ConvertFromUtf32(0x003F5));
                namedCharacterReferences.Add("straightphi;", char.ConvertFromUtf32(0x003D5));
                namedCharacterReferences.Add("strns;", char.ConvertFromUtf32(0x000AF));
                namedCharacterReferences.Add("sub;", char.ConvertFromUtf32(0x02282));
                namedCharacterReferences.Add("subE;", char.ConvertFromUtf32(0x02AC5));
                namedCharacterReferences.Add("subdot;", char.ConvertFromUtf32(0x02ABD));
                namedCharacterReferences.Add("sube;", char.ConvertFromUtf32(0x02286));
                namedCharacterReferences.Add("subedot;", char.ConvertFromUtf32(0x02AC3));
                namedCharacterReferences.Add("submult;", char.ConvertFromUtf32(0x02AC1));
                namedCharacterReferences.Add("subnE;", char.ConvertFromUtf32(0x02ACB));
                namedCharacterReferences.Add("subne;", char.ConvertFromUtf32(0x0228A));
                namedCharacterReferences.Add("subplus;", char.ConvertFromUtf32(0x02ABF));
                namedCharacterReferences.Add("subrarr;", char.ConvertFromUtf32(0x02979));
                namedCharacterReferences.Add("subset;", char.ConvertFromUtf32(0x02282));
                namedCharacterReferences.Add("subseteq;", char.ConvertFromUtf32(0x02286));
                namedCharacterReferences.Add("subseteqq;", char.ConvertFromUtf32(0x02AC5));
                namedCharacterReferences.Add("subsetneq;", char.ConvertFromUtf32(0x0228A));
                namedCharacterReferences.Add("subsetneqq;", char.ConvertFromUtf32(0x02ACB));
                namedCharacterReferences.Add("subsim;", char.ConvertFromUtf32(0x02AC7));
                namedCharacterReferences.Add("subsub;", char.ConvertFromUtf32(0x02AD5));
                namedCharacterReferences.Add("subsup;", char.ConvertFromUtf32(0x02AD3));
                namedCharacterReferences.Add("succ;", char.ConvertFromUtf32(0x0227B));
                namedCharacterReferences.Add("succapprox;", char.ConvertFromUtf32(0x02AB8));
                namedCharacterReferences.Add("succcurlyeq;", char.ConvertFromUtf32(0x0227D));
                namedCharacterReferences.Add("succeq;", char.ConvertFromUtf32(0x02AB0));
                namedCharacterReferences.Add("succnapprox;", char.ConvertFromUtf32(0x02ABA));
                namedCharacterReferences.Add("succneqq;", char.ConvertFromUtf32(0x02AB6));
                namedCharacterReferences.Add("succnsim;", char.ConvertFromUtf32(0x022E9));
                namedCharacterReferences.Add("succsim;", char.ConvertFromUtf32(0x0227F));
                namedCharacterReferences.Add("sum;", char.ConvertFromUtf32(0x02211));
                namedCharacterReferences.Add("sung;", char.ConvertFromUtf32(0x0266A));
                namedCharacterReferences.Add("sup1;", char.ConvertFromUtf32(0x000B9));
                namedCharacterReferences.Add("sup1", char.ConvertFromUtf32(0x000B9));
                namedCharacterReferences.Add("sup2;", char.ConvertFromUtf32(0x000B2));
                namedCharacterReferences.Add("sup2", char.ConvertFromUtf32(0x000B2));
                namedCharacterReferences.Add("sup3;", char.ConvertFromUtf32(0x000B3));
                namedCharacterReferences.Add("sup3", char.ConvertFromUtf32(0x000B3));
                namedCharacterReferences.Add("sup;", char.ConvertFromUtf32(0x02283));
                namedCharacterReferences.Add("supE;", char.ConvertFromUtf32(0x02AC6));
                namedCharacterReferences.Add("supdot;", char.ConvertFromUtf32(0x02ABE));
                namedCharacterReferences.Add("supdsub;", char.ConvertFromUtf32(0x02AD8));
                namedCharacterReferences.Add("supe;", char.ConvertFromUtf32(0x02287));
                namedCharacterReferences.Add("supedot;", char.ConvertFromUtf32(0x02AC4));
                namedCharacterReferences.Add("suphsol;", char.ConvertFromUtf32(0x027C9));
                namedCharacterReferences.Add("suphsub;", char.ConvertFromUtf32(0x02AD7));
                namedCharacterReferences.Add("suplarr;", char.ConvertFromUtf32(0x0297B));
                namedCharacterReferences.Add("supmult;", char.ConvertFromUtf32(0x02AC2));
                namedCharacterReferences.Add("supnE;", char.ConvertFromUtf32(0x02ACC));
                namedCharacterReferences.Add("supne;", char.ConvertFromUtf32(0x0228B));
                namedCharacterReferences.Add("supplus;", char.ConvertFromUtf32(0x02AC0));
                namedCharacterReferences.Add("supset;", char.ConvertFromUtf32(0x02283));
                namedCharacterReferences.Add("supseteq;", char.ConvertFromUtf32(0x02287));
                namedCharacterReferences.Add("supseteqq;", char.ConvertFromUtf32(0x02AC6));
                namedCharacterReferences.Add("supsetneq;", char.ConvertFromUtf32(0x0228B));
                namedCharacterReferences.Add("supsetneqq;", char.ConvertFromUtf32(0x02ACC));
                namedCharacterReferences.Add("supsim;", char.ConvertFromUtf32(0x02AC8));
                namedCharacterReferences.Add("supsub;", char.ConvertFromUtf32(0x02AD4));
                namedCharacterReferences.Add("supsup;", char.ConvertFromUtf32(0x02AD6));
                namedCharacterReferences.Add("swArr;", char.ConvertFromUtf32(0x021D9));
                namedCharacterReferences.Add("swarhk;", char.ConvertFromUtf32(0x02926));
                namedCharacterReferences.Add("swarr;", char.ConvertFromUtf32(0x02199));
                namedCharacterReferences.Add("swarrow;", char.ConvertFromUtf32(0x02199));
                namedCharacterReferences.Add("swnwar;", char.ConvertFromUtf32(0x0292A));
                namedCharacterReferences.Add("szlig;", char.ConvertFromUtf32(0x000DF));
                namedCharacterReferences.Add("szlig", char.ConvertFromUtf32(0x000DF));
                namedCharacterReferences.Add("target;", char.ConvertFromUtf32(0x02316));
                namedCharacterReferences.Add("tau;", char.ConvertFromUtf32(0x003C4));
                namedCharacterReferences.Add("tbrk;", char.ConvertFromUtf32(0x023B4));
                namedCharacterReferences.Add("tcaron;", char.ConvertFromUtf32(0x00165));
                namedCharacterReferences.Add("tcedil;", char.ConvertFromUtf32(0x00163));
                namedCharacterReferences.Add("tcy;", char.ConvertFromUtf32(0x00442));
                namedCharacterReferences.Add("tdot;", char.ConvertFromUtf32(0x020DB));
                namedCharacterReferences.Add("telrec;", char.ConvertFromUtf32(0x02315));
                namedCharacterReferences.Add("tfr;", char.ConvertFromUtf32(0x1D531));
                namedCharacterReferences.Add("there4;", char.ConvertFromUtf32(0x02234));
                namedCharacterReferences.Add("therefore;", char.ConvertFromUtf32(0x02234));
                namedCharacterReferences.Add("theta;", char.ConvertFromUtf32(0x003B8));
                namedCharacterReferences.Add("thetasym;", char.ConvertFromUtf32(0x003D1));
                namedCharacterReferences.Add("thetav;", char.ConvertFromUtf32(0x003D1));
                namedCharacterReferences.Add("thickapprox;", char.ConvertFromUtf32(0x02248));
                namedCharacterReferences.Add("thicksim;", char.ConvertFromUtf32(0x0223C));
                namedCharacterReferences.Add("thinsp;", char.ConvertFromUtf32(0x02009));
                namedCharacterReferences.Add("thkap;", char.ConvertFromUtf32(0x02248));
                namedCharacterReferences.Add("thksim;", char.ConvertFromUtf32(0x0223C));
                namedCharacterReferences.Add("thorn;", char.ConvertFromUtf32(0x000FE));
                namedCharacterReferences.Add("thorn", char.ConvertFromUtf32(0x000FE));
                namedCharacterReferences.Add("tilde;", char.ConvertFromUtf32(0x002DC));
                namedCharacterReferences.Add("times;", char.ConvertFromUtf32(0x000D7));
                namedCharacterReferences.Add("times", char.ConvertFromUtf32(0x000D7));
                namedCharacterReferences.Add("timesb;", char.ConvertFromUtf32(0x022A0));
                namedCharacterReferences.Add("timesbar;", char.ConvertFromUtf32(0x02A31));
                namedCharacterReferences.Add("timesd;", char.ConvertFromUtf32(0x02A30));
                namedCharacterReferences.Add("tint;", char.ConvertFromUtf32(0x0222D));
                namedCharacterReferences.Add("toea;", char.ConvertFromUtf32(0x02928));
                namedCharacterReferences.Add("top;", char.ConvertFromUtf32(0x022A4));
                namedCharacterReferences.Add("topbot;", char.ConvertFromUtf32(0x02336));
                namedCharacterReferences.Add("topcir;", char.ConvertFromUtf32(0x02AF1));
                namedCharacterReferences.Add("topf;", char.ConvertFromUtf32(0x1D565));
                namedCharacterReferences.Add("topfork;", char.ConvertFromUtf32(0x02ADA));
                namedCharacterReferences.Add("tosa;", char.ConvertFromUtf32(0x02929));
                namedCharacterReferences.Add("tprime;", char.ConvertFromUtf32(0x02034));
                namedCharacterReferences.Add("trade;", char.ConvertFromUtf32(0x02122));
                namedCharacterReferences.Add("triangle;", char.ConvertFromUtf32(0x025B5));
                namedCharacterReferences.Add("triangledown;", char.ConvertFromUtf32(0x025BF));
                namedCharacterReferences.Add("triangleleft;", char.ConvertFromUtf32(0x025C3));
                namedCharacterReferences.Add("trianglelefteq;", char.ConvertFromUtf32(0x022B4));
                namedCharacterReferences.Add("triangleq;", char.ConvertFromUtf32(0x0225C));
                namedCharacterReferences.Add("triangleright;", char.ConvertFromUtf32(0x025B9));
                namedCharacterReferences.Add("trianglerighteq;", char.ConvertFromUtf32(0x022B5));
                namedCharacterReferences.Add("tridot;", char.ConvertFromUtf32(0x025EC));
                namedCharacterReferences.Add("trie;", char.ConvertFromUtf32(0x0225C));
                namedCharacterReferences.Add("triminus;", char.ConvertFromUtf32(0x02A3A));
                namedCharacterReferences.Add("triplus;", char.ConvertFromUtf32(0x02A39));
                namedCharacterReferences.Add("trisb;", char.ConvertFromUtf32(0x029CD));
                namedCharacterReferences.Add("tritime;", char.ConvertFromUtf32(0x02A3B));
                namedCharacterReferences.Add("trpezium;", char.ConvertFromUtf32(0x023E2));
                namedCharacterReferences.Add("tscr;", char.ConvertFromUtf32(0x1D4C9));
                namedCharacterReferences.Add("tscy;", char.ConvertFromUtf32(0x00446));
                namedCharacterReferences.Add("tshcy;", char.ConvertFromUtf32(0x0045B));
                namedCharacterReferences.Add("tstrok;", char.ConvertFromUtf32(0x00167));
                namedCharacterReferences.Add("twixt;", char.ConvertFromUtf32(0x0226C));
                namedCharacterReferences.Add("twoheadleftarrow;", char.ConvertFromUtf32(0x0219E));
                namedCharacterReferences.Add("twoheadrightarrow;", char.ConvertFromUtf32(0x021A0));
                namedCharacterReferences.Add("uArr;", char.ConvertFromUtf32(0x021D1));
                namedCharacterReferences.Add("uHar;", char.ConvertFromUtf32(0x02963));
                namedCharacterReferences.Add("uacute;", char.ConvertFromUtf32(0x000FA));
                namedCharacterReferences.Add("uacute", char.ConvertFromUtf32(0x000FA));
                namedCharacterReferences.Add("uarr;", char.ConvertFromUtf32(0x02191));
                namedCharacterReferences.Add("ubrcy;", char.ConvertFromUtf32(0x0045E));
                namedCharacterReferences.Add("ubreve;", char.ConvertFromUtf32(0x0016D));
                namedCharacterReferences.Add("ucirc;", char.ConvertFromUtf32(0x000FB));
                namedCharacterReferences.Add("ucirc", char.ConvertFromUtf32(0x000FB));
                namedCharacterReferences.Add("ucy;", char.ConvertFromUtf32(0x00443));
                namedCharacterReferences.Add("udarr;", char.ConvertFromUtf32(0x021C5));
                namedCharacterReferences.Add("udblac;", char.ConvertFromUtf32(0x00171));
                namedCharacterReferences.Add("udhar;", char.ConvertFromUtf32(0x0296E));
                namedCharacterReferences.Add("ufisht;", char.ConvertFromUtf32(0x0297E));
                namedCharacterReferences.Add("ufr;", char.ConvertFromUtf32(0x1D532));
                namedCharacterReferences.Add("ugrave;", char.ConvertFromUtf32(0x000F9));
                namedCharacterReferences.Add("ugrave", char.ConvertFromUtf32(0x000F9));
                namedCharacterReferences.Add("uharl;", char.ConvertFromUtf32(0x021BF));
                namedCharacterReferences.Add("uharr;", char.ConvertFromUtf32(0x021BE));
                namedCharacterReferences.Add("uhblk;", char.ConvertFromUtf32(0x02580));
                namedCharacterReferences.Add("ulcorn;", char.ConvertFromUtf32(0x0231C));
                namedCharacterReferences.Add("ulcorner;", char.ConvertFromUtf32(0x0231C));
                namedCharacterReferences.Add("ulcrop;", char.ConvertFromUtf32(0x0230F));
                namedCharacterReferences.Add("ultri;", char.ConvertFromUtf32(0x025F8));
                namedCharacterReferences.Add("umacr;", char.ConvertFromUtf32(0x0016B));
                namedCharacterReferences.Add("uml;", char.ConvertFromUtf32(0x000A8));
                namedCharacterReferences.Add("uml", char.ConvertFromUtf32(0x000A8));
                namedCharacterReferences.Add("uogon;", char.ConvertFromUtf32(0x00173));
                namedCharacterReferences.Add("uopf;", char.ConvertFromUtf32(0x1D566));
                namedCharacterReferences.Add("uparrow;", char.ConvertFromUtf32(0x02191));
                namedCharacterReferences.Add("updownarrow;", char.ConvertFromUtf32(0x02195));
                namedCharacterReferences.Add("upharpoonleft;", char.ConvertFromUtf32(0x021BF));
                namedCharacterReferences.Add("upharpoonright;", char.ConvertFromUtf32(0x021BE));
                namedCharacterReferences.Add("uplus;", char.ConvertFromUtf32(0x0228E));
                namedCharacterReferences.Add("upsi;", char.ConvertFromUtf32(0x003C5));
                namedCharacterReferences.Add("upsih;", char.ConvertFromUtf32(0x003D2));
                namedCharacterReferences.Add("upsilon;", char.ConvertFromUtf32(0x003C5));
                namedCharacterReferences.Add("upuparrows;", char.ConvertFromUtf32(0x021C8));
                namedCharacterReferences.Add("urcorn;", char.ConvertFromUtf32(0x0231D));
                namedCharacterReferences.Add("urcorner;", char.ConvertFromUtf32(0x0231D));
                namedCharacterReferences.Add("urcrop;", char.ConvertFromUtf32(0x0230E));
                namedCharacterReferences.Add("uring;", char.ConvertFromUtf32(0x0016F));
                namedCharacterReferences.Add("urtri;", char.ConvertFromUtf32(0x025F9));
                namedCharacterReferences.Add("uscr;", char.ConvertFromUtf32(0x1D4CA));
                namedCharacterReferences.Add("utdot;", char.ConvertFromUtf32(0x022F0));
                namedCharacterReferences.Add("utilde;", char.ConvertFromUtf32(0x00169));
                namedCharacterReferences.Add("utri;", char.ConvertFromUtf32(0x025B5));
                namedCharacterReferences.Add("utrif;", char.ConvertFromUtf32(0x025B4));
                namedCharacterReferences.Add("uuarr;", char.ConvertFromUtf32(0x021C8));
                namedCharacterReferences.Add("uuml;", char.ConvertFromUtf32(0x000FC));
                namedCharacterReferences.Add("uuml", char.ConvertFromUtf32(0x000FC));
                namedCharacterReferences.Add("uwangle;", char.ConvertFromUtf32(0x029A7));
                namedCharacterReferences.Add("vArr;", char.ConvertFromUtf32(0x021D5));
                namedCharacterReferences.Add("vBar;", char.ConvertFromUtf32(0x02AE8));
                namedCharacterReferences.Add("vBarv;", char.ConvertFromUtf32(0x02AE9));
                namedCharacterReferences.Add("vDash;", char.ConvertFromUtf32(0x022A8));
                namedCharacterReferences.Add("vangrt;", char.ConvertFromUtf32(0x0299C));
                namedCharacterReferences.Add("varepsilon;", char.ConvertFromUtf32(0x003F5));
                namedCharacterReferences.Add("varkappa;", char.ConvertFromUtf32(0x003F0));
                namedCharacterReferences.Add("varnothing;", char.ConvertFromUtf32(0x02205));
                namedCharacterReferences.Add("varphi;", char.ConvertFromUtf32(0x003D5));
                namedCharacterReferences.Add("varpi;", char.ConvertFromUtf32(0x003D6));
                namedCharacterReferences.Add("varpropto;", char.ConvertFromUtf32(0x0221D));
                namedCharacterReferences.Add("varr;", char.ConvertFromUtf32(0x02195));
                namedCharacterReferences.Add("varrho;", char.ConvertFromUtf32(0x003F1));
                namedCharacterReferences.Add("varsigma;", char.ConvertFromUtf32(0x003C2));
                namedCharacterReferences.Add("vartheta;", char.ConvertFromUtf32(0x003D1));
                namedCharacterReferences.Add("vartriangleleft;", char.ConvertFromUtf32(0x022B2));
                namedCharacterReferences.Add("vartriangleright;", char.ConvertFromUtf32(0x022B3));
                namedCharacterReferences.Add("vcy;", char.ConvertFromUtf32(0x00432));
                namedCharacterReferences.Add("vdash;", char.ConvertFromUtf32(0x022A2));
                namedCharacterReferences.Add("vee;", char.ConvertFromUtf32(0x02228));
                namedCharacterReferences.Add("veebar;", char.ConvertFromUtf32(0x022BB));
                namedCharacterReferences.Add("veeeq;", char.ConvertFromUtf32(0x0225A));
                namedCharacterReferences.Add("vellip;", char.ConvertFromUtf32(0x022EE));
                namedCharacterReferences.Add("verbar;", char.ConvertFromUtf32(0x0007C));
                namedCharacterReferences.Add("vert;", char.ConvertFromUtf32(0x0007C));
                namedCharacterReferences.Add("vfr;", char.ConvertFromUtf32(0x1D533));
                namedCharacterReferences.Add("vltri;", char.ConvertFromUtf32(0x022B2));
                namedCharacterReferences.Add("vopf;", char.ConvertFromUtf32(0x1D567));
                namedCharacterReferences.Add("vprop;", char.ConvertFromUtf32(0x0221D));
                namedCharacterReferences.Add("vrtri;", char.ConvertFromUtf32(0x022B3));
                namedCharacterReferences.Add("vscr;", char.ConvertFromUtf32(0x1D4CB));
                namedCharacterReferences.Add("vzigzag;", char.ConvertFromUtf32(0x0299A));
                namedCharacterReferences.Add("wcirc;", char.ConvertFromUtf32(0x00175));
                namedCharacterReferences.Add("wedbar;", char.ConvertFromUtf32(0x02A5F));
                namedCharacterReferences.Add("wedge;", char.ConvertFromUtf32(0x02227));
                namedCharacterReferences.Add("wedgeq;", char.ConvertFromUtf32(0x02259));
                namedCharacterReferences.Add("weierp;", char.ConvertFromUtf32(0x02118));
                namedCharacterReferences.Add("wfr;", char.ConvertFromUtf32(0x1D534));
                namedCharacterReferences.Add("wopf;", char.ConvertFromUtf32(0x1D568));
                namedCharacterReferences.Add("wp;", char.ConvertFromUtf32(0x02118));
                namedCharacterReferences.Add("wr;", char.ConvertFromUtf32(0x02240));
                namedCharacterReferences.Add("wreath;", char.ConvertFromUtf32(0x02240));
                namedCharacterReferences.Add("wscr;", char.ConvertFromUtf32(0x1D4CC));
                namedCharacterReferences.Add("xcap;", char.ConvertFromUtf32(0x022C2));
                namedCharacterReferences.Add("xcirc;", char.ConvertFromUtf32(0x025EF));
                namedCharacterReferences.Add("xcup;", char.ConvertFromUtf32(0x022C3));
                namedCharacterReferences.Add("xdtri;", char.ConvertFromUtf32(0x025BD));
                namedCharacterReferences.Add("xfr;", char.ConvertFromUtf32(0x1D535));
                namedCharacterReferences.Add("xhArr;", char.ConvertFromUtf32(0x027FA));
                namedCharacterReferences.Add("xharr;", char.ConvertFromUtf32(0x027F7));
                namedCharacterReferences.Add("xi;", char.ConvertFromUtf32(0x003BE));
                namedCharacterReferences.Add("xlArr;", char.ConvertFromUtf32(0x027F8));
                namedCharacterReferences.Add("xlarr;", char.ConvertFromUtf32(0x027F5));
                namedCharacterReferences.Add("xmap;", char.ConvertFromUtf32(0x027FC));
                namedCharacterReferences.Add("xnis;", char.ConvertFromUtf32(0x022FB));
                namedCharacterReferences.Add("xodot;", char.ConvertFromUtf32(0x02A00));
                namedCharacterReferences.Add("xopf;", char.ConvertFromUtf32(0x1D569));
                namedCharacterReferences.Add("xoplus;", char.ConvertFromUtf32(0x02A01));
                namedCharacterReferences.Add("xotime;", char.ConvertFromUtf32(0x02A02));
                namedCharacterReferences.Add("xrArr;", char.ConvertFromUtf32(0x027F9));
                namedCharacterReferences.Add("xrarr;", char.ConvertFromUtf32(0x027F6));
                namedCharacterReferences.Add("xscr;", char.ConvertFromUtf32(0x1D4CD));
                namedCharacterReferences.Add("xsqcup;", char.ConvertFromUtf32(0x02A06));
                namedCharacterReferences.Add("xuplus;", char.ConvertFromUtf32(0x02A04));
                namedCharacterReferences.Add("xutri;", char.ConvertFromUtf32(0x025B3));
                namedCharacterReferences.Add("xvee;", char.ConvertFromUtf32(0x022C1));
                namedCharacterReferences.Add("xwedge;", char.ConvertFromUtf32(0x022C0));
                namedCharacterReferences.Add("yacute;", char.ConvertFromUtf32(0x000FD));
                namedCharacterReferences.Add("yacute", char.ConvertFromUtf32(0x000FD));
                namedCharacterReferences.Add("yacy;", char.ConvertFromUtf32(0x0044F));
                namedCharacterReferences.Add("ycirc;", char.ConvertFromUtf32(0x00177));
                namedCharacterReferences.Add("ycy;", char.ConvertFromUtf32(0x0044B));
                namedCharacterReferences.Add("yen;", char.ConvertFromUtf32(0x000A5));
                namedCharacterReferences.Add("yen", char.ConvertFromUtf32(0x000A5));
                namedCharacterReferences.Add("yfr;", char.ConvertFromUtf32(0x1D536));
                namedCharacterReferences.Add("yicy;", char.ConvertFromUtf32(0x00457));
                namedCharacterReferences.Add("yopf;", char.ConvertFromUtf32(0x1D56A));
                namedCharacterReferences.Add("yscr;", char.ConvertFromUtf32(0x1D4CE));
                namedCharacterReferences.Add("yucy;", char.ConvertFromUtf32(0x0044E));
                namedCharacterReferences.Add("yuml;", char.ConvertFromUtf32(0x000FF));
                namedCharacterReferences.Add("yuml", char.ConvertFromUtf32(0x000FF));
                namedCharacterReferences.Add("zacute;", char.ConvertFromUtf32(0x0017A));
                namedCharacterReferences.Add("zcaron;", char.ConvertFromUtf32(0x0017E));
                namedCharacterReferences.Add("zcy;", char.ConvertFromUtf32(0x00437));
                namedCharacterReferences.Add("zdot;", char.ConvertFromUtf32(0x0017C));
                namedCharacterReferences.Add("zeetrf;", char.ConvertFromUtf32(0x02128));
                namedCharacterReferences.Add("zeta;", char.ConvertFromUtf32(0x003B6));
                namedCharacterReferences.Add("zfr;", char.ConvertFromUtf32(0x1D537));
                namedCharacterReferences.Add("zhcy;", char.ConvertFromUtf32(0x00436));
                namedCharacterReferences.Add("zigrarr;", char.ConvertFromUtf32(0x021DD));
                namedCharacterReferences.Add("zopf;", char.ConvertFromUtf32(0x1D56B));
                namedCharacterReferences.Add("zscr;", char.ConvertFromUtf32(0x1D4CF));
                namedCharacterReferences.Add("zwj;", char.ConvertFromUtf32(0x0200D));
                namedCharacterReferences.Add("zwnj;", char.ConvertFromUtf32(0x0200C));
            }
        }
    }
}
