namespace XBrowserProject.Internal.Html.DOM.Parsing.HtmlTokenizer.States
{
    internal class ScriptDataLessThanSignState : TokenizerState
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
                    tokenizer.AdvanceState(new ScriptDataEndTagOpenState());
                }
                else if (currentChar == HtmlCharacterUtilities.ExclamationMark)
                {
                    tokenizer.EmitToken(new CharacterToken(HtmlCharacterUtilities.LessThanSign));
                    tokenizer.EmitToken(new CharacterToken(HtmlCharacterUtilities.ExclamationMark));
                    tokenizer.AdvanceState(new ScriptDataEscapeStartState());
                    tokenEmitted = true;
                }
                else
                {
                    tokenizer.ReconsumeInputCharacterInNextState();
                    EmitTokens(tokenizer);
                    tokenEmitted = true;
                }
            }
            return tokenEmitted;
        }

        private static void EmitTokens(Tokenizer tokenizer)
        {
            tokenizer.EmitToken(new CharacterToken(HtmlCharacterUtilities.LessThanSign));
            tokenizer.AdvanceState(new ScriptDataState());
        }
    }
}
