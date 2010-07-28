namespace XBrowserProject.HtmlParser.HtmlTokenizer.States
{
    internal class ScriptDataEscapedEndTagNameState : TokenizerState
    {
        private TagToken stateToken;

        public ScriptDataEscapedEndTagNameState(TagToken token)
        {
            stateToken = token;
        }

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
                if (HtmlCharacterUtilities.IsWhiteSpace(currentChar) && tokenizer.IsAppropriateEndTagToken(stateToken))
                {
                    tokenizer.AdvanceState(new BeforeAttributeNameState(stateToken));
                }
                else if (currentChar == HtmlCharacterUtilities.Solidus && tokenizer.IsAppropriateEndTagToken(stateToken))
                {
                    tokenizer.AdvanceState(new SelfClosingStartTagState(stateToken));
                }
                else if (currentChar == HtmlCharacterUtilities.GreaterThanSign && tokenizer.IsAppropriateEndTagToken(stateToken))
                {
                    tokenizer.EmitToken(stateToken);
                    tokenizer.AdvanceState(new DataState());
                    tokenEmitted = true;
                }
                else if (HtmlCharacterUtilities.IsUpperCaseLetter(currentChar))
                {
                    stateToken.Name += currentChar.ToString().ToLowerInvariant();
                    tokenizer.TemporaryBuffer += currentChar;
                }
                else if (HtmlCharacterUtilities.IsLowerCaseLetter(currentChar))
                {
                    stateToken.Name += currentChar;
                    tokenizer.TemporaryBuffer += currentChar;
                }
                else
                {
                    EmitTokens(tokenizer);
                    tokenizer.ReconsumeInputCharacterInNextState();
                    tokenEmitted = true;
                }
            }

            return tokenEmitted;
        }

        private static void EmitTokens(Tokenizer tokenizer)
        {
            tokenizer.EmitToken(new CharacterToken(HtmlCharacterUtilities.LessThanSign));
            tokenizer.EmitToken(new CharacterToken(HtmlCharacterUtilities.Solidus));
            foreach (char character in tokenizer.TemporaryBuffer)
            {
                tokenizer.EmitToken(new CharacterToken(character));
            }

            tokenizer.AdvanceState(new ScriptDataEscapedState());
        }
    }
}
