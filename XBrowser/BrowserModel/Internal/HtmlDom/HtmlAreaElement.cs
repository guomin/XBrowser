using XBrowserProject.HtmlDom;

namespace XBrowserProject.BrowserModel.Internal.HtmlDom
{
    internal class HtmlAreaElement : HtmlElement, IHTMLAreaElement
    {
        public HtmlAreaElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        public string accessKey
        {
            get { return GetAttribute(HtmlAttributeNames.AccessKeyAttributeName); }
            set { SetAttribute(HtmlAttributeNames.AccessKeyAttributeName, value); }
        }

        public string alt
        {
            get { return GetAttribute(HtmlAttributeNames.AltAttributeName); }
            set { SetAttribute(HtmlAttributeNames.AltAttributeName, value); }
        }

        public string coords
        {
            get { return GetAttribute(HtmlAttributeNames.CoordsAttributeName); }
            set { SetAttribute(HtmlAttributeNames.CoordsAttributeName, value); }
        }

        public string href
        {
            get { return GetAttribute(HtmlAttributeNames.HrefAttributeName); }
            set { SetAttribute(HtmlAttributeNames.HrefAttributeName, value); }
        }

        public bool noHref
        {
            get { return HasAttribute(HtmlAttributeNames.NoHrefAttributeName); }
            set { SetBooleanAttribute(HtmlAttributeNames.NoHrefAttributeName, value); }
        }

        public string shape
        {
            get { return GetAttribute(HtmlAttributeNames.ShapeAttributeName); }
            set { SetAttribute(HtmlAttributeNames.ShapeAttributeName, value); }
        }

        public int tabIndex
        {
            get { return int.Parse(GetAttribute(HtmlAttributeNames.TabIndexAttributeName)); }
            set { SetAttribute(HtmlAttributeNames.TabIndexAttributeName, value.ToString()); }
        }

        public string target
        {
            get { return GetAttribute(HtmlAttributeNames.TargetAttributeName); }
            set { SetAttribute(HtmlAttributeNames.TargetAttributeName, value); }
        }
    }
}
