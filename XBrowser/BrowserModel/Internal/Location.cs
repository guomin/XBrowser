using System;
using XBrowserProject.HtmlDom;

namespace XBrowserProject.BrowserModel.Internal
{
    internal class Location : IHTMLLocation
    {
        Uri internalLocation;

        #region IHTMLLocation Members

        public string href
        {
            get { return internalLocation.ToString(); }
            set { throw new NotImplementedException(); }
        }

        public void assign(string url)
        {
            throw new NotImplementedException();
        }

        public void replace(string url)
        {
            throw new NotImplementedException();
        }

        public void reload()
        {
            throw new NotImplementedException();
        }

        public string resolveURL(string url)
        {
            throw new NotImplementedException();
        }

        public string protocol
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string host
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string hostname
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string port
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string pathname
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string search
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string hash
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
