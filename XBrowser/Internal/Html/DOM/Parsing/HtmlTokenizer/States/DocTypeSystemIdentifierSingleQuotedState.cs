namespace XBrowserProject.Internal.Html.DOM.Parsing.HtmlTokenizer.States
{
    internal class DocTypeSystemIdentifierSingleQuotedState : TokenizerState
    {
        private DocTypeToken stateToken;

        public DocTypeSystemIdentifierSingleQuotedState(DocTypeToken token)
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
                else if (currentChar == HtmlCharacterUtilities.Apostrophe)
                {
                    tokenizer.AdvanceState(new AfterDocTypeSystemIdentifierState(stateToken));
                }
                else
                {
                    stateToken.SystemId += currentChar;
                    tokenizer.AdvanceState(new DocTypeSystemIdentifierSingleQuotedState(stateToken));
                }
            }

            return tokenEmitted;
        }
    }
}
