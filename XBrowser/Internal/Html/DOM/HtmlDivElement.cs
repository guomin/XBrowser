using XBrowserProject.Internal.Html.Interfaces.DOM;

namespace XBrowserProject.Internal.Html.DOM
{
    internal class HtmlDivElement : HtmlElement, IHTMLDivElement
    {
        public HtmlDivElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        public string align
        {
            get { return GetAttribute(HtmlAttributeNames.AlignAttributeName); }
            set { SetAttribute(HtmlAttributeNames.AlignAttributeName, value); }
        }
    }
}
