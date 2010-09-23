namespace XBrowserProject.Internal.Html.DOM.Parsing.HtmlTokenizer.States
{
    internal class RCDataEndTagOpenState : TokenizerState
    {
        public override bool ParseTokenFromDataStream(Tokenizer tokenizer)
        {
            if (tokenizer.IsAtEndOfFile)
            {
                EmitTokens(tokenizer);
            }
            else
            {
                char currentChar = tokenizer.ConsumeNextInputCharacter();
                if (HtmlCharacterUtilities.IsUpperCaseLetter(currentChar))
                {
                    TagToken stateToken = new TagToken(TokenType.EndTag, currentChar.ToString().ToLowerInvariant());
                    tokenizer.TemporaryBuffer += currentChar;
                    tokenizer.AdvanceState(new RCDataEndTagNameState(stateToken));
                }
                else if (HtmlCharacterUtilities.IsLowerCaseLetter(currentChar))
                {
                    TagToken stateToken = new TagToken(TokenType.EndTag, currentChar.ToString());
                    tokenizer.TemporaryBuffer += currentChar;
                    tokenizer.AdvanceState(new RCDataEndTagNameState(stateToken));
                }
                else
                {
                    EmitTokens(tokenizer);
                    tokenizer.ReconsumeInputCharacterInNextState();
                }
            }

            return false;
        }

        private static void EmitTokens(Tokenizer tokenizer)
        {
            tokenizer.EmitToken(new CharacterToken(HtmlCharacterUtilities.LessThanSign));
            tokenizer.EmitToken(new CharacterToken(HtmlCharacterUtilities.Solidus));
            tokenizer.AdvanceState(new RCDataState());
        }
    }
}
