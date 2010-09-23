namespace XBrowserProject.Internal.Html.DOM.Parsing.HtmlTokenizer.States
{
    internal class AfterDocTypeNameState : TokenizerState
    {
        private const string PublicKeyword = "public";
        private const string SystemKeyword = "system";

        private DocTypeToken stateToken;

        public AfterDocTypeNameState(DocTypeToken token)
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
                if (tokenizer.ConsumeKeyword(PublicKeyword, true))
                {
                    tokenizer.AdvanceState(new AfterDocTypePublicKeywordState(stateToken));
                }
                else if (tokenizer.ConsumeKeyword(SystemKeyword, true))
                {
                    tokenizer.AdvanceState(new AfterDocTypeSystemKeywordState(stateToken));
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
                    else
                    {
                        tokenizer.LogParseError("Bogus doctype encountered", "Switch quirks flag and move to bogus doctype state");
                        stateToken.QuirksMode = true;
                        tokenizer.AdvanceState(new BogusDocTypeState(stateToken));
                    }
                }
            }

            return tokenEmitted;
        }
    }
}
