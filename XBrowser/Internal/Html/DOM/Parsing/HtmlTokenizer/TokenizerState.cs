namespace XBrowserProject.Internal.Html.DOM.Parsing.HtmlTokenizer
{
    internal abstract class TokenizerState
    {
        public abstract bool ParseTokenFromDataStream(Tokenizer tokenizer);
    }
}
