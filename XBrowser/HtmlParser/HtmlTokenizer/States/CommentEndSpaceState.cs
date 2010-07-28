namespace XBrowserProject.HtmlParser.HtmlTokenizer.States
{
    internal class CommentEndSpaceState : TokenizerState
    {
        private CommentToken stateToken;

        public CommentEndSpaceState(CommentToken token)
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
                    tokenizer.AdvanceState(new CommentEndDashState(stateToken));
                }
                else if (HtmlCharacterUtilities.IsWhiteSpace(currentChar))
                {
                    stateToken.Data += currentChar;
                    tokenizer.AdvanceState(new CommentEndSpaceState(stateToken));
                }
                else if (currentChar == HtmlCharacterUtilities.GreaterThanSign)
                {
                    tokenizer.EmitToken(stateToken);
                    tokenizer.AdvanceState(new DataState());
                    tokenEmitted = true;
                }
                else
                {
                    stateToken.Data += currentChar;
                    tokenizer.AdvanceState(new CommentState(stateToken));
                }
            }

            return tokenEmitted;
        }
    }
}
