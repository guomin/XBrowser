using XBrowserProject.HtmlDom;

namespace XBrowserProject.BrowserModel.Internal.HtmlDom
{
    public class HtmlFieldSetElement : HtmlElement, IHTMLFieldsetElement, IFormChild
    {
        public HtmlFieldSetElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        private HtmlFormElement _form;

        #region IHTMLFieldsetElement Members

        public IHTMLFormElement form
        {
            get { return _form; }
        }

        #endregion

        #region IFormChild Members

        public void SetForm(HtmlFormElement form)
        {
            _form = form;
        }

        #endregion
    }
}
