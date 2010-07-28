namespace XBrowserProject.HtmlParser.HtmlTokenizer.States
{
    internal class DocTypeState : TokenizerState
    {
        public override bool ParseTokenFromDataStream(Tokenizer tokenizer)
        {
            bool tokenEmitted = false;
            if (tokenizer.IsAtEndOfFile)
            {
                tokenizer.LogParseError("End of input encountered before doctype", "Doctype emitted and continuing");
                DocTypeToken token = new DocTypeToken();
                token.QuirksMode = true;
                tokenizer.EmitToken(token);
                tokenizer.AdvanceState(new DataState());
                tokenEmitted = true;
            }
            else
            {
                char currentChar = tokenizer.ConsumeNextInputCharacter();
                if (HtmlCharacterUtilities.IsWhiteSpace(currentChar))
                {
                    tokenizer.AdvanceState(new BeforeDocTypeNameState());
                }
                else
                {
                    tokenizer.LogParseError("Unexpected character '" + currentChar.ToString() + "' in doctype", "none");
                    tokenizer.ReconsumeInputCharacterInNextState();
                    tokenizer.AdvanceState(new BeforeDocTypeNameState());
                }
            }

            return tokenEmitted;
        }
    }
}
