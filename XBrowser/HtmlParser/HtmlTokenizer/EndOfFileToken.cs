namespace XBrowserProject.HtmlParser.HtmlTokenizer
{
    public class EndOfFileToken : Token
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
