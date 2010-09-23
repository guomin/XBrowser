namespace XBrowserProject.Internal.Html.DOM.Parsing.HtmlTokenizer.States
{
    internal class ScriptDataEscapedState : TokenizerState
    {
        public override bool ParseTokenFromDataStream(Tokenizer tokenizer)
        {
            bool tokenEmitted = false;
            if (tokenizer.IsAtEndOfFile)
            {
                tokenizer.LogParseError("Unexpected EOF token in script data escaped state", "Reconsuming in data state");
                tokenizer.AdvanceState(new DataState());
            }
            else
            {
                char currentChar = tokenizer.ConsumeNextInputCharacter();
                if (currentChar == HtmlCharacterUtilities.Hyphen)
                {
                    tokenizer.EmitToken(new CharacterToken(currentChar));
                    tokenizer.AdvanceState(new ScriptDataEscapedDashState());
                }
                else if (currentChar == HtmlCharacterUtilities.LessThanSign)
                {
                    tokenizer.AdvanceState(new ScriptDataEscapedLessThanSignState());
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
