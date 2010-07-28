namespace XBrowserProject.HtmlParser.HtmlTokenizer.States
{
    internal class BeforeDocTypeSystemIdentifierState : TokenizerState
    {
        private DocTypeToken stateToken;

        public BeforeDocTypeSystemIdentifierState(DocTypeToken token)
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
                    tokenizer.AdvanceState(new BeforeDocTypeSystemIdentifierState(stateToken));
                }
                else if (currentChar == HtmlCharacterUtilities.GreaterThanSign)
                {
                    tokenizer.LogParseError("End of tag encountered before expected", "Doctype emitted and continuing");
                    stateToken.QuirksMode = true;
                    tokenizer.EmitToken(stateToken);
                    tokenizer.AdvanceState(new DataState());
                    tokenEmitted = true;
                }
                else if (currentChar == HtmlCharacterUtilities.Quote)
                {
                    stateToken.SystemId = string.Empty;
                    tokenizer.AdvanceState(new DocTypeSystemIdentifierDoubleQuotedState(stateToken));
                }
                else if (currentChar == HtmlCharacterUtilities.Apostrophe)
                {
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
