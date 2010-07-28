

namespace XBrowserProject.HtmlDom
{
    public interface IHTMLMapElement : IHTMLElement
    {
        IHTMLCollection areas { get; }
        string name { get; set; }
    }
}
