using XBrowserProject.HtmlDom;

namespace XBrowserProject.BrowserModel.Internal.HtmlDom
{
    internal class HtmlBaseElement : HtmlElement, IHTMLBaseElement
    {
        public HtmlBaseElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        public string href
        {
            get { return GetAttribute(HtmlAttributeNames.HrefAttributeName); }
            set { SetAttribute(HtmlAttributeNames.HrefAttributeName, value); }
        }

        public string target
        {
            get { return GetAttribute(HtmlAttributeNames.TargetAttributeName); }
            set { SetAttribute(HtmlAttributeNames.TargetAttributeName, value); }
        }
    }
}
