using XBrowserProject.HtmlDom;

namespace XBrowserProject.BrowserModel.Internal.HtmlDom
{
    internal class HtmlInputElement : HtmlElement, IHTMLInputElement, IFormChild, IResettable
    {
        public HtmlInputElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        private HtmlFormElement _form;
        private bool _isChecked;
        private string _defaultValue;

        public IHTMLFormElement form
        {
            get { return _form; }
        }

        public bool disabled
        {
            get { return HasAttribute(HtmlAttributeNames.DisabledAttributeName); }
            set { SetBooleanAttribute(HtmlAttributeNames.DisabledAttributeName, value); }
        }

        public bool defaultChecked
        {
            get { return HasAttribute(HtmlAttributeNames.CheckedAttributeName); }
            set { SetBooleanAttribute(HtmlAttributeNames.CheckedAttributeName, value); }
        }

        public bool readOnly
        {
            get { return HasAttribute(HtmlAttributeNames.ReadOnlyAttributeName); }
            set { SetBooleanAttribute(HtmlAttributeNames.ReadOnlyAttributeName, value); }
        }

        public bool @checked
        {
            get { return _isChecked; }
            set { _isChecked = value; }
        }

        public string accept
        {
            get { return GetAttribute(HtmlAttributeNames.AcceptAttributeName); }
            set { SetAttribute(HtmlAttributeNames.AcceptAttributeName, value); }
        }

        public string accessKey
        {
            get { return GetAttribute(HtmlAttributeNames.AccessKeyAttributeName); }
            set { SetAttribute(HtmlAttributeNames.AccessKeyAttributeName, value); }
        }

        public string align
        {
            get { return GetAttribute(HtmlAttributeNames.AlignAttributeName); }
            set { SetAttribute(HtmlAttributeNames.AlignAttributeName, value); }
        }

        public string alt
        {
            get { return GetAttribute(HtmlAttributeNames.AltAttributeName); }
            set { SetAttribute(HtmlAttributeNames.AltAttributeName, value); }
        }

        public int maxLength
        {
            get { return int.Parse(GetAttribute(HtmlAttributeNames.MaxLengthAttributeName)); }
            set { SetAttribute(HtmlAttributeNames.MaxLengthAttributeName, value.ToString()); }
        }

        public int size
        {
            get { return int.Parse(GetAttribute(HtmlAttributeNames.SizeAttributeName)); }
            set { SetAttribute(HtmlAttributeNames.SizeAttributeName, value.ToString()); }
        }

        public string name
        {
            get { return GetAttribute(HtmlAttributeNames.NameAttributeName); }
            set { SetAttribute(HtmlAttributeNames.NameAttributeName, value); }
        }

        public string useMap
        {
            get { return GetAttribute(HtmlAttributeNames.UseMapAttributeName); }
            set { SetAttribute(HtmlAttributeNames.UseMapAttributeName, value); }
        }

        public string src
        {
            get { return GetAttribute(HtmlAttributeNames.SrcAttributeName); }
            set { SetAttribute(HtmlAttributeNames.SrcAttributeName, value); }
        }

        public string type
        {
            get { return HasAttribute(HtmlAttributeNames.TypeAttributeName) ? GetAttribute(HtmlAttributeNames.TypeAttributeName) : "text"; }
            set { SetAttribute(HtmlAttributeNames.TypeAttributeName, value); }
        }

        public int tabIndex
        {
            get { return int.Parse(GetAttribute(HtmlAttributeNames.TabIndexAttributeName)); }
            set { SetAttribute(HtmlAttributeNames.TabIndexAttributeName, value.ToString()); }
        }

        public string value
        {
            get { return GetAttribute(HtmlAttributeNames.ValueAttributeName); }
            set { SetAttribute(HtmlAttributeNames.ValueAttributeName, value); }
        }

        public string defaultValue
        {
            get { return _defaultValue; }
            set { _defaultValue = value; }
        }

        public void blur()
        {
        }

        public void focus()
        {
        }

        public void select()
        {
        }

        public void click()
        {
        }

        //         attribute DOMString       defaultValue;
        //         attribute boolean         defaultChecked;
        //readonly attribute HTMLFormElement form;
        //         attribute DOMString       accept;
        //         attribute DOMString       accessKey;
        //         attribute DOMString       align;
        //         attribute DOMString       alt;
        //         attribute boolean         checked;
        //         attribute boolean         disabled;
        //         attribute long            maxLength;
        //         attribute DOMString       name;
        //         attribute boolean         readOnly;
        //// Modified in DOM Level 2:
        //         attribute unsigned long   size;
        //         attribute DOMString       src;
        //         attribute long            tabIndex;
        //// Modified in DOM Level 2:
        //         attribute DOMString       type;
        //         attribute DOMString       useMap;
        //         attribute DOMString       value;
        //void               blur();
        //void               focus();
        //void               select();
        //void               click();

        #region IFormChild Members

        public void SetForm(HtmlFormElement form)
        {
            _form = form;
        }

        #endregion

        #region IResettable Members

        public void Reset()
        {
            value = defaultValue;
            @checked = defaultChecked;
        }

        #endregion
    }
}
