using XBrowserProject.Internal.Html.Interfaces.DOM;

namespace XBrowserProject.Internal.Html.DOM
{
    internal class HtmlUListElement : HtmlElement, IHTMLUListElement
    {
        public HtmlUListElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        public bool compact
        {
            get { return HasAttribute(HtmlAttributeNames.CompactAttributeName); }
            set { SetBooleanAttribute(HtmlAttributeNames.CompactAttributeName, value); }
        }

        public string type
        {
            get { return GetAttribute(HtmlAttributeNames.TypeAttributeName); }
            set { SetAttribute(HtmlAttributeNames.TypeAttributeName, value); }
        }
        //attribute boolean         compact;
        //attribute DOMString       type;
    }
}
