namespace XBrowserProject.HtmlParser.HtmlTokenizer.States
{
    internal class DocTypeSystemIdentifierDoubleQuotedState : TokenizerState
    {
        private DocTypeToken stateToken;

        public DocTypeSystemIdentifierDoubleQuotedState(DocTypeToken token)
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
                if (currentChar == HtmlCharacterUtilities.GreaterThanSign)
                {
                    tokenizer.LogParseError("End of tag encountered before expected", "Doctype emitted and continuing");
                    stateToken.QuirksMode = true;
                    tokenizer.EmitToken(stateToken);
                    tokenizer.AdvanceState(new DataState());
                    tokenEmitted = true;
                }
                else if (currentChar == HtmlCharacterUtilities.Quote)
                {
                    tokenizer.AdvanceState(new AfterDocTypeSystemIdentifierState(stateToken));
                }
                else
                {
                    stateToken.SystemId += currentChar;
                    tokenizer.AdvanceState(new DocTypeSystemIdentifierDoubleQuotedState(stateToken));
                }
            }

            return tokenEmitted;
        }
    }
}
