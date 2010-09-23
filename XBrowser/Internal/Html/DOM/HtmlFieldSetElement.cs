using XBrowserProject.Internal.Html.Interfaces;
using XBrowserProject.Internal.Html.Interfaces.DOM;

namespace XBrowserProject.Internal.Html.DOM
{
    internal class HtmlFieldSetElement : HtmlElement, IHTMLFieldsetElement, IFormChild
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
