namespace XBrowserProject.Internal.Html.DOM.Parsing.HtmlTokenizer.States
{
    internal class DataState : TokenizerState
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
                if (currentChar == HtmlCharacterUtilities.Ampersand)
                {
                    tokenizer.AdvanceState(new CharacterReferenceInDataState());
                }
                else if (currentChar == HtmlCharacterUtilities.LessThanSign)
                {
                    tokenizer.AdvanceState(new TagOpenState());
                }
                else
                {
                    if (tokenizer.ApplyXmlConformanceRules)
                    {
                        if (currentChar == HtmlCharacterUtilities.FormFeed)
                        {
                            currentChar = HtmlCharacterUtilities.Space;
                        }
                        else if (!HtmlCharacterUtilities.IsValidXmlCharacter(currentChar))
                        {
                            currentChar = HtmlCharacterUtilities.ReplacementCharacter;
                        }
                    }

                    tokenizer.EmitToken(new CharacterToken(currentChar));
                    if (!char.IsHighSurrogate(currentChar))
                    {
                        tokenEmitted = true;
                    }
                }
            }

            return tokenEmitted;
        }
    }
}
