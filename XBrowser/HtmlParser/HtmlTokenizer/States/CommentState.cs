namespace XBrowserProject.HtmlParser.HtmlTokenizer.States
{
    internal class CommentState : TokenizerState
    {
        private CommentToken stateToken;

        public CommentState(CommentToken token)
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
                    tokenizer.AdvanceState(new CommentEndDashState(stateToken));
                }
                else
                {
                    stateToken.Data += currentChar;
                    tokenizer.AdvanceState(new CommentState(stateToken));
                }
            }

            return false;
        }
    }
}
