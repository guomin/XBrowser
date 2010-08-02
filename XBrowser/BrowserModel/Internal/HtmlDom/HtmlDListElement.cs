using XBrowserProject.HtmlDom;

namespace XBrowserProject.BrowserModel.Internal.HtmlDom
{
    internal class HtmlDListElement : HtmlElement, IHTMLDListElement
    {
        public HtmlDListElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

       public bool compact
        {
            get { return HasAttribute(HtmlAttributeNames.CompactAttributeName); }
            set { SetBooleanAttribute(HtmlAttributeNames.CompactAttributeName, value); }
        }
    }
}
