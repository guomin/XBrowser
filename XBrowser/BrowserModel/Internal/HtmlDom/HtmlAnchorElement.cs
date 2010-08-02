using XBrowserProject.HtmlDom;

namespace XBrowserProject.BrowserModel.Internal.HtmlDom
{
    internal class HtmlAnchorElement : HtmlElement, IHTMLAnchorElement
    {
        public HtmlAnchorElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        #region IHTMLAnchorElement Members
        public string coords
        {
            get { return GetAttribute(HtmlAttributeNames.CoordsAttributeName); }
            set { SetAttribute(HtmlAttributeNames.CoordsAttributeName, value); }
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

        public string target
        {
            get { return GetAttribute(HtmlAttributeNames.TargetAttributeName); }
            set { SetAttribute(HtmlAttributeNames.TargetAttributeName, value); }
        }

        public int tabIndex
        {
            get { return int.Parse(GetAttribute(HtmlAttributeNames.TabIndexAttributeName)); }
            set { SetAttribute(HtmlAttributeNames.TabIndexAttributeName, value.ToString()); }
        }

        public string accessKey
        {
            get { return GetAttribute(HtmlAttributeNames.AccessKeyAttributeName); }
            set { SetAttribute(HtmlAttributeNames.AccessKeyAttributeName, value); }
        }

        public string name
        {
            get { return GetAttribute(HtmlAttributeNames.NameAttributeName); }
            set { SetAttribute(HtmlAttributeNames.NameAttributeName, value); }
        }

        public string hreflang
        {
            get { return GetAttribute(HtmlAttributeNames.HrefLangAttributeName); }
            set { SetAttribute(HtmlAttributeNames.HrefLangAttributeName, value); }
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

        public string shape
        {
            get { return GetAttribute(HtmlAttributeNames.ShapeAttributeName); }
            set { SetAttribute(HtmlAttributeNames.ShapeAttributeName, value); }
        }

        public string type
        {
            get { return GetAttribute(HtmlAttributeNames.TypeAttributeName); }
            set { SetAttribute(HtmlAttributeNames.TypeAttributeName, value); }
        }

        public void blur()
        {
        }

        public void focus()
        {
        }
        #endregion
    }
}
