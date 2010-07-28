namespace XBrowserProject.HtmlParser.HtmlTokenizer.States
{
    internal class AttributeValueDoubleQuotedState : TokenizerState
    {
        private TagToken stateToken;

        public AttributeValueDoubleQuotedState(TagToken token)
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
                if (currentChar == HtmlCharacterUtilities.Quote)
                {
                    stateToken.CommitAttributeValue();
                    tokenizer.AdvanceState(new AfterAttributeValueQuotedState(stateToken));
                }
                else if (currentChar == HtmlCharacterUtilities.Ampersand)
                {
                    tokenizer.AdvanceState(new CharacterReferenceInAttributeValueState(stateToken, HtmlCharacterUtilities.Quote));
                }
                else
                {
                    stateToken.CurrentAttributeValue += currentChar;
                    tokenizer.AdvanceState(new AttributeValueDoubleQuotedState(stateToken));
                }
            }

            return false;
        }
    }
}
