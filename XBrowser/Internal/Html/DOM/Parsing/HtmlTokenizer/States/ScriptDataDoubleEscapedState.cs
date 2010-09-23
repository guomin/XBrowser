namespace XBrowserProject.Internal.Html.DOM.Parsing.HtmlTokenizer.States
{
    internal class ScriptDataDoubleEscapedState : TokenizerState
    {
        public override bool ParseTokenFromDataStream(Tokenizer tokenizer)
        {
            bool tokenEmitted = false;
            if (tokenizer.IsAtEndOfFile)
            {
                tokenizer.LogParseError("End of input encountered in script data double escaped state", "Switching to data state and reconsuming");
                tokenizer.AdvanceState(new DataState());
            }
            else
            {
                char currentChar = tokenizer.ConsumeNextInputCharacter();
                if (currentChar == HtmlCharacterUtilities.Hyphen)
                {
                    tokenizer.EmitToken(new CharacterToken(currentChar));
                    tokenizer.AdvanceState(new ScriptDataDoubleEscapedDashState());
                }
                else if (currentChar == HtmlCharacterUtilities.LessThanSign)
                {
                    tokenizer.EmitToken(new CharacterToken(currentChar));
                    tokenizer.AdvanceState(new ScriptDataDoubleEscapedLessThanSignState());
                }
                else
                {
                    tokenizer.EmitToken(new CharacterToken(currentChar));
                    tokenEmitted = true;
                }
            }

            return tokenEmitted;
        }
    }
}
