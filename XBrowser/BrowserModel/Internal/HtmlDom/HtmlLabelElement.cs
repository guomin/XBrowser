using XBrowserProject.HtmlDom;

namespace XBrowserProject.BrowserModel.Internal.HtmlDom
{
    internal class HtmlLabelElement : HtmlElement, IHTMLLabelElement, IFormChild
    {
        public HtmlLabelElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        private HtmlFormElement _form;

        public IHTMLFormElement form
        {
            get { return _form; }
        }

        public string accessKey
        {
            get { return GetAttribute(HtmlAttributeNames.AccessKeyAttributeName); }
            set { SetAttribute(HtmlAttributeNames.AccessKeyAttributeName, value); }
        }

        public string htmlFor
        {
            get { return GetAttribute(HtmlAttributeNames.ForAttributeName); }
            set { SetAttribute(HtmlAttributeNames.ForAttributeName, value); }
        }

        //readonly attribute HTMLFormElement form;
        //         attribute DOMString       accessKey;
        //         attribute DOMString       htmlFor;

        #region IFormChild Members

        public void SetForm(HtmlFormElement form)
        {
            _form = form;
        }

        #endregion
    }
}
