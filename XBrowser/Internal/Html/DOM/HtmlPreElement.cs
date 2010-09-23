using XBrowserProject.Internal.Html.Interfaces.DOM;

namespace XBrowserProject.Internal.Html.DOM
{
    internal class HtmlPreElement : HtmlElement, IHTMLPreElement
    {
        public HtmlPreElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        public int width
        {
            get { return int.Parse(GetAttribute(HtmlAttributeNames.WidthAttributeName)); }
            set { SetAttribute(HtmlAttributeNames.WidthAttributeName, value.ToString()); }
        }
    }
}
