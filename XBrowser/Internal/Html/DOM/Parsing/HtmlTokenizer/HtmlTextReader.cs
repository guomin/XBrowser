using System;
using System.Text;
using System.IO;
using System.Xml;

namespace XBrowserProject.Internal.Html.DOM.Parsing.HtmlTokenizer
{
    internal class HtmlTextReader : TextReader, IXmlLineInfo
    {
        private TextReader internalReader;
        private bool isMarked;
        private StringBuilder internalBuffer = new StringBuilder();
        private int bufferPosition;
        private bool _lastReadCharWasCarriageReturnInLineFeedLookAhead;
        private bool applyXmlConformanceRules;
        private bool lowSurrogateExpected;

        private int _lineNumber;
        private int _linePosition;
        private int _markLineNumber;
        private int _markLinePosition;

        public HtmlTextReader(TextReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            internalReader = reader;
        }

        public virtual event EventHandler<ParseErrorEventArgs> ParseError;

        public bool ApplyXmlConformanceRules
        {
            get { return applyXmlConformanceRules; }
            set { applyXmlConformanceRules = value; }
        }

        protected virtual void OnParseError(string message, string actionTaken)
        {
            ParseErrorEventArgs args = new ParseErrorEventArgs(message, actionTaken, this);

            if (ParseError != null)
            {
                ParseError(this, args);
            }
        }

        private void OnReplacedNul()
        {
            OnParseError("Null character requires substitution", "replacing with substitution character (0xFFFD)");
        }

        public bool IsMarked
        {
            get { return isMarked; }
        }

        public void Mark()
        {
            if (isMarked)
            {
                throw new InvalidOperationException();
            }

            isMarked = true;
            _markLineNumber = _lineNumber;
            _markLinePosition = _linePosition;
        }

        public void UnsetMark()
        {
            if (!isMarked)
            {
                throw new InvalidOperationException();
            }

            isMarked = false;
            FreeBuffer();
        }

        private void FreeBuffer()
        {
            internalBuffer.Length = 0;
            bufferPosition = 0;
        }

        public void ResetToMark()
        {
            if (!isMarked)
            {
                throw new InvalidOperationException();
            }

            isMarked = false;
            bufferPosition = 0;
            _lineNumber = _markLineNumber;
            _linePosition = _markLinePosition;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                internalReader.Dispose();
                internalBuffer = null;
            }

            base.Dispose(disposing);
        }

        public override int Peek()
        {
            if (internalBuffer == null)
            {
                throw new ObjectDisposedException(null);
            }

            if (!isMarked && internalBuffer.Length > bufferPosition)
            {
                return internalBuffer[bufferPosition];
            }
            else if (_lastReadCharWasCarriageReturnInLineFeedLookAhead)
            {
                return HtmlCharacterUtilities.LineFeed;
            }
            else
            {
                int c = internalReader.Peek();
                if (c == 0)
                {
                    c = HtmlCharacterUtilities.ReplacementCharacter;
                }
                else if (c == HtmlCharacterUtilities.CarriageReturn)
                {
                    internalReader.Read();
                    c = internalReader.Peek();
                    if (c != HtmlCharacterUtilities.LineFeed)
                    {
                        _lastReadCharWasCarriageReturnInLineFeedLookAhead = true;
                    }

                    c = HtmlCharacterUtilities.LineFeed;
                }

                return c;
            }
        }

