namespace XBrowserProject.Internal.Html.DOM.Parsing.HtmlTokenizer.States
{
    internal class ScriptDataDoubleEscapedLessThanSignState : TokenizerState
    {
        public override bool ParseTokenFromDataStream(Tokenizer tokenizer)
        {
            bool tokenEmitted = false;
            if (tokenizer.IsAtEndOfFile)
            {
                tokenizer.AdvanceState(new ScriptDataDoubleEscapedState());
            }
            else
            {
                char currentChar = tokenizer.ConsumeNextInputCharacter();
                if (currentChar == HtmlCharacterUtilities.Solidus)
                {
                    tokenizer.EmitToken(new CharacterToken(currentChar));
                    tokenizer.TemporaryBuffer = string.Empty;
                    tokenizer.AdvanceState(new ScriptDataDoubleEscapeEndState());
                    tokenEmitted = true;
                }
                else
                {
                    tokenizer.ReconsumeInputCharacterInNextState();
                    tokenizer.AdvanceState(new ScriptDataDoubleEscapedState());
                }
            }
            return tokenEmitted;
        }
    }
}
