namespace XBrowserProject.Internal.Html.DOM.Parsing.HtmlTokenizer.States
{
    internal class AttributeNameState : TokenizerState
    {
        private TagToken stateToken;

        public AttributeNameState(TagToken token)
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
                    CommitAttributeName(tokenizer);
                    tokenizer.AdvanceState(new AfterAttributeNameState(stateToken));
                }
                else if (currentChar == HtmlCharacterUtilities.Solidus)
                {
                    CommitAttributeName(tokenizer);
                    tokenizer.AdvanceState(new SelfClosingStartTagState(stateToken));
                }
                else if (currentChar == HtmlCharacterUtilities.GreaterThanSign)
                {
                    CommitAttributeName(tokenizer);
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
                    CommitAttributeName(tokenizer);
                    tokenizer.AdvanceState(new BeforeAttributeValueState(stateToken));
                }
                else if (HtmlCharacterUtilities.IsUpperCaseLetter(currentChar))
                {
                    stateToken.CurrentAttributeName += currentChar.ToString().ToLowerInvariant();
                    tokenizer.AdvanceState(new AttributeNameState(stateToken));
                }
                else
                {
                    if (currentChar == HtmlCharacterUtilities.Quote ||
                        currentChar == HtmlCharacterUtilities.Apostrophe ||
                        currentChar == HtmlCharacterUtilities.LessThanSign)
                    {
                        tokenizer.LogParseError("Attribute name cannot contain '" + currentChar.ToString() + "'", "none");
                    }

                    stateToken.CurrentAttributeName += currentChar;
                    tokenizer.AdvanceState(new AttributeNameState(stateToken));
                }
            }

            return tokenEmitted;
        }

        private void CommitAttributeName(Tokenizer tokenizer)
        {
            if (!stateToken.CommitAttributeName())
            {
                tokenizer.LogParseError("Attribute name '" + stateToken.CurrentAttributeName + "' already exists", "Duplicate attribute name and value will be dropped");
            }
        }
    }
}
