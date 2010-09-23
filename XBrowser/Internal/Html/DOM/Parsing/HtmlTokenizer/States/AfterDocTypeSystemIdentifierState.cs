namespace XBrowserProject.Internal.Html.DOM.Parsing.HtmlTokenizer.States
{
    internal class AfterDocTypeSystemIdentifierState : TokenizerState
    {
        private DocTypeToken stateToken;

        public AfterDocTypeSystemIdentifierState(DocTypeToken token)
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
                    tokenizer.AdvanceState(new AfterDocTypeSystemIdentifierState(stateToken));
                }
                else if (currentChar == HtmlCharacterUtilities.GreaterThanSign)
                {
                    tokenizer.EmitToken(stateToken);
                    tokenizer.AdvanceState(new DataState());
                    tokenEmitted = true;
                }
                else
                {
                    tokenizer.LogParseError("Bogus doctype encountered", "Move to bogus doctype state");
                    tokenizer.AdvanceState(new BogusDocTypeState(stateToken));
                }
            }

            return tokenEmitted;
        }
    }
}
