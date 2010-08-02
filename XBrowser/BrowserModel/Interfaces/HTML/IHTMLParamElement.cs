

namespace XBrowserProject.HtmlDom
{
    internal interface IHTMLObjectElement : IHTMLElement
    {
        IHTMLFormElement form { get; }
        string code { get; set; }
        string align { get; set; }
        string archive { get; set; }
        string border { get; set; }
        string codeBase { get; set; }
        string codeType { get; set; }
        string data { get; set; }
        bool declare { get; set; }
        string height { get; set; }
        int hspace { get; set; }
        string name { get; set; }
        string standby { get; set; }
        int tabIndex { get; set; }
        string type { get; set; }
        string useMap { get; set; }
        int vspace { get; set; }
        string width { get; set; }
        // Introduced in DOM Level 2:
        IDocument contentDocument { get; }
    }
}
