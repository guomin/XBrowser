namespace XBrowserProject.Internal.Html.DOM.Parsing.HtmlTokenizer.States
{
    internal class PlainTextState : TokenizerState
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
                tokenizer.EmitToken(new CharacterToken(currentChar));
                tokenizer.AdvanceState(new PlainTextState());
            }

            return tokenEmitted;
        }
    }
}
