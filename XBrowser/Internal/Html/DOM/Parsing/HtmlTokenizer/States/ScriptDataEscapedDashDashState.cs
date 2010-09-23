namespace XBrowserProject.Internal.Html.DOM.Parsing.HtmlTokenizer.States
{
    internal class ScriptDataEscapedDashDashState : TokenizerState
    {
        public override bool ParseTokenFromDataStream(Tokenizer tokenizer)
        {
            bool tokenEmitted = false;
            if (tokenizer.IsAtEndOfFile)
            {
                tokenizer.LogParseError("Unexpected EOF token in script data escaped dash dash state", "Reconsuming in data state");
                tokenizer.AdvanceState(new DataState());
            }
            else
            {
                char currentChar = tokenizer.ConsumeNextInputCharacter();
                if (currentChar == HtmlCharacterUtilities.Hyphen)
                {
                    tokenizer.EmitToken(new CharacterToken(HtmlCharacterUtilities.Hyphen));
                }
                else if (currentChar == HtmlCharacterUtilities.LessThanSign)
                {
                    tokenizer.AdvanceState(new ScriptDataEscapedLessThanSignState());
                }
                else if (currentChar == HtmlCharacterUtilities.GreaterThanSign)
                {
                    tokenizer.EmitToken(new CharacterToken(currentChar));
                    tokenizer.AdvanceState(new ScriptDataState());
                    tokenEmitted = true;
                }
                else
                {
                    tokenizer.EmitToken(new CharacterToken(currentChar));
                    tokenizer.AdvanceState(new ScriptDataEscapedState());
                    tokenEmitted = true;
                }
            }

            return tokenEmitted;
        }
    }
}
