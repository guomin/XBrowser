using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace XBrowserProject.HtmlDom
{
    public interface IElement : INode
    {
        /// <summary>
        /// The name of the element.
        /// </summary>
        string tagName
        {
            get;
        }

        /// <summary>
        /// Retrieves an attribute value by name.
        /// </summary>
        /// <param name="name">The name of the attribute to retrieve.</param>
        /// <returns>The Attr value as a string, or the empty string if that attribute does not have a specified or default value.</returns>
        string getAttribute(string name);

        /// <summary>
        /// Adds a new attribute. If an attribute with that name is already present in the element, its value is changed to be that of the value parameter. This value is a simple string, it is not parsed as it is being set. So any markup (such as syntax to be recognized as an entity reference) is treated as literal text, and needs to be appropriately escaped by the implementation when it is written out. In order to assign an attribute value that contains entity references, the user must create an Attr node plus any Text and EntityReference nodes, build the appropriate subtree, and use setAttributeNode to assign it as the value of an attribute.
        /// </summary>
        /// <param name="name">The name of the attribute to create or alter.</param>
        /// <param name="value">Value to set in string form.</param>
        void setAttribute(string name, string value);
        //raises(DOMException);

        /// <summary>
        /// Removes an attribute by name. If the removed attribute has a default value it is immediately replaced.
        /// </summary>
        /// <param name="name">The name of the attribute to remove.</param>
        void removeAttribute(string name);
        //raises(DOMException);

        /// <summary>
        /// Retrieves an Attr node by name.
        /// </summary>
        /// <param name="name">The name of the attribute to retrieve.</param>
        /// <returns>The Attr node with the specified attribute name or null if there is no such attribute.</returns>
        XmlAttribute getAttributeNode(string name);

        /// <summary>
        /// Adds a new attribute. If an attribute with that name is already present in the element, it is replaced by the new one.The Attr node to add to the attribute list.
        /// </summary>
        /// <param name="newAttr">The Attr node to add to the attribute list.</param>
        /// <returns>If the newAttr attribute replaces an existing attribute with the same name, the previously existing Attr node is returned, otherwise null is returned.</returns>
        XmlAttribute setAttributeNode(XmlAttribute newAttr);
        //raises(DOMException);

        /// <summary>
        /// Removes the specified attribute.
        /// </summary>
        /// <param name="oldAttr">The Attr node to remove from the attribute list. If the removed Attr has a default value it is immediately replaced.</param>
        /// <returns>The Attr node that was removed.</returns>
        XmlAttribute removeAttributeNode(XmlAttribute oldAttr);
        //raises(DOMException);

        /// <summary>
        /// Returns a NodeList of all descendant elements with a given tag name, in the order in which they would be encountered in a preorder traversal of the Element tree.
        /// </summary>
        /// <param name="name">The name of the tag to match on. The special value "*" matches all tags.</param>
        /// <returns>A list of matching Element nodes.</returns>
        XmlNodeList getElementsByTagName(string name);

        // Introduced in DOM Level 2:
        string getAttributeNS(string namespaceURI, string localName);

        // Introduced in DOM Level 2:
        void setAttributeNS(string namespaceURI, string qualifiedName, string value);
        //raises(DOMException);

        // Introduced in DOM Level 2:
        void removeAttributeNS(string namespaceURI, string localName);
        //raises(DOMException);
        // Introduced in DOM Level 2:
        XmlAttribute getAttributeNodeNS(string namespaceURI, string localName);

        // Introduced in DOM Level 2:
        XmlAttribute setAttributeNodeNS(XmlAttribute newAttr);
        //raises(DOMException);
        // Introduced in DOM Level 2:
        XmlNodeList getElementsByTagNameNS(string namespaceURI, string localName);

        // Introduced in DOM Level 2:
        bool hasAttribute(string name);

        // Introduced in DOM Level 2:
        bool hasAttributeNS(string namespaceURI, string localName);
    }
}
