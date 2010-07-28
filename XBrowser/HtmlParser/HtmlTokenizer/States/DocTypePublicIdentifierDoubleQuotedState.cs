namespace XBrowserProject.HtmlParser.HtmlTokenizer.States
{
    internal class DocTypePublicIdentifierDoubleQuotedState : TokenizerState
    {
        private DocTypeToken stateToken;

        public DocTypePublicIdentifierDoubleQuotedState(DocTypeToken token)
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
                    tokenizer.AdvanceState(new AfterDocTypePublicIdentifierState(stateToken));
                }
                else
                {
                    stateToken.PublicId += currentChar;
                    tokenizer.AdvanceState(new DocTypePublicIdentifierDoubleQuotedState(stateToken));
                }
            }

            return tokenEmitted;
        }
    }
}
