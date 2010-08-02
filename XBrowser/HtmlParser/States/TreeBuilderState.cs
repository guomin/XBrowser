namespace XBrowserProject.HtmlParser.States
{
    internal abstract class TreeBuilderState
    {
        public abstract void ParseToken(Parser parser);
    }
}