        public override int Read()
        {
            int c;
            bool readFromBuffer = false;
            if (internalBuffer.Length > 0 && internalBuffer.Length > bufferPosition)
            {
                c = internalBuffer[bufferPosition++];
                readFromBuffer = true;
                if (!isMarked && internalBuffer.Length == bufferPosition)
                {
                    FreeBuffer();
                }
            }
            else if (_lastReadCharWasCarriageReturnInLineFeedLookAhead)
            {
                _lastReadCharWasCarriageReturnInLineFeedLookAhead = false;
                c = HtmlCharacterUtilities.LineFeed;
            }
            else
            {
                c = internalReader.Read();
                if (c >= 0)
                {
                    if (c == 0)
                    {
                        OnReplacedNul();
                        c = HtmlCharacterUtilities.ReplacementCharacter;
                    }
                    else if (c == HtmlCharacterUtilities.CarriageReturn)
                    {
                        c = HtmlCharacterUtilities.LineFeed;
                        if (internalReader.Peek() == HtmlCharacterUtilities.LineFeed)
                        {
                            internalReader.Read();
                        }
                    }
                    else if (c == HtmlCharacterUtilities.FormFeed)
                    {
                        if (applyXmlConformanceRules)
                        {
                            c = HtmlCharacterUtilities.Space;
                        }
                    }
                    else if (char.IsHighSurrogate((char)c))
                    {
                        int nextChar = internalReader.Peek();
                        if (!char.IsLowSurrogate((char)nextChar))
                        {
                            OnParseError("Character with code point " + c.ToString() + " high surrogate, but with no following low surrogate", "none (should use replacement char)");
                        }
                        else
                        {
                            lowSurrogateExpected = true;
                            int fullCodePoint = char.ConvertToUtf32((char)c, (char)nextChar);
                            if (HtmlCharacterUtilities.CharacterCodePointIsUnprintable(fullCodePoint))
                            {
                                OnParseError("Character with code point " + fullCodePoint.ToString() + " is defined as Unicode control or permanently undefined character", "none");
                            }
                        }
                    }
                    else if (char.IsLowSurrogate((char)c))
                    {
                        if (lowSurrogateExpected)
                        {
                            lowSurrogateExpected = false;
                        }
                        else
                        {
                            OnParseError("Character with code point " + c.ToString() + " low surrogate, but with no preceding high surrogate", "none (should use replacement char)");
                        }
                    }
                    else if (HtmlCharacterUtilities.IsInvalidHtmlCharacter((char)c) || HtmlCharacterUtilities.CodePointIsNonCharacter(c))
                    {
                        OnParseError("Character with code point " + c + " is invalid", "none (should use replacement char)");
                        if ((HtmlCharacterUtilities.IsControlCharacter((char)c) || HtmlCharacterUtilities.CodePointIsNonCharacter(c)) && applyXmlConformanceRules)
                        {
                            c = HtmlCharacterUtilities.ReplacementCharacter;
                        }
                    }
                }
            }

            if (c >= 0 && isMarked && !readFromBuffer)
            {
                internalBuffer.Append((char)c);
                bufferPosition = internalBuffer.Length;
            }

            if (c == HtmlCharacterUtilities.LineFeed)
            {
                _lineNumber++;
                _linePosition = 0;
            }
            else
            {
                _linePosition++;
            }

            return c;
        }

        public override int Read(char[] buffer, int index, int count)
        {
            if (internalBuffer.Length <= bufferPosition + count)
            {
                if (_lastReadCharWasCarriageReturnInLineFeedLookAhead && count > 0)
                {
                    _lastReadCharWasCarriageReturnInLineFeedLookAhead = false;
                    internalBuffer.Append(HtmlCharacterUtilities.LineFeed);
                }

                if (internalBuffer.Length <= bufferPosition + count)
                {
                    char[] chars = new char[count - (internalBuffer.Length - bufferPosition + 1)];
                    while (internalBuffer.Length <= bufferPosition + count)
                    {
                        int charsToRead = Math.Min(count, count - (internalBuffer.Length - bufferPosition + 1));

                        int charsRead = internalReader.Read(chars, 0, charsToRead);
                        if (charsRead > 0)
                        {
                            if (chars[charsRead - 1] == HtmlCharacterUtilities.CarriageReturn && internalReader.Peek() == HtmlCharacterUtilities.LineFeed)
                            {
                                chars[charsRead - 1] = HtmlCharacterUtilities.LineFeed;
                                internalReader.Read();
                            }

                            internalBuffer.Append(chars, 0, charsRead);
                            internalBuffer.Replace("\r\n", "\n", internalBuffer.Length - charsRead, charsRead);
                        }

                        if (charsRead < charsToRead)
                        {
                            // At EOF 
                            break;
                        }
                    }
                }
            }

            internalBuffer.Replace(HtmlCharacterUtilities.ReplacementCharacter, HtmlCharacterUtilities.LineFeed, bufferPosition, internalBuffer.Length - bufferPosition + 1);

            int read = Math.Min(count, internalBuffer.Length - bufferPosition + 1);
            internalBuffer.CopyTo(bufferPosition, buffer, index, read);

            for (int i = 0; i < read; i++)
            {
                char c = buffer[index + i];
                if (c == HtmlCharacterUtilities.LineFeed)
                {
                    _lineNumber++;
                    _linePosition = 0;
                }
                else
                {
                    _linePosition++;
                    if (c == '\0')
                    {
                        OnReplacedNul();
                        buffer[index + i] = HtmlCharacterUtilities.ReplacementCharacter;
                    }
                }
            }

            internalBuffer.Replace('\0', HtmlCharacterUtilities.ReplacementCharacter, bufferPosition, internalBuffer.Length - bufferPosition + 1);

            bufferPosition += read;
            if (!isMarked && bufferPosition >= internalBuffer.Length)
            {
                FreeBuffer();
            }

            return read;
        }

