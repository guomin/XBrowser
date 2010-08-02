

namespace XBrowserProject.HtmlDom
{
    internal interface IHTMLAppletElement : IHTMLElement
    {
        string align { get; set; }
        string alt { get; set; }
        string archive { get; set; }
        string code { get; set; }
        string codeBase { get; set; }
        string height { get; set; }
        // Modified in DOM Level 2:
        int hspace { get; set; }
        string name { get; set; }
        // Modified in DOM Level 2:
        string @object { get; set; }
        // Modified in DOM Level 2:
        int vspace { get; set; }
        string width { get; set; }
    }
}
