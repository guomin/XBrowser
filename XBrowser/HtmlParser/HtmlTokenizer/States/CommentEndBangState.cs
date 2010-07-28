namespace XBrowserProject.HtmlParser.HtmlTokenizer.States
{
    internal class CommentEndBangState : TokenizerState
    {
        private CommentToken stateToken;

        public CommentEndBangState(CommentToken token)
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
                    stateToken.Data += HtmlCharacterUtilities.Hyphen;
                    stateToken.Data += HtmlCharacterUtilities.Hyphen;
                    stateToken.Data += HtmlCharacterUtilities.ExclamationMark;
                    tokenizer.AdvanceState(new CommentEndDashState(stateToken));
                }
                else if (currentChar == HtmlCharacterUtilities.GreaterThanSign)
                {
                    tokenizer.EmitToken(stateToken);
                    tokenizer.AdvanceState(new DataState());
                    tokenEmitted = true;
                }
                else
                {
                    stateToken.Data += HtmlCharacterUtilities.Hyphen;
                    stateToken.Data += HtmlCharacterUtilities.Hyphen;
                    stateToken.Data += HtmlCharacterUtilities.ExclamationMark;
                    stateToken.Data += currentChar;
                    tokenizer.AdvanceState(new CommentState(stateToken));
                }
            }

            return tokenEmitted;
        }
    }
}
