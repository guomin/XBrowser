using System;
using System.Collections.Generic;
using System.Text;

namespace XBrowserProject.HtmlParser
{
    public class HtmlCharacterUtilities
    {
        public const char Ampersand = '&';
        public const char Semicolon = ';';
        public const char Solidus = '/';
        public const char LessThanSign = '<';
        public const char GreaterThanSign = '>';
        public const char EqualsSign = '=';
        public const char Tab = '\t';
        public const char LineFeed = '\n';
        public const char FormFeed = '\f';
        public const char CarriageReturn = '\r';
        public const char Space = ' ';
        public const char Quote = '"';
        public const char Apostrophe = '\'';
        public const char QuestionMark = '?';
        public const char ExclamationMark = '!';
        public const char HashMark = '#';
        public const char Grave = '`';
        public const char RightSquareBracket = ']';
        public const char Hyphen = '-';
        public const char ReplacementCharacter = '\xFFFD';

        public static bool IsUpperCaseLetter(char ch)
        {
            return (ch >= 'A' && ch <= 'Z');
        }

        public static bool IsLowerCaseLetter(char ch)
        {
            return (ch >= 'a' && ch <= 'z');
        }

        public static bool IsDigit(char ch)
        {
            return (ch >= '0' && ch <= '9');
        }

        public static bool IsHexDigit(char ch)
        {
            return IsDigit(ch) || (ch >= 'a' && ch <= 'f') || (ch >= 'A' && ch <= 'F');
        }

        public static bool IsWhiteSpace(char ch)
        {
            return ch == Tab || ch == Space || ch == LineFeed || ch == FormFeed;
        }

        public static bool CharacterCodePointRequiresReplacementCharacter(int codePoint)
        {
            return ((codePoint >= 0xD800 && codePoint <= 0xDFFF) || codePoint > 0x10FFFF || codePoint < 0);
        }

        public static bool CharacterCodePointIsUnprintable(int codePoint)
        {
            bool charIsUnprintable = false;
            if ((codePoint >= 0x0001 && codePoint <= 0x0008) ||
                (codePoint >= 0x000E && codePoint <= 0x001F) ||
                (codePoint >= 0x007F && codePoint <= 0x009F) ||
                (codePoint >= 0xFDD0 && codePoint <= 0xFDEF) ||
                (codePoint == 0x000B) ||
                ((codePoint <= 0x10FFFF) && ((codePoint & 0xFFFE) == 0xFFFE) || (codePoint & 0xFFFF) == 0xFFFF))
            {
                charIsUnprintable = true;
            }

            return charIsUnprintable;
        }

        public static bool IsValidXmlCharacter(char ch)
        {
            return (ch > '\0' && ch < '\xD800') || (ch > '\xE000' && ch < '\xFFFE');
        }

        public static bool IsPrivateUse(char ch)
        {
            return char.GetUnicodeCategory(ch) == System.Globalization.UnicodeCategory.PrivateUse;
        }

        public static bool IsControlCharacter(char ch)
        {
            return (char.IsControl(ch) && ch != Tab && ch != LineFeed);
        }

        public static bool IsInvalidHtmlCharacter(char ch)
        {
            return IsControlCharacter(ch) || (ch >= '\xFDD0' && ch <= '\xFDEF');
        }

        public static bool CodePointIsNonCharacter(int codePoint)
        {
            return (codePoint & 0xFFFE) == 0xFFFE;
        }
    }
}
