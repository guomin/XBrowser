using XBrowserProject.Internal.Html.Interfaces.DOM;

namespace XBrowserProject.Internal.Html.DOM
{
    internal class HtmlOListElement : HtmlElement, IHTMLOListElement
    {
        public HtmlOListElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        public int start
        {
            get { return int.Parse(GetAttribute(HtmlAttributeNames.StartAttributeName)); }
            set { SetAttribute(HtmlAttributeNames.StartAttributeName, value.ToString()); }
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
        //attribute long            start;
        //attribute DOMString       type;
    }
}
