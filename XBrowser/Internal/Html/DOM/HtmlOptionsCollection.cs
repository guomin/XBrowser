using System.Collections.Generic;
using System.Xml;
using XBrowserProject.Internal.Html.Interfaces.DOM;

namespace XBrowserProject.Internal.Html.DOM
{
    internal class HtmlOptionsCollection : IHTMLOptionsCollection
    {
        private List<XmlNode> _internalCollection = new List<XmlNode>();

        public int length
        {
            get { return _internalCollection.Count; }
        }

        public XmlNode this[int index]
        {
            get { return item(index); }
        }

        public XmlNode this[string name]
        {
            get { return namedItem(name); }
        }

        public XmlNode item(int index)
        {
            return _internalCollection[index];
        }

        public XmlNode namedItem(string name)
        {
            return null;
        }
    }
}
