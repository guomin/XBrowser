using XBrowserProject.HtmlDom;

namespace XBrowserProject.BrowserModel.Internal.HtmlDom
{
    public class HtmlLIElement : HtmlElement, IHTMLLIElement
    {
        public HtmlLIElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        public string type
        {
            get { return GetAttribute(HtmlAttributeNames.TypeAttributeName); }
            set { SetAttribute(HtmlAttributeNames.TypeAttributeName, value); }
        }

        public int value
        {
            get { return int.Parse(GetAttribute(HtmlAttributeNames.ValueAttributeName)); }
            set { SetAttribute(HtmlAttributeNames.ValueAttributeName, value.ToString()); }
        }
        //attribute DOMString       type;
        //attribute long            value;
    }
}
