using XBrowserProject.Internal.Html.Interfaces.DOM;

namespace XBrowserProject.Internal.Html.DOM
{
    internal class HtmlHeadElement : HtmlElement, IHTMLHeadElement
    {
        public HtmlHeadElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        public string profile
        {
            get { return GetAttribute(HtmlAttributeNames.ProfileAttributeName); }
            set { SetAttribute(HtmlAttributeNames.ProfileAttributeName, value); }
        }

        //attribute DOMString       profile;
    }
}
