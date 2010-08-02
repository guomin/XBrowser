using XBrowserProject.HtmlDom;

namespace XBrowserProject.BrowserModel.Internal.HtmlDom
{
    internal class HtmlButtonElement : HtmlElement, IHTMLButtonElement, IFormChild
    {
        public HtmlButtonElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        private HtmlFormElement _form;

        public IHTMLFormElement form
        {
            get { return _form; }
        }

        public string type
        {
            get { return HasAttribute(HtmlAttributeNames.TypeAttributeName) ? GetAttribute(HtmlAttributeNames.TypeAttributeName) : "button"; }
            set { SetAttribute(HtmlAttributeNames.TypeAttributeName, value); }
        }

        public bool disabled
        {
            get { return HasAttribute(HtmlAttributeNames.DisabledAttributeName); }
            set { SetBooleanAttribute(HtmlAttributeNames.DisabledAttributeName, value); }
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

        public string value
        {
            get { return GetAttribute(HtmlAttributeNames.ValueAttributeName); }
            set { SetAttribute(HtmlAttributeNames.ValueAttributeName, value); }
        }

        //readonly attribute HTMLFormElement form;
        //         attribute DOMString       accessKey;
        //         attribute boolean         disabled;
        //         attribute DOMString       name;
        //         attribute long            tabIndex;
        //readonly attribute DOMString       type;
        //         attribute DOMString       value;

        #region IFormChild Members

        public void SetForm(HtmlFormElement form)
        {
            _form = form;
        }

        #endregion
    }
}
