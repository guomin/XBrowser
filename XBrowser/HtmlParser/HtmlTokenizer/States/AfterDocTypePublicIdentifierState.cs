namespace XBrowserProject.HtmlParser.HtmlTokenizer.States
{
    internal class AfterDocTypePublicIdentifierState : TokenizerState
    {
        private DocTypeToken stateToken;

        public AfterDocTypePublicIdentifierState(DocTypeToken token)
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
                    tokenizer.AdvanceState(new BetweenDocTypePublicAndSystemIdentifiersState(stateToken));
                }
                else if (currentChar == HtmlCharacterUtilities.GreaterThanSign)
                {
                    tokenizer.EmitToken(stateToken);
                    tokenizer.AdvanceState(new DataState());
                    tokenEmitted = true;
                }
                else if (currentChar == HtmlCharacterUtilities.Quote)
                {
                    tokenizer.LogParseError("Expected space before quote for system identifier", "none");
                    stateToken.SystemId = string.Empty;
                    tokenizer.AdvanceState(new DocTypeSystemIdentifierDoubleQuotedState(stateToken));
                }
                else if (currentChar == HtmlCharacterUtilities.Apostrophe)
                {
                    tokenizer.LogParseError("Expected space before quote for system identifier", "none");
                    stateToken.SystemId = string.Empty;
                    tokenizer.AdvanceState(new DocTypeSystemIdentifierSingleQuotedState(stateToken));
                }
                else
                {
                    tokenizer.LogParseError("Bogus doctype encountered", "Switch quirks flag and move to bogus doctype state");
                    stateToken.QuirksMode = true;
                    tokenizer.AdvanceState(new BogusDocTypeState(stateToken));
                }
            }

            return tokenEmitted;
        }
    }
}
