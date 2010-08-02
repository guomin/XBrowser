using XBrowserProject.HtmlDom;

namespace XBrowserProject.BrowserModel.Internal.HtmlDom
{
    internal class HtmlOptGroupElement : HtmlElement, IHTMLOptGroupElement
    {
        public HtmlOptGroupElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        public bool disabled
        {
            get { return HasAttribute(HtmlAttributeNames.DisabledAttributeName); }
            set { SetBooleanAttribute(HtmlAttributeNames.DisabledAttributeName, value); }
        }

        public string label
        {
            get { return GetAttribute(HtmlAttributeNames.LabelAttributeName); }
            set { SetAttribute(HtmlAttributeNames.LabelAttributeName, value); }
        }
    }
}
