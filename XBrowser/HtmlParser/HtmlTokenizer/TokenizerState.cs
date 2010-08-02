namespace XBrowserProject.HtmlParser.HtmlTokenizer
{
    internal abstract class TokenizerState
    {
        public abstract bool ParseTokenFromDataStream(Tokenizer tokenizer);
    }
}
