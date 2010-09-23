namespace XBrowserProject.Internal.Html.DOM.Parsing.HtmlTokenizer.States
{
    internal class CDataSectionState : TokenizerState
    {
        public override bool ParseTokenFromDataStream(Tokenizer tokenizer)
        {
            if (!tokenizer.IsAtEndOfFile)
            {
                bool firstEndMarkerFound = false;
                bool secondEndMarkerFound = false;
                char currentChar = tokenizer.ConsumeNextInputCharacter();
                while (!tokenizer.IsAtEndOfFile && !firstEndMarkerFound && !secondEndMarkerFound && currentChar != HtmlCharacterUtilities.GreaterThanSign)
                {
                    if (currentChar == HtmlCharacterUtilities.RightSquareBracket)
                    {
                        if (firstEndMarkerFound)
                        {
                            secondEndMarkerFound = true;
                        }
                        else
                        {
                            firstEndMarkerFound = true;
                        }
                    }
                    else if (currentChar == HtmlCharacterUtilities.GreaterThanSign)
                    {
                        if (!secondEndMarkerFound)
                        {
                            firstEndMarkerFound = false;
                            tokenizer.EmitToken(new CharacterToken(HtmlCharacterUtilities.GreaterThanSign));
                        }
                    }
                    else
                    {
                        if (firstEndMarkerFound)
                        {
                            tokenizer.EmitToken(new CharacterToken(HtmlCharacterUtilities.RightSquareBracket));
                        }

                        if (secondEndMarkerFound)
                        {
                            tokenizer.EmitToken(new CharacterToken(HtmlCharacterUtilities.RightSquareBracket));
                        }

                        firstEndMarkerFound = false;
                        secondEndMarkerFound = false;
                        tokenizer.EmitToken(new CharacterToken(currentChar));
                    }
                }
            }

            tokenizer.AdvanceState(new DataState());
            return true;
        }
    }
}
