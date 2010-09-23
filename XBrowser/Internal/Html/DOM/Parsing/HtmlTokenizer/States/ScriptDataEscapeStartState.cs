namespace XBrowserProject.Internal.Html.DOM.Parsing.HtmlTokenizer.States
{
    internal class ScriptDataEscapeStartState : TokenizerState
    {
        public override bool ParseTokenFromDataStream(Tokenizer tokenizer)
        {
            if (tokenizer.IsAtEndOfFile)
            {
                tokenizer.AdvanceState(new ScriptDataState());
            }
            else
            {
                char currentChar = tokenizer.ConsumeNextInputCharacter();
                if (currentChar == HtmlCharacterUtilities.Hyphen)
                {
                    tokenizer.EmitToken(new CharacterToken(HtmlCharacterUtilities.Hyphen));
                    tokenizer.AdvanceState(new ScriptDataEscapeStartDashState());
                }
                else
                {
                    tokenizer.ReconsumeInputCharacterInNextState();
                    tokenizer.AdvanceState(new ScriptDataState());
                }
            }
            return false;
        }
    }
}
