using XBrowserProject.HtmlDom;

namespace XBrowserProject.BrowserModel.Internal.HtmlDom
{
    public class HtmlTextAreaElement : HtmlElement, IHTMLTextAreaElement, IFormChild

    {
        public HtmlTextAreaElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        private HtmlFormElement _form;
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

        public string defaultValue
        {
            get { return _defaultValue; }
            set { _defaultValue = value; }
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

        public bool readOnly
        {
            get { return HasAttribute(HtmlAttributeNames.ReadOnlyAttributeName); }
            set { SetBooleanAttribute(HtmlAttributeNames.ReadOnlyAttributeName, value); }
        }

        public string type
        {
            get { return "textarea"; }
        }

        public int rows
        {
            get { return int.Parse(GetAttribute(HtmlAttributeNames.RowsAttributeName)); }
            set { SetAttribute(HtmlAttributeNames.RowsAttributeName, value.ToString()); }
        }

        public int cols
        {
            get { return int.Parse(GetAttribute(HtmlAttributeNames.ColsAttributeName)); }
            set { SetAttribute(HtmlAttributeNames.ColsAttributeName, value.ToString()); }
        }

        public int tabIndex
        {
            get { return int.Parse(GetAttribute(HtmlAttributeNames.TabIndexAttributeName)); }
            set { SetAttribute(HtmlAttributeNames.TabIndexAttributeName, value.ToString()); }
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

        //         attribute DOMString       defaultValue;
        //readonly attribute HTMLFormElement form;
        //         attribute DOMString       accessKey;
        //         attribute long            cols;
        //         attribute boolean         disabled;
        //         attribute DOMString       name;
        //         attribute boolean         readOnly;
        //         attribute long            rows;
        //         attribute long            tabIndex;
        //readonly attribute DOMString       type;
        //         attribute DOMString       value;
        //void               blur();
        //void               focus();
        //void               select();

        #region IFormChild Members

        public void SetForm(HtmlFormElement form)
        {
            _form = form;
        }

        #endregion
    }
}
