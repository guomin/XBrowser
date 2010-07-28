using XBrowserProject.HtmlDom;

namespace XBrowserProject.BrowserModel.Internal.HtmlDom
{
    public class HtmlQuoteElement : HtmlElement, IHTMLQuoteElement
    {
        public HtmlQuoteElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        public string cite
        {
            get { return GetAttribute(HtmlAttributeNames.CiteAttributeName); }
            set { SetAttribute(HtmlAttributeNames.CiteAttributeName, value); }
        }
    }
}
