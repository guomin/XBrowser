using System;
using System.Xml;
using XBrowserProject.HtmlDom;

namespace XBrowserProject.BrowserModel.Internal.HtmlDom
{
    internal class HtmlTextNode : XmlText, IText
    {
        public HtmlTextNode(string data, HtmlDocument doc)
            : base(data, doc)
        {
        }

        #region IText Members

        public IText splitText(int offset)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ICharacterData Members

        public string data
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

        public int length
        {
            get { throw new NotImplementedException(); }
        }

        public string substringData(int offset, int count)
        {
            throw new NotImplementedException();
        }

        public void appendData(string arg)
        {
            throw new NotImplementedException();
        }

        public void insertData(int offset, string arg)
        {
            throw new NotImplementedException();
        }

        public void deleteData(int offset, int count)
        {
            throw new NotImplementedException();
        }

        public void replaceData(int offset, int count, string arg)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
