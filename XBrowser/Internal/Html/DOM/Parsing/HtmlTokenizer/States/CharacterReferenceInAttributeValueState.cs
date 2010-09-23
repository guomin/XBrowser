namespace XBrowserProject.Internal.Html.DOM.Parsing.HtmlTokenizer.States
{
    internal class CharacterReferenceInAttributeValueState : TokenizerState
    {
        private TagToken stateToken;
        private char stateAdditionalCharacter;

        public CharacterReferenceInAttributeValueState(TagToken token, char additionalCharacter)
        {
            stateToken = token;
            stateAdditionalCharacter = additionalCharacter;
        }

        public override bool ParseTokenFromDataStream(Tokenizer tokenizer)
        {
            string currentChar = tokenizer.ConsumeCharacterReference(stateAdditionalCharacter, true);
            if (string.IsNullOrEmpty(currentChar))
            {
                stateToken.CurrentAttributeValue += HtmlCharacterUtilities.Ampersand;
            }
            else
            {
                stateToken.CurrentAttributeValue += currentChar;
            }

            if (stateAdditionalCharacter == HtmlCharacterUtilities.Quote)
            {
                tokenizer.AdvanceState(new AttributeValueDoubleQuotedState(stateToken));
            }
            else if (stateAdditionalCharacter == HtmlCharacterUtilities.Apostrophe)
            {
                tokenizer.AdvanceState(new AttributeValueSingleQuotedState(stateToken));
            }
            else
            {
                tokenizer.AdvanceState(new AttributeValueUnquotedState(stateToken));
            }

            return false;
        }
    }
}
