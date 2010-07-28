namespace XBrowserProject.HtmlParser.HtmlTokenizer.States
{
    internal class CommentEndDashState : TokenizerState
    {
        private CommentToken stateToken;

        public CommentEndDashState(CommentToken token)
        {
            stateToken = token;
        }

        public override bool ParseTokenFromDataStream(Tokenizer tokenizer)
        {
            if (tokenizer.IsAtEndOfFile)
            {
                tokenizer.LogParseError("Unexpected end of input", "Resetting to data state and reconsuming");
                tokenizer.EmitToken(stateToken);
                tokenizer.AdvanceState(new DataState());
            }
            else
            {
                char currentChar = tokenizer.ConsumeNextInputCharacter();
                if (currentChar == HtmlCharacterUtilities.Hyphen)
                {
                    tokenizer.AdvanceState(new CommentEndState(stateToken));
                }
                else
                {
                    stateToken.Data += HtmlCharacterUtilities.Hyphen;
                    stateToken.Data += currentChar;
                    tokenizer.AdvanceState(new CommentState(stateToken));
                }
            }

            return false;
        }
    }
}
