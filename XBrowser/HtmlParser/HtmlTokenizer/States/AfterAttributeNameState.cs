namespace XBrowserProject.HtmlParser.HtmlTokenizer.States
{
    internal class AfterAttributeNameState : TokenizerState
    {
        private TagToken stateToken;

        public AfterAttributeNameState(TagToken token)
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
                    tokenizer.AdvanceState(new AfterAttributeNameState(stateToken));
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
                else if (currentChar == HtmlCharacterUtilities.EqualsSign)
                {
                    tokenizer.AdvanceState(new BeforeAttributeValueState(stateToken));
                }
                else if (HtmlCharacterUtilities.IsUpperCaseLetter(currentChar))
                {
                    stateToken.CurrentAttributeName = currentChar.ToString().ToLowerInvariant();
                    tokenizer.AdvanceState(new AttributeNameState(stateToken));
                }
                else
                {
                    if (currentChar == HtmlCharacterUtilities.Quote || currentChar == HtmlCharacterUtilities.Apostrophe || currentChar == HtmlCharacterUtilities.LessThanSign)
                    {
                        tokenizer.LogParseError("Invalid character '" + currentChar.ToString() + "' at start of attribute name", "Adding as part of attribute name");
                    }

                    stateToken.CurrentAttributeName = currentChar.ToString();
                    tokenizer.AdvanceState(new AttributeNameState(stateToken));
                }
            }

            return tokenEmitted;
        }
    }
}
