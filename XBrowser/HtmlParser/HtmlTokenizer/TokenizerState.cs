namespace XBrowserProject.HtmlParser.HtmlTokenizer
{
    public abstract class TokenizerState
    {
        public abstract bool ParseTokenFromDataStream(Tokenizer tokenizer);
    }
}
