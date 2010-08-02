using XBrowserProject.HtmlDom;

namespace XBrowserProject.BrowserModel.Internal.HtmlDom
{
    internal class HtmlStyleElement : HtmlElement, IHTMLStyleElement
    {
        public HtmlStyleElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        private bool _disabled = false;

        public bool disabled
        {
            get { return _disabled; }
            set { _disabled = value; }
        }

        public string media
        {
            get { return GetAttribute(HtmlAttributeNames.MediaAttributeName); }
            set { SetAttribute(HtmlAttributeNames.MediaAttributeName, value); }
        }

        public string type
        {
            get { return GetAttribute(HtmlAttributeNames.TypeAttributeName); }
            set { SetAttribute(HtmlAttributeNames.TypeAttributeName, value); }
        }

        //attribute boolean         disabled;
        //attribute DOMString       media;
        //attribute DOMString       type;
    }
}
