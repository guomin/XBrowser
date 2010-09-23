

namespace XBrowserProject.Internal.Html.Interfaces.DOM
{
    internal interface IHTMLFrameElement : IHTMLElement
    {
        string frameBorder { get; set; }
        string longDesc { get; set; }
        string marginHeight { get; set; }
        string marginWidth { get; set; }
        string name { get; set; }
        bool noResize { get; set; }
        string scrolling { get; set; }
        string src { get; set; }
        // Introduced in DOM Level 2:
        IDocument contentDocument { get; }
    }
}
