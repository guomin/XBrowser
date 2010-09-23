namespace XBrowserProject.Internal.Html.DOM.Parsing.HtmlTokenizer.States
{
    internal class ScriptDataDoubleEscapeEndState : TokenizerState
    {
        public override bool ParseTokenFromDataStream(Tokenizer tokenizer)
        {
            bool tokenEmitted = false;
            if (tokenizer.IsAtEndOfFile)
            {
                tokenizer.AdvanceState(new ScriptDataDoubleEscapedState());
            }
            else
            {
                char currentChar = tokenizer.ConsumeNextInputCharacter();
                if (HtmlCharacterUtilities.IsWhiteSpace(currentChar) ||
                    currentChar == HtmlCharacterUtilities.Solidus ||
                    currentChar == HtmlCharacterUtilities.GreaterThanSign)
                {
                    tokenizer.EmitToken(new CharacterToken(currentChar));
                    if (tokenizer.TemporaryBuffer == "script")
                    {
                        tokenizer.AdvanceState(new ScriptDataEscapedState());
                    }
                    else
                    {
                        tokenizer.AdvanceState(new ScriptDataDoubleEscapedState());
                    }

                    tokenEmitted = true;
                }
                else if (HtmlCharacterUtilities.IsUpperCaseLetter(currentChar))
                {
                    tokenizer.EmitToken(new CharacterToken(currentChar));
                    tokenizer.TemporaryBuffer += currentChar.ToString().ToLowerInvariant();
                }
                else if (HtmlCharacterUtilities.IsLowerCaseLetter(currentChar))
                {
                    tokenizer.EmitToken(new CharacterToken(currentChar));
                    tokenizer.TemporaryBuffer += currentChar;
                }
                else
                {
                    tokenizer.ReconsumeInputCharacterInNextState();
                    tokenizer.AdvanceState(new ScriptDataDoubleEscapedState());
                }
            }
            return tokenEmitted;
        }
    }
}
