namespace XBrowserProject.Internal.Html.DOM.Parsing.HtmlTokenizer.States
{
    internal class ScriptDataDoubleEscapedDashState : TokenizerState
    {
        public override bool ParseTokenFromDataStream(Tokenizer tokenizer)
        {
            bool tokenEmitted = false;
            if (tokenizer.IsAtEndOfFile)
            {
                tokenizer.LogParseError("End of input encountered in script data double escaped dash state", "Switching to data state and reconsuming");
                tokenizer.AdvanceState(new DataState());
            }
            else
            {
                char currentChar = tokenizer.ConsumeNextInputCharacter();
                if (currentChar == HtmlCharacterUtilities.Hyphen)
                {
                    tokenizer.EmitToken(new CharacterToken(currentChar));
                    tokenizer.AdvanceState(new ScriptDataDoubleEscapedDashDashState());
                }
                else if (currentChar == HtmlCharacterUtilities.LessThanSign)
                {
                    tokenizer.EmitToken(new CharacterToken(currentChar));
                    tokenizer.AdvanceState(new ScriptDataDoubleEscapedLessThanSignState());
                }
                else
                {
                    tokenizer.EmitToken(new CharacterToken(currentChar));
                    tokenizer.AdvanceState(new ScriptDataDoubleEscapedState());
                    tokenEmitted = true;
                }
            }

            return tokenEmitted;
        }
    }
}
