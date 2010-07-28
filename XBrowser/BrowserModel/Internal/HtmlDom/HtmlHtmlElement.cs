using XBrowserProject.HtmlDom;

namespace XBrowserProject.BrowserModel.Internal.HtmlDom
{
    public class HtmlHtmlElement : HtmlElement, IHTMLHtmlElement
    {
        public HtmlHtmlElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        public string version
        {
            get { return GetAttribute(HtmlAttributeNames.VersionAttributeName); }
            set { SetAttribute(HtmlAttributeNames.VersionAttributeName, value); }
        }

        //attribute DOMString       version;
    }
}
