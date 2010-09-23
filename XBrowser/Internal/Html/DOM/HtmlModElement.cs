using XBrowserProject.Internal.Html.Interfaces.DOM;

namespace XBrowserProject.Internal.Html.DOM
{
    internal class HtmlModElement : HtmlElement, IHTMLModElement
    {
        public HtmlModElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        public string cite
        {
            get { return GetAttribute(HtmlAttributeNames.CiteAttributeName); }
            set { SetAttribute(HtmlAttributeNames.CiteAttributeName, value); }
        }

        public string dateTime
        {
            get { return GetAttribute(HtmlAttributeNames.DateTimeAttributeName); }
            set { SetAttribute(HtmlAttributeNames.DateTimeAttributeName, value); }
        }

        //attribute DOMString       cite;
        //attribute DOMString       dateTime;
    }
}
