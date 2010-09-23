namespace XBrowserProject.Internal.Html.DOM.Parsing.HtmlTokenizer.States
{
    internal class BeforeAttributeNameState : TokenizerState
    {
        private TagToken stateToken;

        public BeforeAttributeNameState(TagToken token)
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
                    tokenizer.EmitToken(stateToken);
                    tokenizer.AdvanceState(new DataState());
                    tokenEmitted = true;
                }
                else if (HtmlCharacterUtilities.IsUpperCaseLetter(currentChar))
                {
                    stateToken.CurrentAttributeName = currentChar.ToString().ToLowerInvariant();
                    stateToken.CurrentAttributeValue = string.Empty;
                    tokenizer.AdvanceState(new AttributeNameState(stateToken));
                }
                else
                {
                    if (currentChar == HtmlCharacterUtilities.Quote || currentChar == HtmlCharacterUtilities.Apostrophe || currentChar == HtmlCharacterUtilities.LessThanSign || currentChar == HtmlCharacterUtilities.EqualsSign)
                    {
                        tokenizer.LogParseError("Invalid character '" + currentChar.ToString() + "' at start of attribute name", "Adding as part of attribute name");
                    }

                    stateToken.CurrentAttributeName = currentChar.ToString();
                    stateToken.CurrentAttributeValue = string.Empty;
                    tokenizer.AdvanceState(new AttributeNameState(stateToken));
                }
            }

            return tokenEmitted;
        }
    }
}
