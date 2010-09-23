namespace XBrowserProject.Internal.Html.DOM.Parsing.HtmlTokenizer.States
{
    internal class ScriptDataState : TokenizerState
    {
        public override bool ParseTokenFromDataStream(Tokenizer tokenizer)
        {
            bool tokenEmitted = false;
            if (tokenizer.IsAtEndOfFile)
            {
                tokenizer.EmitToken(new EndOfFileToken());
                tokenEmitted = true;
            }
            else
            {
                char currentChar = tokenizer.ConsumeNextInputCharacter();
                if (currentChar == HtmlCharacterUtilities.LessThanSign)
                {
                    tokenizer.AdvanceState(new ScriptDataLessThanSignState());
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
