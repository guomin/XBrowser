namespace XBrowserProject.Internal.Html.DOM.Parsing.HtmlTokenizer.States
{
    internal class CommentEndState : TokenizerState
    {
        private CommentToken stateToken;

        public CommentEndState(CommentToken token)
        {
            stateToken = token;
        }

        public override bool ParseTokenFromDataStream(Tokenizer tokenizer)
        {
            bool tokenEmitted = false;
            if (tokenizer.IsAtEndOfFile)
            {
                tokenizer.LogParseError("Unexpected end of input", "Resetting to data state and reconsuming");
                tokenizer.EmitToken(stateToken);
                tokenizer.AdvanceState(new DataState());
            }
            else
            {
                char currentChar = tokenizer.ConsumeNextInputCharacter();
                if (currentChar == HtmlCharacterUtilities.Hyphen)
                {
                    tokenizer.LogParseError("Unexpected '-' character at end of comment", "Added hyphen '-' to comment and continuing");
                    stateToken.Data += HtmlCharacterUtilities.Hyphen;
                    tokenizer.AdvanceState(new CommentEndState(stateToken));
                }
                else if (currentChar == HtmlCharacterUtilities.GreaterThanSign)
                {
                    tokenizer.EmitToken(stateToken);
                    tokenizer.AdvanceState(new DataState());
                    tokenEmitted = true;
                }
                else if (HtmlCharacterUtilities.IsWhiteSpace(currentChar))
                {
                    tokenizer.LogParseError("Unexpected character ('" + currentChar.ToString() + "') at end of comment", "Added '--' to comment and continuing");
                    AppendHyphensToToken(tokenizer);
                    stateToken.Data += currentChar;
                    tokenizer.AdvanceState(new CommentEndSpaceState(stateToken));
                }
                else if (currentChar == HtmlCharacterUtilities.ExclamationMark)
                {
                    tokenizer.LogParseError("Unexpected exclamation point at end of comment", "Continuing");
                    tokenizer.AdvanceState(new CommentEndBangState(stateToken));
                }
                else
                {
                    tokenizer.LogParseError("Unexpected character ('" + currentChar.ToString() + "') at end of comment", "Added '--' to comment and continuing");
                    AppendHyphensToToken(tokenizer);
                    stateToken.Data += currentChar;
                    tokenizer.AdvanceState(new CommentState(stateToken));
                }
            }

            return tokenEmitted;
        }

        private void AppendHyphensToToken(Tokenizer tokenizer)
        {
            stateToken.Data += HtmlCharacterUtilities.Hyphen;
            if (tokenizer.ApplyXmlConformanceRules)
            {
                stateToken.Data += HtmlCharacterUtilities.Space;
            }

            stateToken.Data += HtmlCharacterUtilities.Hyphen;
        }
    }
}
