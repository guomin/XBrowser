namespace XBrowserProject.Internal.Html.DOM.Parsing.HtmlTokenizer
{
    internal class CommentToken : Token
    {
        private string tokenData;

        public CommentToken(string data)
        {
            tokenData = data;
        }

        public string Data
        {
            get { return tokenData; }
            set { tokenData = value; }
        }

        public override TokenType TokenType
        {
            get { return TokenType.Comment; }
        }

        public override string ToString()
        {
            return string.Format("{0}: '{1}'", TokenType, Data);
        }
    }
}
