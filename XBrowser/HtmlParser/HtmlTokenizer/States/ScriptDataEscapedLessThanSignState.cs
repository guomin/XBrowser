namespace XBrowserProject.HtmlParser.HtmlTokenizer.States
{
    internal class ScriptDataEscapedLessThanSignState : TokenizerState
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
                if (currentChar == HtmlCharacterUtilities.Solidus)
                {
                    tokenizer.TemporaryBuffer = string.Empty;
                    tokenizer.AdvanceState(new ScriptDataEscapedEndTagOpenState());
                }
                else if (HtmlCharacterUtilities.IsUpperCaseLetter(currentChar))
                {
                    tokenizer.EmitToken(new CharacterToken(HtmlCharacterUtilities.LessThanSign));
                    tokenizer.EmitToken(new CharacterToken(currentChar));
                    tokenizer.TemporaryBuffer = currentChar.ToString().ToLowerInvariant();
                    tokenizer.AdvanceState(new ScriptDataDoubleEscapeStartState());
                }
                else if (HtmlCharacterUtilities.IsLowerCaseLetter(currentChar))
                {
                    tokenizer.EmitToken(new CharacterToken(HtmlCharacterUtilities.LessThanSign));
                    tokenizer.EmitToken(new CharacterToken(currentChar));
                    tokenizer.TemporaryBuffer = currentChar.ToString();
                    tokenizer.AdvanceState(new ScriptDataDoubleEscapeStartState());
                }
                else
                {
                    EmitTokens(tokenizer);
                    tokenizer.ReconsumeInputCharacterInNextState();
                }
            }
            return tokenEmitted;
        }

        private static void EmitTokens(Tokenizer tokenizer)
        {
            tokenizer.EmitToken(new CharacterToken(HtmlCharacterUtilities.LessThanSign));
            tokenizer.AdvanceState(new ScriptDataEscapedState());
        }
    }
}
