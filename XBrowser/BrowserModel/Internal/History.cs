using System;
using XBrowserProject.HtmlDom;

namespace XBrowserProject.BrowserModel.Internal
{
    internal class History : IHTMLHistory
    {
        #region IHTMLHistory Members

        public int length
        {
            get { throw new NotImplementedException(); }
        }

        public void go(int delta)
        {
            throw new NotImplementedException();
        }

        public void back()
        {
            throw new NotImplementedException();
        }

        public void forward()
        {
            throw new NotImplementedException();
        }

        public void pushState(object data, string title, string url)
        {
            throw new NotImplementedException();
        }

        public void replaceState(object data, string title, string url)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
