namespace XBrowserProject.Internal.Html.DOM.Parsing.HtmlTokenizer.States
{
    internal class ScriptDataEndTagNameState : TokenizerState
    {
        TagToken stateToken;

        public ScriptDataEndTagNameState(TagToken token)
        {
            this.stateToken = token;
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
                    tokenizer.TemporaryBuffer += currentChar;
                    stateToken.Name += currentChar.ToString().ToLowerInvariant();
                }
                else if (HtmlCharacterUtilities.IsLowerCaseLetter(currentChar))
                {
                    tokenizer.TemporaryBuffer += currentChar;
                    stateToken.Name += currentChar;
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

            tokenizer.AdvanceState(new ScriptDataState());
        }
    }
}
