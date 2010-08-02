namespace XBrowserProject.HtmlParser.HtmlTokenizer
{
    internal class EndOfFileToken : Token
    {
        public override TokenType TokenType
        {
            get { return TokenType.EndOfFile; }
        }

        public override string ToString()
        {
            return TokenType.ToString();
        }
    }
}
