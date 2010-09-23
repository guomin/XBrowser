using XBrowserProject.Internal.Html.Interfaces.DOM;

namespace XBrowserProject.Internal.Html.DOM
{
    internal class HtmlParamElement : HtmlElement, IHTMLParamElement
    {
        public HtmlParamElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        public string name
        {
            get { return GetAttribute(HtmlAttributeNames.NameAttributeName); }
            set { SetAttribute(HtmlAttributeNames.NameAttributeName, value); }
        }

        public string type
        {
            get { return GetAttribute(HtmlAttributeNames.TypeAttributeName); }
            set { SetAttribute(HtmlAttributeNames.TypeAttributeName, value); }
        }

        public string value
        {
            get { return GetAttribute(HtmlAttributeNames.ValueAttributeName); }
            set { SetAttribute(HtmlAttributeNames.ValueAttributeName, value); }
        }

        public string valueType
        {
            get { return GetAttribute(HtmlAttributeNames.ValueTypeAttributeName); }
            set { SetAttribute(HtmlAttributeNames.ValueTypeAttributeName, value); }
        }

        //attribute DOMString       name;
        //attribute DOMString       type;
        //attribute DOMString       value;
        //attribute DOMString       valueType;
    }
}
