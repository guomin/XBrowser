namespace XBrowserProject.HtmlParser.HtmlTokenizer.States
{
    internal class RCDataLessThanSignState : TokenizerState
    {
        public override bool ParseTokenFromDataStream(Tokenizer tokenizer)
        {
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
                    tokenizer.AdvanceState(new RCDataEndTagOpenState());
                }
                else
                {
                    EmitTokens(tokenizer);
                    tokenizer.ReconsumeInputCharacterInNextState();
                }
            }
            return false;
        }

        private static void EmitTokens(Tokenizer tokenizer)
        {
            tokenizer.EmitToken(new CharacterToken(HtmlCharacterUtilities.LessThanSign));
            tokenizer.AdvanceState(new RCDataState());
        }
    }
}
