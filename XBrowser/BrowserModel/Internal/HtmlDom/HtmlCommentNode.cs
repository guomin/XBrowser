using System.Xml;
using XBrowserProject.HtmlDom;

namespace XBrowserProject.BrowserModel.Internal.HtmlDom
{
    internal class HtmlCommentNode : XmlComment, IComment
    {
        string commentData = string.Empty;

        public HtmlCommentNode(string comment, HtmlDocument doc)
            : base(comment, doc)
        {
            commentData = comment;
        }

        #region ICharacterData Members

        public string data
        {
            get { return commentData; }
            set { commentData = value; }
        }

        public int length
        {
            get { return commentData.Length; }
        }

        public string substringData(int offset, int count)
        {
            return commentData.Substring(offset, count);
        }

        public void appendData(string arg)
        {
            commentData += arg;
        }

        public void insertData(int offset, string arg)
        {
            commentData = commentData.Insert(offset, arg);
        }

        public void deleteData(int offset, int count)
        {
            commentData = commentData.Remove(offset, count);
        }

        public void replaceData(int offset, int count, string arg)
        {
            commentData = commentData.Remove(offset, count);
            commentData = commentData.Insert(offset, arg);
        }

        #endregion
    }
}
