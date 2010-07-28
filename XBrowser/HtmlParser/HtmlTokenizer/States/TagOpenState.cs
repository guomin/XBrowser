namespace XBrowserProject.HtmlParser.HtmlTokenizer.States
{
    internal class TagOpenState : TokenizerState
    {
        public override bool ParseTokenFromDataStream(Tokenizer tokenizer)
        {
            bool tokenEmitted = false;
            if (!tokenizer.IsAtEndOfFile)
            {
                char currentChar = tokenizer.ConsumeNextInputCharacter();
                if (currentChar == HtmlCharacterUtilities.ExclamationMark)
                {
                    tokenizer.AdvanceState(new MarkupDeclarationOpenState());
                }
                else if (currentChar == HtmlCharacterUtilities.Solidus)
                {
                    tokenizer.AdvanceState(new EndTagOpenState());
                }
                else if (HtmlCharacterUtilities.IsLowerCaseLetter(currentChar))
                {
                    TagToken stateToken = new TagToken(TokenType.StartTag, currentChar.ToString());
                    tokenizer.AdvanceState(new TagNameState(stateToken));
                }
                else if (HtmlCharacterUtilities.IsUpperCaseLetter(currentChar))
                {
                    TagToken stateToken = new TagToken(TokenType.StartTag, currentChar.ToString().ToLowerInvariant());
                    tokenizer.AdvanceState(new TagNameState(stateToken));
                }
                else if (currentChar == HtmlCharacterUtilities.QuestionMark)
                {
                    tokenizer.LogParseError("Bogus comment in source", "Correcting for bogus comment");
                    tokenizer.ReconsumeInputCharacterInNextState();
                    tokenizer.AdvanceState(new BogusCommentState());
                }
                else
                {
                    tokenizer.LogParseError("Invalid character '" + currentChar + "' at start of tag name", "Emitting empty element");

                    // Move the position pointer back so the current character will be reconsumed.
                    tokenizer.ReconsumeInputCharacterInNextState();
                    tokenizer.EmitToken(new CharacterToken(HtmlCharacterUtilities.LessThanSign));
                    tokenizer.AdvanceState(new DataState());
                    tokenEmitted = true;
                }
            }
            else
            {
                tokenizer.LogParseError("Unexpected end of file at start of tag name", "Emitting less than sign");

                tokenizer.EmitToken(new CharacterToken(HtmlCharacterUtilities.LessThanSign));
                tokenizer.AdvanceState(new DataState());
                tokenEmitted = true;
            }

            return tokenEmitted;
        }
    }
}
