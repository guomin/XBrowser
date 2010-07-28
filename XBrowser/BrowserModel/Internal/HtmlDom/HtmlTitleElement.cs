using XBrowserProject.HtmlDom;

namespace XBrowserProject.BrowserModel.Internal.HtmlDom
{
    public class HtmlTitleElement : HtmlElement, IHTMLTitleElement
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
