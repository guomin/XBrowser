using XBrowserProject.HtmlDom;

namespace XBrowserProject.BrowserModel.Internal.HtmlDom
{
    public class HtmlMetaElement : HtmlElement, IHTMLMetaElement
    {
        public HtmlMetaElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        public string content
        {
            get { return GetAttribute(HtmlAttributeNames.ContentAttributeName); }
            set { SetAttribute(HtmlAttributeNames.ContentAttributeName, value); }
        }

        public string httpEquiv
        {
            get { return GetAttribute(HtmlAttributeNames.HttpEquivAttributeName); }
            set { SetAttribute(HtmlAttributeNames.HttpEquivAttributeName, value); }
        }

        public string name
        {
            get { return GetAttribute(HtmlAttributeNames.NameAttributeName); }
            set { SetAttribute(HtmlAttributeNames.NameAttributeName, value); }
        }

        public string scheme
        {
            get { return GetAttribute(HtmlAttributeNames.SchemeAttributeName); }
            set { SetAttribute(HtmlAttributeNames.SchemeAttributeName, value); }
        }

        //attribute DOMString       content;
        //attribute DOMString       httpEquiv;
        //attribute DOMString       name;
        //attribute DOMString       scheme;
    }
}
