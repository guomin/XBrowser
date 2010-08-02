namespace XBrowserProject.HtmlParser.HtmlTokenizer
{
    internal class CharacterToken : Token
    {
        private char tokenData;

        public CharacterToken(char data)
        {
            tokenData = data;
        }

        public char Data
        {
            get { return tokenData; }
        }

        public override TokenType TokenType
        {
            get { return TokenType.Character; }
        }

        public override string ToString()
        {
            return string.Format("{0}: '{1}'", TokenType, Data);
        }
    }
}
