namespace XBrowserProject.Internal.Html.DOM.Parsing.HtmlTokenizer.States
{
    internal class BogusDocTypeState : TokenizerState
    {
        private DocTypeToken stateToken;

        public BogusDocTypeState(DocTypeToken token)
        {
            stateToken = token;
        }

        public override bool ParseTokenFromDataStream(Tokenizer tokenizer)
        {
            bool tokenEmitted = false;
            if (tokenizer.IsAtEndOfFile)
            {
                tokenizer.EmitToken(stateToken);
                tokenizer.AdvanceState(new DataState());
            }
            else
            {
                char currentChar = tokenizer.ConsumeNextInputCharacter();
                if (currentChar == HtmlCharacterUtilities.GreaterThanSign)
                {
                    tokenizer.EmitToken(stateToken);
                    tokenizer.AdvanceState(new DataState());
                    tokenEmitted = true;
                }
                else
                {
                    tokenizer.AdvanceState(new BogusDocTypeState(stateToken));
                }
            }

            return tokenEmitted;
        }
    }
}
