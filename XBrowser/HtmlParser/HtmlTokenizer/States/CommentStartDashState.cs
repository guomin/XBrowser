namespace XBrowserProject.HtmlParser.HtmlTokenizer.States
{
    internal class CommentStartDashState : TokenizerState
    {
        private CommentToken stateToken;

        public CommentStartDashState(CommentToken token)
        {
            stateToken = token;
        }

        public override bool ParseTokenFromDataStream(Tokenizer tokenizer)
        {
            bool tokenEmitted = false;
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
                else if (currentChar == HtmlCharacterUtilities.GreaterThanSign)
                {
                    tokenizer.LogParseError("Unexpected end to comment tag", "Closing comment tag and continuing");
                    tokenizer.EmitToken(stateToken);
                    tokenizer.AdvanceState(new DataState());
                    tokenEmitted = true;
                }
                else
                {
                    stateToken.Data += HtmlCharacterUtilities.Hyphen;
                    stateToken.Data += currentChar;
                    tokenizer.AdvanceState(new CommentState(stateToken));
                }
            }

            return tokenEmitted;
        }
    }
}
