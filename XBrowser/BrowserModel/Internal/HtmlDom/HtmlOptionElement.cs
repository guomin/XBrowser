using XBrowserProject.HtmlDom;

namespace XBrowserProject.BrowserModel.Internal.HtmlDom
{
    public class HtmlOptionElement : HtmlElement, IHTMLOptionElement, IFormChild
    {
        private HtmlFormElement _form;
        private bool _isSelected;
        private int _index = -1;

        public HtmlOptionElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
            //_index = index;
            _isSelected = selected;
        }

        public IHTMLFormElement form
        {
            get { return _form; }
        }

        public bool disabled
        {
            get { return HasAttribute(HtmlAttributeNames.DisabledAttributeName); }
            set { SetBooleanAttribute(HtmlAttributeNames.DisabledAttributeName, value); }
        }

        public bool defaultSelected
        {
            get { return HasAttribute(HtmlAttributeNames.SelectedAttributeName); }
            set { SetBooleanAttribute(HtmlAttributeNames.SelectedAttributeName, value); }
        }

        public bool selected
        {
            get { return _isSelected; }
            set { _isSelected = value; }
        }

        public string value
        {
            get { return GetAttribute(HtmlAttributeNames.ValueAttributeName); }
            set { SetAttribute(HtmlAttributeNames.ValueAttributeName, value); }
        }

        public string label
        {
            get { return GetAttribute(HtmlAttributeNames.LabelAttributeName); }
            set { SetAttribute(HtmlAttributeNames.LabelAttributeName, value); }
        }

        public int index
        {
            get { return _index; }
        }

        public string text
        {
            get { return FirstChild.InnerText; }
        }

        //readonly attribute HTMLFormElement form;
        //// Modified in DOM Level 2:
        //         attribute boolean         defaultSelected;
        //readonly attribute DOMString       text;
        //// Modified in DOM Level 2:
        //readonly attribute long            index;
        //         attribute boolean         disabled;
        //         attribute DOMString       label;
        //         attribute boolean         selected;
        //         attribute DOMString       value;

        #region IFormChild Members

        public void SetForm(HtmlFormElement form)
        {
            _form = form;
        }

        #endregion
    }
}
