using XBrowserProject.Internal.Html.Interfaces.DOM;

namespace XBrowserProject.Internal.Html.DOM
{
    internal class HtmlQuoteElement : HtmlElement, IHTMLQuoteElement
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
