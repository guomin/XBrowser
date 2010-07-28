namespace XBrowserProject.HtmlParser.HtmlTokenizer.States
{
    internal class AttributeValueSingleQuotedState : TokenizerState
    {
        private TagToken stateToken;

        public AttributeValueSingleQuotedState(TagToken token)
        {
            stateToken = token;
        }

        public override bool ParseTokenFromDataStream(Tokenizer tokenizer)
        {
            if (tokenizer.IsAtEndOfFile)
            {
                tokenizer.LogParseError("Unexpected end of input", "Resetting to data state and reconsuming");
                tokenizer.AdvanceState(new DataState());
            }
            else
            {
                char currentChar = tokenizer.ConsumeNextInputCharacter();
                if (currentChar == HtmlCharacterUtilities.Apostrophe)
                {
                    stateToken.CommitAttributeValue();
                    tokenizer.AdvanceState(new AfterAttributeValueQuotedState(stateToken));
                }
                else if (currentChar == HtmlCharacterUtilities.Ampersand)
                {
                    tokenizer.AdvanceState(new CharacterReferenceInAttributeValueState(stateToken, HtmlCharacterUtilities.Apostrophe));
                }
                else
                {
                    stateToken.CurrentAttributeValue += currentChar;
                    tokenizer.AdvanceState(new AttributeValueSingleQuotedState(stateToken));
                }
            }

            return false;
        }
    }
}
