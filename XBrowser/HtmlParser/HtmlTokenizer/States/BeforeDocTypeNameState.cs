namespace XBrowserProject.HtmlParser.HtmlTokenizer.States
{
    internal class BeforeDocTypeNameState : TokenizerState
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
            }
            else
            {
                char currentChar = tokenizer.ConsumeNextInputCharacter();
                if (HtmlCharacterUtilities.IsWhiteSpace(currentChar))
                {
                    tokenizer.AdvanceState(new BeforeDocTypeNameState());
                }
                else if (currentChar == HtmlCharacterUtilities.GreaterThanSign)
                {
                    tokenizer.LogParseError("Unexpected end of doctype tag encountered", "Doctype emitted and continuing");
                    DocTypeToken token = new DocTypeToken();
                    token.QuirksMode = true;
                    tokenizer.EmitToken(token);
                    tokenizer.AdvanceState(new DataState());
                    tokenEmitted = true;
                }
                else if (HtmlCharacterUtilities.IsUpperCaseLetter(currentChar))
                {
                    DocTypeToken token = new DocTypeToken();
                    token.Name = currentChar.ToString().ToLowerInvariant();
                    tokenizer.AdvanceState(new DocTypeNameState(token));
                }
                else
                {
                    DocTypeToken token = new DocTypeToken();
                    token.Name = currentChar.ToString();
                    tokenizer.AdvanceState(new DocTypeNameState(token));
                }
            }

            return tokenEmitted;
        }
    }
}
