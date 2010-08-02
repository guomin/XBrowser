using System.Xml;
using XBrowserProject.HtmlDom;

namespace XBrowserProject.BrowserModel.Internal.HtmlDom
{
    internal class HtmlCollection : IHTMLCollection
    {
        private XmlNodeList _internalCollection;

        public HtmlCollection(XmlNodeList internalList)
        {
            _internalCollection = internalList;
        }

        /**
         * An <code>HTMLCollection</code> is a list of nodes. An individual node may 
         * be accessed by either ordinal index or the node's <code>name</code> or 
         * <code>id</code> attributes. Collections in the HTML DOM are assumed to be 
         * live meaning that they are automatically updated when the underlying 
         * document is changed. 
         * <p>See also the <a href='http://www.w3.org/TR/2003/REC-DOM-Level-2-HTML-20030109'>Document Object Model (DOM) Level 2 HTML Specification</a>.
         */

        /**
         * This attribute specifies the length or size of the list. 
         */
        public int length 
        {
            get { return _internalCollection.Count; }
        }

        /**
         * This method retrieves a node specified by ordinal index. Nodes are 
         * numbered in tree order (depth-first traversal order).
         * @param index The index of the node to be fetched. The index origin is 
         *   <code>0</code>.
         * @return The <code>Node</code> at the corresponding position upon 
         *   success. A value of <code>null</code> is returned if the index is 
         *   out of range. 
         */
        public XmlNode item(int index)
        {
            return _internalCollection[index];
        }

        /**
         * This method retrieves a <code>Node</code> using a name. With [<a href='http://www.w3.org/TR/1999/REC-html401-19991224'>HTML 4.01</a>] 
         * documents, it first searches for a <code>Node</code> with a matching 
         * <code>id</code> attribute. If it doesn't find one, it then searches 
         * for a <code>Node</code> with a matching <code>name</code> attribute, 
         * but only on those elements that are allowed a name attribute. With [<a href='http://www.w3.org/TR/2002/REC-xhtml1-20020801'>XHTML 1.0</a>] 
         * documents, this method only searches for <code>Nodes</code> with a 
         * matching <code>id</code> attribute. This method is case insensitive 
         * in HTML documents and case sensitive in XHTML documents.
         * @param name The name of the <code>Node</code> to be fetched.
         * @return The <code>Node</code> with a <code>name</code> or 
         *   <code>id</code> attribute whose value corresponds to the specified 
         *   string. Upon failure (e.g., no node with this name exists), returns 
         *   <code>null</code>.
         */
        public XmlNode namedItem(string name)
        {
            return null;
        }

        #region IHTMLCollection Members


        public XmlNode this[int index]
        {
            get { return item(index); }
        }

        public XmlNode this[string name]
        {
            get { return namedItem(name); }
        }

        #endregion
    }
}
