using XBrowserProject.Internal.Html.Interfaces.DOM;

namespace XBrowserProject.Internal.Html.DOM
{
    internal class HtmlBRElement : HtmlElement, IHTMLBRElement
    {
        public HtmlBRElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

       public string clear
        {
            get { return GetAttribute(HtmlAttributeNames.ClearAttributeName); }
            set { SetAttribute(HtmlAttributeNames.ClearAttributeName, value); }
        }
    }
}
