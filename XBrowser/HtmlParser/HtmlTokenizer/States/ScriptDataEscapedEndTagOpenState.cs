namespace XBrowserProject.HtmlParser.HtmlTokenizer.States
{
    internal class ScriptDataEscapedEndTagOpenState : TokenizerState
    {
        public override bool ParseTokenFromDataStream(Tokenizer tokenizer)
        {
            bool tokenEmitted = false;
            if (tokenizer.IsAtEndOfFile)
            {
                EmitTokens(tokenizer);
            }
            else
            {
                char currentChar = tokenizer.ConsumeNextInputCharacter();
                if (HtmlCharacterUtilities.IsUpperCaseLetter(currentChar))
                {
                    TagToken endTagToken = new TagToken(TokenType.EndTag, currentChar.ToString().ToLowerInvariant());
                    tokenizer.TemporaryBuffer += currentChar;
                    tokenizer.AdvanceState(new ScriptDataEscapedEndTagNameState(endTagToken));
                }
                else if (HtmlCharacterUtilities.IsLowerCaseLetter(currentChar))
                {
                    TagToken endTagToken = new TagToken(TokenType.EndTag, currentChar.ToString());
                    tokenizer.TemporaryBuffer += currentChar;
                    tokenizer.AdvanceState(new ScriptDataEscapedEndTagNameState(endTagToken));
                }
                else
                {
                    EmitTokens(tokenizer);
                    tokenizer.ReconsumeInputCharacterInNextState();
                }
            }

            return tokenEmitted;
        }

        private void EmitTokens(Tokenizer tokenizer)
        {
            tokenizer.EmitToken(new CharacterToken(HtmlCharacterUtilities.LessThanSign));
            tokenizer.EmitToken(new CharacterToken(HtmlCharacterUtilities.Solidus));
            tokenizer.AdvanceState(new ScriptDataEscapedState());
        }
    }
}