        public override int ReadBlock(char[] buffer, int index, int count)
        {
            if (internalBuffer.Length <= bufferPosition + count)
            {
                if (_lastReadCharWasCarriageReturnInLineFeedLookAhead && count > 0)
                {
                    _lastReadCharWasCarriageReturnInLineFeedLookAhead = false;
                    internalBuffer.Append(HtmlCharacterUtilities.LineFeed);
                }
                if (internalBuffer.Length <= bufferPosition + count)
                {
                    char[] chars = new char[count - (internalBuffer.Length - bufferPosition + 1)];
                    while (internalBuffer.Length <= bufferPosition + count)
                    {
                        int charsToRead = Math.Min(count, count - (internalBuffer.Length - bufferPosition + 1));

                        int charsRead = internalReader.ReadBlock(chars, 0, charsToRead);
                        if (charsRead > 0)
                        {

                            if (chars[charsRead - 1] == HtmlCharacterUtilities.CarriageReturn && internalReader.Peek() == HtmlCharacterUtilities.LineFeed)
                            {
                                chars[charsRead - 1] = HtmlCharacterUtilities.LineFeed;
                                internalReader.Read();
                            }

                            internalBuffer.Append(chars, 0, charsRead);
                            internalBuffer.Replace("\r\n", "\n", internalBuffer.Length - charsRead, charsRead);
                        }

                        if (charsRead < charsToRead)
                        {
                            // At EOF 
                            break;
                        }
                    }
                }
            }

            internalBuffer.Replace(HtmlCharacterUtilities.CarriageReturn, HtmlCharacterUtilities.LineFeed, bufferPosition, internalBuffer.Length - bufferPosition + 1);

            int read = Math.Min(count, internalBuffer.Length - bufferPosition + 1);
            internalBuffer.CopyTo(bufferPosition, buffer, index, read);

            for (int i = 0; i < read; i++)
            {
                char c = buffer[index + i];
                if (c == HtmlCharacterUtilities.LineFeed)
                {
                    _lineNumber++;
                    _linePosition = 0;
                }
                else
                {
                    _linePosition++;
                    if (c == '\0')
                    {
                        OnReplacedNul();
                        buffer[index + i] = HtmlCharacterUtilities.ReplacementCharacter;
                    }
                }
            }

            internalBuffer.Replace('\0', HtmlCharacterUtilities.ReplacementCharacter, bufferPosition, internalBuffer.Length - bufferPosition + 1);

            bufferPosition += read;
            if (!isMarked && bufferPosition >= internalBuffer.Length)
            {
                FreeBuffer();
            }

            return read;
        }

        public override string ReadLine()
        {
            throw new NotImplementedException();
        }

        public override string ReadToEnd()
        {
            string toEnd = internalReader.ReadToEnd();
            toEnd = toEnd.Replace("\r\n", "\n");
            toEnd = toEnd.Replace(HtmlCharacterUtilities.CarriageReturn, HtmlCharacterUtilities.LineFeed);

            foreach (char c in toEnd)
            {
                if (c == HtmlCharacterUtilities.LineFeed)
                {
                    _lineNumber++;
                    _linePosition = 0;
                }
                else
                {
                    _linePosition++;
                    if (c == '\0')
                    {
                        OnReplacedNul();
                    }
                }
            }

            toEnd = toEnd.Replace('\0', HtmlCharacterUtilities.ReplacementCharacter);

            if (isMarked)
            {
                internalBuffer.Append(toEnd);
            }
            else if (internalBuffer.Length > bufferPosition)
            {
                toEnd = internalBuffer.ToString(bufferPosition, internalBuffer.Length - bufferPosition) + toEnd;
                FreeBuffer();
            }

            return toEnd;
        }

        #region IXmlLineInfo Membres

        public bool HasLineInfo() { return true; }

        public int LineNumber { get { return _lineNumber; } }

        public int LinePosition { get { return _linePosition; } }

        #endregion
    }
}
