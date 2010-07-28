using XBrowserProject.HtmlDom;

namespace XBrowserProject.BrowserModel.Internal.HtmlDom
{
    public class HtmlIsIndexElement : HtmlElement, IHTMLIsIndexElement, IFormChild
    {
        public HtmlIsIndexElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        private HtmlFormElement _form;

        public IHTMLFormElement form
        {
            get { return _form; }
        }

        public string prompt
        {
            get { return GetAttribute(HtmlAttributeNames.PromptAttributeName); }
            set { SetAttribute(HtmlAttributeNames.PromptAttributeName, value); }
        }

        //readonly attribute HTMLFormElement form;
        //attribute DOMString       prompt;

        #region IFormChild Members

        public void SetForm(HtmlFormElement form)
        {
            _form = form;
        }

        #endregion
    }
}
