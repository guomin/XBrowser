using XBrowserProject.Internal.Html.Interfaces.DOM;

namespace XBrowserProject.Internal.Html.DOM
{
    internal class HtmlHtmlElement : HtmlElement, IHTMLHtmlElement
    {
        public HtmlHtmlElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        public string version
        {
            get { return GetAttribute(HtmlAttributeNames.VersionAttributeName); }
            set { SetAttribute(HtmlAttributeNames.VersionAttributeName, value); }
        }

        //attribute DOMString       version;
    }
}
