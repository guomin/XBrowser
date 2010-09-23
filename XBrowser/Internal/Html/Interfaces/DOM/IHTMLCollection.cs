using System.Xml;

namespace XBrowserProject.Internal.Html.Interfaces.DOM
{
    internal interface IHTMLCollection
    {
        /// <summary>
        /// This attribute specifies the length or size of the list.
        /// </summary>
        int length
        {
            get;
        }

        /// <summary>
        /// This method retrieves a node specified by ordinal index. Nodes are numbered in tree order (depth-first traversal order).
        /// </summary>
        /// <param name="index">The index of the node to be fetched. The index origin is 0.</param>
        /// <returns>The Node at the corresponding position upon success. A value of null is returned if the index is out of range.</returns>
        XmlNode this[int index]
        {
            get;
        }

        /// <summary>
        /// This method retrieves a Node using a name. It first searches for a Node with a matching id attribute. If it doesn't find one, it then searches for a Node with a matching name attribute, but only on those elements that are allowed a name attribute.
        /// </summary>
        /// <param name="name">The name of the Node to be fetched.</param>
        /// <returns>The Node with a name or id attribute whose value corresponds to the specified string. Upon failure (newNode.g., no node with this name exists), returns null.</returns>
        XmlNode this[string name]
        {
            get;
        }

        XmlNode item(int index);
        XmlNode namedItem(string name);
    }
}
