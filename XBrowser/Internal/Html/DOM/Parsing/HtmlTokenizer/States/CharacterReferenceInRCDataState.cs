namespace XBrowserProject.Internal.Html.DOM.Parsing.HtmlTokenizer.States
{
    internal class CharacterReferenceInRCDataState : TokenizerState
    {
        public override bool ParseTokenFromDataStream(Tokenizer tokenizer)
        {
            string currentChar = string.Empty;
            if (!tokenizer.IsAtEndOfFile)
            {
                currentChar = tokenizer.ConsumeCharacterReference(char.MinValue, false);
                if (string.IsNullOrEmpty(currentChar))
                {
                    currentChar = "&";
                }
            }

            foreach (char character in currentChar)
            {
                tokenizer.EmitToken(new CharacterToken(character));
            }

            tokenizer.AdvanceState(new RCDataState());
            return false;
        }
    }
}
