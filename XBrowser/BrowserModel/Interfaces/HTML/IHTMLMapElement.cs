

namespace XBrowserProject.HtmlDom
{
    internal interface IHTMLMapElement : IHTMLElement
    {
        IHTMLCollection areas { get; }
        string name { get; set; }
    }
}
