namespace XBrowserProject.Internal.Html.DOM.Parsing.HtmlTokenizer.States
{
    internal class MarkupDeclarationOpenState : TokenizerState
    {
        private const string CommentKeyword = "--";
        private const string DocTypeKeyword = "doctype";
        private const string CDataKeyword = "[CDATA[";

        public override bool ParseTokenFromDataStream(Tokenizer tokenizer)
        {
            if (tokenizer.ConsumeKeyword(CommentKeyword, true))
            {
                tokenizer.AdvanceState(new CommentStartState(new CommentToken(string.Empty)));
            }
            else if (tokenizer.ConsumeKeyword(DocTypeKeyword, true))
            {
                tokenizer.AdvanceState(new DocTypeState());
            }
            else if (CDataIsSupported && tokenizer.ConsumeKeyword(CDataKeyword, false))
            {
                tokenizer.AdvanceState(new CDataSectionState());
            }
            else
            {
                tokenizer.LogParseError("Invalid character in markup after '!'", "Processing as bogus comment");
                tokenizer.AdvanceState(new BogusCommentState()); 
            }

            return false;
        }

        private bool CDataIsSupported
        {
            // TODO: Check the insert state and the top node on the stack.
            get { return true; }
        }
    }
}
