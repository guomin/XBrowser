namespace XBrowserProject.Internal.Html.DOM.Parsing.HtmlTokenizer.States
{
    internal class BeforeAttributeValueState : TokenizerState
    {
        private TagToken stateToken;

        public BeforeAttributeValueState(TagToken token)
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
                    tokenizer.AdvanceState(new BeforeAttributeValueState(stateToken));
                }
                else if (currentChar == HtmlCharacterUtilities.Quote)
                {
                    tokenizer.AdvanceState(new AttributeValueDoubleQuotedState(stateToken));
                }
                else if (currentChar == HtmlCharacterUtilities.Apostrophe)
                {
                    tokenizer.AdvanceState(new AttributeValueSingleQuotedState(stateToken));
                }
                else if (currentChar == HtmlCharacterUtilities.GreaterThanSign)
                {
                    tokenizer.LogParseError("Unexpected close tag (HtmlCharacterUtilities.GreaterThanSign) before attribute value", "Closing tag and proceeding");
                    tokenizer.EmitToken(stateToken);
                    tokenizer.AdvanceState(new DataState());
                    tokenEmitted = true;
                }
                else if (currentChar == HtmlCharacterUtilities.Ampersand)
                {
                    tokenizer.ReconsumeInputCharacterInNextState();
                    tokenizer.AdvanceState(new AttributeValueUnquotedState(stateToken));
                }
                else
                {
                    if (currentChar == HtmlCharacterUtilities.LessThanSign || currentChar == '=' || currentChar == '`')
                    {
                        tokenizer.LogParseError("Invalid character ('" + currentChar.ToString() + "') before attribute value", "Closing tag and proceeding");
                    }

                    stateToken.CurrentAttributeValue += currentChar;
                    tokenizer.AdvanceState(new AttributeValueUnquotedState(stateToken));
                }
            }

            return tokenEmitted;
        }
    }
}
