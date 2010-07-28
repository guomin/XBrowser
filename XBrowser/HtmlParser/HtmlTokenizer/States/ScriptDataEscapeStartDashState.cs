namespace XBrowserProject.HtmlParser.HtmlTokenizer.States
{
    internal class ScriptDataEscapeStartDashState : TokenizerState
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
                    tokenizer.AdvanceState(new ScriptDataEscapedDashDashState());
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
