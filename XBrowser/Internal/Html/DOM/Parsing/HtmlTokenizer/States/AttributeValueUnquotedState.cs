namespace XBrowserProject.Internal.Html.DOM.Parsing.HtmlTokenizer.States
{
    internal class AttributeValueUnquotedState : TokenizerState
    {
        private TagToken stateToken;

        public AttributeValueUnquotedState(TagToken token)
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
                    stateToken.CommitAttributeValue();
                    tokenizer.AdvanceState(new BeforeAttributeNameState(stateToken));
                }
                else if (currentChar == HtmlCharacterUtilities.Ampersand)
                {
                    tokenizer.AdvanceState(new CharacterReferenceInAttributeValueState(stateToken, HtmlCharacterUtilities.GreaterThanSign));
                }
                else if (currentChar == HtmlCharacterUtilities.GreaterThanSign)
                {
                    stateToken.CommitAttributeValue();
                    tokenizer.EmitToken(stateToken);
                    tokenizer.AdvanceState(new DataState());
                    tokenEmitted = true;
                }
                else
                {
                    if (currentChar == HtmlCharacterUtilities.Quote ||
                        currentChar == HtmlCharacterUtilities.Apostrophe ||
                        currentChar == HtmlCharacterUtilities.LessThanSign ||
                        currentChar == HtmlCharacterUtilities.EqualsSign ||
                        currentChar == HtmlCharacterUtilities.Grave)
                    {
                        tokenizer.LogParseError("Invalid character '" + currentChar.ToString() + "' in attribute value", "Adding as part of attribute value and continuing");
                    }

                    stateToken.CurrentAttributeValue += currentChar;
                    tokenizer.AdvanceState(new AttributeValueUnquotedState(stateToken));
                }
            }

            return tokenEmitted;
        }
    }
}
