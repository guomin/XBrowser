namespace XBrowserProject.HtmlParser.HtmlTokenizer.States
{
    internal class DocTypeNameState : TokenizerState
    {
        private DocTypeToken stateToken;

        public DocTypeNameState(DocTypeToken token)
        {
            stateToken = token;
        }

        public override bool ParseTokenFromDataStream(Tokenizer tokenizer)
        {
            bool tokenEmitted = false;
            if (tokenizer.IsAtEndOfFile)
            {
                tokenizer.LogParseError("End of input encountered before doctype", "Doctype emitted and continuing");
                stateToken.QuirksMode = true;
                tokenizer.EmitToken(stateToken);
                tokenizer.AdvanceState(new DataState());
            }
            else
            {
                char currentChar = tokenizer.ConsumeNextInputCharacter();
                if (HtmlCharacterUtilities.IsWhiteSpace(currentChar))
                {
                    tokenizer.AdvanceState(new AfterDocTypeNameState(stateToken));
                }
                else if (currentChar == HtmlCharacterUtilities.GreaterThanSign)
                {
                    tokenizer.EmitToken(stateToken);
                    tokenizer.AdvanceState(new DataState());
                    tokenEmitted = true;
                }
                else if (HtmlCharacterUtilities.IsUpperCaseLetter(currentChar))
                {
                    stateToken.Name += currentChar.ToString().ToLowerInvariant();
                }
                else
                {
                    stateToken.Name += currentChar;
                }
            }

            return tokenEmitted;
        }
    }
}
