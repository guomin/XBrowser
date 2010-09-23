using XBrowserProject.Internal.Html.Interfaces.DOM;

namespace XBrowserProject.Internal.Html.DOM
{
    internal class HtmlTitleElement : HtmlElement, IHTMLTitleElement
    {
        public HtmlTitleElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        public string text
        {
            get { return FirstChild.InnerText; }
            set { FirstChild.InnerText = value;}
        }
    }
}
