namespace XBrowserProject.HtmlParser.HtmlTokenizer
{
    internal class DocTypeToken : Token
    {
        private string tokenName = string.Empty;
        private string tokenSystemId;
        private string tokenPublicId;
        private bool tokenQuirksMode = false;

        public DocTypeToken()
        {
        }

        public string Name
        {
            get { return tokenName; }
            set { tokenName = value; }
        }

        public string SystemId
        {
            get { return tokenSystemId; }
            set { tokenSystemId = value; }
        }

        public string PublicId
        {
            get { return tokenPublicId; }
            set { tokenPublicId = value; }
        }

        public bool QuirksMode
        {
            get { return tokenQuirksMode; }
            set { tokenQuirksMode = value; }
        }

        public override TokenType TokenType
        {
            get { return TokenType.DocType; }
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", TokenType, Name);
        }
    }
}
