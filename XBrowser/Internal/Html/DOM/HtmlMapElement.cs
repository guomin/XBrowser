using XBrowserProject.Internal.Html.Interfaces.DOM;

namespace XBrowserProject.Internal.Html.DOM
{
    internal class HtmlMapElement : HtmlElement, IHTMLMapElement
    {
        public HtmlMapElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        HtmlCollection _areas;

        public IHTMLCollection areas
        {
            get { return _areas; }
        }

        public string name
        {
            get { return GetAttribute(HtmlAttributeNames.NameAttributeName); }
            set { SetAttribute(HtmlAttributeNames.NameAttributeName, value); }
        }

        //readonly attribute HTMLCollection  areas;
        // attribute DOMString       name;
    }
}
