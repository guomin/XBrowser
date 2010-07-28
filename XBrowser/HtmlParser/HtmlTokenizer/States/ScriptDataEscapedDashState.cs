namespace XBrowserProject.HtmlParser.HtmlTokenizer.States
{
    internal class ScriptDataEscapedDashState : TokenizerState
    {
        public override bool ParseTokenFromDataStream(Tokenizer tokenizer)
        {
            bool tokenEmitted = false;
            if (tokenizer.IsAtEndOfFile)
            {
                tokenizer.LogParseError("Unexpected EOF token in script data escaped dash state", "Reconsuming in data state");
                tokenizer.AdvanceState(new DataState());
            }
            else
            {
                char currentChar = tokenizer.ConsumeNextInputCharacter();
                if (currentChar == HtmlCharacterUtilities.Hyphen)
                {
                    tokenizer.EmitToken(new CharacterToken(HtmlCharacterUtilities.Hyphen));
                    tokenizer.AdvanceState(new ScriptDataEscapedDashDashState());
                }
                else if (currentChar == HtmlCharacterUtilities.LessThanSign)
                {
                    tokenizer.AdvanceState(new ScriptDataEscapedLessThanSignState());
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
