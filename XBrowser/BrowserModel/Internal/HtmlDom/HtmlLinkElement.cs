using XBrowserProject.HtmlDom;

namespace XBrowserProject.BrowserModel.Internal.HtmlDom
{
    public class HtmlLinkElement : HtmlElement, IHTMLLinkElement
    {
        public HtmlLinkElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        private bool _disabled = false;

        public bool disabled
        {
            get { return _disabled; }
            set { _disabled = value; }
        }

        public string charset
        {
            get { return GetAttribute(HtmlAttributeNames.CharsetAttributeName); }
            set { SetAttribute(HtmlAttributeNames.CharsetAttributeName, value); }
        }

        public string href
        {
            get { return GetAttribute(HtmlAttributeNames.HrefAttributeName); }
            set { SetAttribute(HtmlAttributeNames.HrefAttributeName, value); }
        }

        public string hreflang
        {
            get { return GetAttribute(HtmlAttributeNames.HrefLangAttributeName); }
            set { SetAttribute(HtmlAttributeNames.HrefLangAttributeName, value); }
        }

        public string media
        {
            get { return GetAttribute(HtmlAttributeNames.MediaAttributeName); }
            set { SetAttribute(HtmlAttributeNames.MediaAttributeName, value); }
        }

        public string rel
        {
            get { return GetAttribute(HtmlAttributeNames.RelAttributeName); }
            set { SetAttribute(HtmlAttributeNames.RelAttributeName, value); }
        }

        public string rev
        {
            get { return GetAttribute(HtmlAttributeNames.RevAttributeName); }
            set { SetAttribute(HtmlAttributeNames.RevAttributeName, value); }
        }

        public string target
        {
            get { return GetAttribute(HtmlAttributeNames.TargetAttributeName); }
            set { SetAttribute(HtmlAttributeNames.TargetAttributeName, value); }
        }

        public string type
        {
            get { return GetAttribute(HtmlAttributeNames.TypeAttributeName); }
            set { SetAttribute(HtmlAttributeNames.TypeAttributeName, value); }
        }

        //attribute boolean         disabled;
        //attribute DOMString       charset;
        //attribute DOMString       href;
        //attribute DOMString       hreflang;
        //attribute DOMString       media;
        //attribute DOMString       rel;
        //attribute DOMString       rev;
        //attribute DOMString       target;
        //attribute DOMString       type;
    }
}
