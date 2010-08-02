

namespace XBrowserProject.HtmlDom
{
    internal interface IHTMLIframeElement : IHTMLElement
    {
        string align { get; set; }
        string height { get; set; }
        string width { get; set; }
        string frameBorder { get; set; }
        string longDesc { get; set; }
        string marginHeight { get; set; }
        string marginWidth { get; set; }
        string name { get; set; }
        string scrolling { get; set; }
        string src { get; set; }
        // Introduced in DOM Level 2:
        IDocument contentDocument { get; }
    }
}
