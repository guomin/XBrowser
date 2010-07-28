namespace XBrowserProject.HtmlParser.HtmlTokenizer.States
{
    internal class BogusCommentState : TokenizerState
    {
        public override bool ParseTokenFromDataStream(Tokenizer tokenizer)
        {
            string commentData = string.Empty;

            if (!tokenizer.IsAtEndOfFile)
            {
                char currentChar = tokenizer.ConsumeNextInputCharacter();
                bool endOfFile = false;
                while (currentChar != HtmlCharacterUtilities.GreaterThanSign && !endOfFile)
                {
                    commentData += currentChar;
                    if (!tokenizer.IsAtEndOfFile)
                    {
                        currentChar = tokenizer.ConsumeNextInputCharacter();
                    }
                    else
                    {
                        endOfFile = true;
                    }
                }
            }

            tokenizer.EmitToken(new CommentToken(commentData));
            tokenizer.AdvanceState(new DataState());
            return false;
        }
    }
}
