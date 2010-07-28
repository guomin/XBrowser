namespace XBrowserProject.HtmlParser.HtmlTokenizer.States
{
    internal class AfterAttributeValueQuotedState : TokenizerState
    {
        private TagToken stateToken;

        public AfterAttributeValueQuotedState(TagToken token)
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
                if (HtmlCharacterUtilities.IsWhiteSpace(currentChar))
                {
                    tokenizer.AdvanceState(new BeforeAttributeNameState(stateToken));
                }
                else if (currentChar == HtmlCharacterUtilities.Solidus)
                {
                    tokenizer.AdvanceState(new SelfClosingStartTagState(stateToken));
                }
                else if (currentChar == HtmlCharacterUtilities.GreaterThanSign)
                {
                    if (stateToken.TokenType == TokenType.EndTag)
                    {
                        if (stateToken.Attributes.Count > 0)
                        {
                            tokenizer.LogParseError("End tag token cannot have attributes", "none");
                        }

                        if (stateToken.IsSelfClosing)
                        {
                            tokenizer.LogParseError("End tag token cannot have self-closing flag set", "none");
                        }
                    }

                    tokenizer.EmitToken(stateToken);
                    tokenizer.AdvanceState(new DataState());
                    tokenEmitted = true;
                }
                else
                {
                    tokenizer.LogParseError("Unexpected character after end of attribute value", "reconsuming in 'before attribute name' state");
                    tokenizer.ReconsumeInputCharacterInNextState();
                    tokenizer.AdvanceState(new BeforeAttributeNameState(stateToken));
                }
            }

            return tokenEmitted;
        }
    }
}
