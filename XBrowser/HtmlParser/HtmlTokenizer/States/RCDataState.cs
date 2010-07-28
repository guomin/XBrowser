namespace XBrowserProject.HtmlParser.HtmlTokenizer.States
{
    internal class RCDataState : TokenizerState
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
                    tokenizer.AdvanceState(new RCDataLessThanSignState());
                }
                else if (currentChar == HtmlCharacterUtilities.Ampersand)
                {
                    tokenizer.AdvanceState(new CharacterReferenceInRCDataState());
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
