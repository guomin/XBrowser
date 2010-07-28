namespace XBrowserProject.HtmlParser.HtmlTokenizer.States
{
    internal class SelfClosingStartTagState : TokenizerState
    {
        private TagToken stateToken;

        public SelfClosingStartTagState(TagToken token)
        {
            stateToken = token;
        }

        public override bool ParseTokenFromDataStream(Tokenizer tokenizer)
        {
            bool tokenEmitted = false;
            if (tokenizer.IsAtEndOfFile)
            {
                tokenizer.LogParseError("Unexpected end of input", "Resetting to data state and reconsuming");
                tokenizer.AdvanceState(new DataState());
            }
            else
            {
                char currentChar = tokenizer.ConsumeNextInputCharacter();
                if (currentChar == HtmlCharacterUtilities.GreaterThanSign)
                {
                    if (stateToken.TokenType == TokenType.EndTag)
                    {
                        tokenizer.LogParseError("End tags cannot be self-closing", "setting self-closing flag to false");
                    }
                    else
                    {
                        stateToken.IsSelfClosing = true;
                    }

                    tokenizer.EmitToken(stateToken);
                    tokenizer.AdvanceState(new DataState());
                    tokenEmitted = true;
                }
                else
                {
                    tokenizer.LogParseError("Unexpected character in self-closing tag", "Reconsuming character in before attribute name state");
                    tokenizer.ReconsumeInputCharacterInNextState();
                    tokenizer.AdvanceState(new BeforeAttributeNameState(stateToken));
                }
            }

            return tokenEmitted;
        }
    }
}
