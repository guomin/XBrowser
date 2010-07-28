namespace XBrowserProject.HtmlParser.HtmlTokenizer.States
{
    internal class ScriptDataEndTagOpenState : TokenizerState
    {
        private TagToken token;

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
                    token = new TagToken(TokenType.EndTag, currentChar.ToString().ToLowerInvariant());
                    tokenizer.TemporaryBuffer += currentChar;
                    tokenizer.AdvanceState(new ScriptDataEndTagNameState(token));
                }
                else if (HtmlCharacterUtilities.IsLowerCaseLetter(currentChar))
                {
                    token = new TagToken(TokenType.EndTag, currentChar.ToString());
                    tokenizer.TemporaryBuffer += currentChar;
                    tokenizer.AdvanceState(new ScriptDataEndTagNameState(token));
                }
                else
                {
                    tokenizer.ReconsumeInputCharacterInNextState();
                    EmitTokens(tokenizer);
                    tokenEmitted = true;
                }
            }

            return tokenEmitted;
        }

        private static void EmitTokens(Tokenizer tokenizer)
        {
            tokenizer.EmitToken(new CharacterToken(HtmlCharacterUtilities.LessThanSign));
            tokenizer.EmitToken(new CharacterToken(HtmlCharacterUtilities.Solidus));
            tokenizer.AdvanceState(new ScriptDataState());
        }
    }
}
