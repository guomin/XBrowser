

namespace XBrowserProject.HtmlDom
{
    internal interface IHTMLParamElement : IHTMLElement
    {
        string name { get; set; }
        string type { get; set; }
        string value { get; set; }
        string valueType { get; set; }
    }
}
