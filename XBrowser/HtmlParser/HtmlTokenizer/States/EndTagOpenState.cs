namespace XBrowserProject.HtmlParser.HtmlTokenizer.States
{
    internal class EndTagOpenState : TokenizerState
    {
        public override bool ParseTokenFromDataStream(Tokenizer tokenizer)
        {
            bool tokenEmitted = false;
            if (tokenizer.IsAtEndOfFile)
            {
                tokenizer.LogParseError("Unexpected end of data", "Creating closing tag");
                tokenizer.EmitToken(new CharacterToken(HtmlCharacterUtilities.LessThanSign));
                tokenizer.EmitToken(new CharacterToken(HtmlCharacterUtilities.Solidus));
                tokenizer.AdvanceState(new DataState());
                tokenEmitted = true;
            }
            else
            {
                char currentChar = tokenizer.ConsumeNextInputCharacter();
                if (HtmlCharacterUtilities.IsLowerCaseLetter(currentChar))
                {
                    TagToken stateToken = new TagToken(TokenType.EndTag, currentChar.ToString());
                    tokenizer.AdvanceState(new TagNameState(stateToken));
                }
                else if (HtmlCharacterUtilities.IsUpperCaseLetter(currentChar))
                {
                    TagToken stateToken = new TagToken(TokenType.EndTag, currentChar.ToString().ToLowerInvariant());
                    tokenizer.AdvanceState(new TagNameState(stateToken));
                }
                else if (currentChar == HtmlCharacterUtilities.GreaterThanSign)
                {
                    tokenizer.LogParseError("Unexpected close bracket (HtmlCharacterUtilities.GreaterThanSign)", "none");
                    tokenizer.AdvanceState(new DataState());
                    tokenEmitted = true;
                }
                else
                {
                    tokenizer.LogParseError("Unexpected character in tag name", "Processing unexpected text as bogus comment");
                    tokenizer.ReconsumeInputCharacterInNextState();
                    tokenizer.AdvanceState(new BogusCommentState());
                }
            }

            return tokenEmitted;
        }
    }
}
