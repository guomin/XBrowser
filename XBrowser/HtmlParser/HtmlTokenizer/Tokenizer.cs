using System;
using System.Collections.Generic;
using System.Text;
using XBrowserProject.HtmlParser.HtmlTokenizer.States;

namespace XBrowserProject.HtmlParser.HtmlTokenizer
{
    public class Tokenizer
    {
        private HtmlTextReader internalReader;
        private TokenizerState state;
        private Queue<Token> tokenQueue = new Queue<Token>();

        private string temporaryBuffer;
        private string currentTagName = string.Empty;
        private bool reconsumeCharacter;
        private char nextInputChar = '\0';

        public Tokenizer(HtmlTextReader reader)
            : this(reader, InitialTokenizerState.Data)
        {
        }

        public Tokenizer(HtmlTextReader reader, InitialTokenizerState initialState)
        {
            internalReader = reader;
            internalReader.ParseError += new EventHandler<ParseErrorEventArgs>(internalReader_ParseError);
            SetInitialState(initialState);
        }

        private void internalReader_ParseError(object sender, ParseErrorEventArgs e)
        {
            LogParseError(e.Description, e.ActionTaken);
        }

        public event EventHandler<ParseErrorEventArgs> ParseError;

        public void SetInitialState(InitialTokenizerState initialState)
        {
            switch (initialState)
            {
                case InitialTokenizerState.RCData:
                    state = new RCDataState();
                    break;

                case InitialTokenizerState.RawText:
                    state = new RawTextState();
                    break;

                case InitialTokenizerState.PlainText:
                    state = new PlainTextState();
                    break;

                case InitialTokenizerState.Script:
                    state = new ScriptDataState();
                    break;

                default:
                    state = new DataState();
                    break;
            }
        }

        public bool IsAtEndOfFile
        {
            get { return !reconsumeCharacter && internalReader.Peek() < 0; } 
        }

        public bool IsAppropriateEndTagToken(TagToken endTagToken)
        {
            return endTagToken != null && endTagToken.TokenType == TokenType.EndTag && endTagToken.Name == currentTagName;
        }

        public string TemporaryBuffer
        {
            get { return temporaryBuffer; }
            set { temporaryBuffer = value; }
        }

        public bool ApplyXmlConformanceRules
        {
            get { return internalReader.ApplyXmlConformanceRules; }
            set { internalReader.ApplyXmlConformanceRules = value; }
        }

        public char ConsumeNextInputCharacter()
        {
            if (!reconsumeCharacter)
            {
                int inputCharValue = internalReader.Read();
                if (inputCharValue >= 0)
                {
                    nextInputChar = (char)inputCharValue;
                }
                else
                {
                    nextInputChar = '\0';
                }
            }
            else
            {
                reconsumeCharacter = false;
            }

            return nextInputChar;
        }

        public string ConsumeCharacterReference(char additionalAllowedCharacter, bool isInAttribute)
        {
            internalReader.Mark();
            CharacterReference charRef = new CharacterReference(internalReader);
            charRef.ParseError += new EventHandler<ParseErrorEventArgs>(charRef_ParseError);
            bool foundCharacterReference = charRef.Parse(additionalAllowedCharacter, isInAttribute);
            if (foundCharacterReference)
            {
                internalReader.UnsetMark();
            }
            else
            {
                internalReader.ResetToMark();
            }

            string charToReturn = charRef.CharacterReferenceText;
            return charToReturn;
        }

        private void charRef_ParseError(object sender, ParseErrorEventArgs e)
        {
            OnParseError(e);
        }

        public bool ConsumeKeyword(string keyword, bool ignoreCase)
        {
            int charCount = 0;
            StringBuilder builder = new StringBuilder();
            internalReader.Mark();
            while (!IsAtEndOfFile && charCount < keyword.Length)
            {
                builder.Append(ConsumeNextInputCharacter());
                charCount++;
            }

            bool keywordFound = string.Compare(builder.ToString(), keyword, ignoreCase) == 0;
            if (keywordFound)
            {
                internalReader.UnsetMark();
            }
            else
            {
                internalReader.ResetToMark();
            }

            return keywordFound;
        }

        public void ReconsumeInputCharacterInNextState()
        {
            reconsumeCharacter = true;
        }

        public void LogParseError(string errorDescription, string actionTaken)
        {
            OnParseError(new ParseErrorEventArgs(errorDescription, actionTaken, internalReader));
        }

        public void EmitToken(Token token)
        {
            tokenQueue.Enqueue(token);
            if (token.TokenType == TokenType.StartTag)
            {
                TagToken startTagToken = token as TagToken;
                currentTagName = startTagToken.Name;
            }
        }

        public void AdvanceState(TokenizerState nextState)
        {
            state = nextState;
        }

        public bool IsNextTokenLineFeed()
        {
            bool nextTokenIsLineFeed = false;
            if (tokenQueue.Count != 0)
            {
                CharacterToken nextToken = tokenQueue.Peek() as CharacterToken;
                nextTokenIsLineFeed = (nextToken != null && nextToken.Data == HtmlCharacterUtilities.LineFeed);
            }
            else
            {
                nextTokenIsLineFeed = internalReader.Peek() == HtmlCharacterUtilities.LineFeed;
            }

            return nextTokenIsLineFeed;
        }

        public Token GetNextToken()
        {
            Token token = null;
            if (tokenQueue.Count == 0)
            {
                bool tokenEmitted = state.ParseTokenFromDataStream(this);
                while (!tokenEmitted)
                {
                    tokenEmitted = state.ParseTokenFromDataStream(this);
                }
            }

            if (tokenQueue.Count == 0)
            {
                token = new EndOfFileToken();
            }
            else
            {
                token = tokenQueue.Dequeue();
            }

            return token;
        }

        protected void OnParseError(ParseErrorEventArgs e)
        {
            if (ParseError != null)
            {
                ParseError(this, e);
            }
        }
    }
}
