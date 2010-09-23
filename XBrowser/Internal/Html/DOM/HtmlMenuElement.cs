using XBrowserProject.Internal.Html.Interfaces.DOM;

namespace XBrowserProject.Internal.Html.DOM
{
    internal class HtmlMenuElement : HtmlElement, IHTMLMenuElement
    {
        public HtmlMenuElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
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
