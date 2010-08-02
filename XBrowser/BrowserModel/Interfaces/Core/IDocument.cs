using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace XBrowserProject.HtmlDom
{
    internal interface IDocument : INode
    {
        /// <summary>
        /// The Document Type Declaration (see DocumentType) associated with this document. For HTML documents as well as XML documents without a document type declaration this returns null. The DOM Level 1 does not support editing the Document Type Declaration, therefore docType cannot be altered in any way.
        /// </summary>
        XmlDocumentType doctype
        {
            get;
        }

        /// <summary>
        /// The DOMImplementation object that handles this document. A DOM application may use objects from multiple implementations.
        /// </summary>
        XmlImplementation implementation
        {
            get;
        }

        /// <summary>
        /// This is a convenience attribute that allows direct access to the child node that is the root element of the document. For HTML documents, this is the element with the tagName "HTML".
        /// </summary>
        XmlElement documentElement
        {
            get;
        }

        /// <summary>
        /// Creates an element of the type specified. Note that the instance returned implements the Element interface, so attributes can be specified directly on the returned object.
        /// </summary>
        /// <param name="tagName">The name of the element type to instantiate. For XML, this is case-sensitive. For HTML, the tagName parameter may be provided in any case, but it must be mapped to the canonical uppercase form by the DOM implementation. </param>
        /// <returns>A new Element object.</returns>
        XmlElement createElement(string tagName);
        //raises(DOMException);

        /// <summary>
        /// Creates an empty DocumentFragment object.
        /// </summary>
        /// <returns>A new DocumentFragment.</returns>
        XmlDocumentFragment createDocumentFragment();

        /// <summary>
        /// Creates a Text node given the specified string.
        /// </summary>
        /// <param name="data">The data for the node.</param>
        /// <returns>The new Text object.</returns>
        XmlText createTextNode(string data);

        /// <summary>
        /// Creates a Comment node given the specified string.
        /// </summary>
        /// <param name="data">The data for the node.</param>
        /// <returns>The new Comment object.</returns>
        XmlComment createComment(string data);

        /// <summary>
        /// Creates a CDATASection node whose value is the specified string.
        /// </summary>
        /// <param name="data">The data for the CDATASection contents.</param>
        /// <returns>The new CDATASection object.</returns>
        XmlCDataSection createCDATASection(string data);
        //raises(DOMException);

        /// <summary>
        /// Creates a ProcessingInstruction node given the specified name and data strings.
        /// </summary>
        /// <param name="target">The target part of the processing instruction.</param>
        /// <param name="data">The data for the node.</param>
        /// <returns>The new ProcessingInstruction object.</returns>
        XmlProcessingInstruction createProcessingInstruction(string target, string data);
        //raises(DOMException);

        /// <summary>
        /// Creates an Attr of the given name. Note that the Attr instance can then be set on an Element using the setAttribute method.
        /// </summary>
        /// <param name="name">The name of the attribute.</param>
        /// <returns>A new Attr object.</returns>
        XmlAttribute createAttribute(string name);
        //raises(DOMException);

        /// <summary>
        /// Creates an EntityReference object.
        /// </summary>
        /// <param name="name">The name of the entity to reference.</param>
        /// <returns>The new EntityReference object.</returns>
        XmlEntityReference createEntityReference(string name);
        //raises(DOMException);

        /// <summary>
        /// Returns a NodeList of all the Elements with a given tag name in the order in which they would be encountered in a preorder traversal of the Document tree.
        /// </summary>
        /// <param name="tagname">The name of the tag to match on. The special value "*" matches all tags.</param>
        /// <returns>A new NodeList object containing all the matched Elements.</returns>
        XmlNodeList getElementsByTagName(string tagname);

        // Introduced in DOM Level 2:
        XmlNode importNode(XmlNode importedNode, bool deep);
        //raises(DOMException);

        // Introduced in DOM Level 2:
        XmlElement createElementNS(string namespaceURI, string qualifiedName);
        //raises(DOMException);
        
        // Introduced in DOM Level 2:
        XmlAttribute createAttributeNS(string namespaceURI, string qualifiedName);
        //raises(DOMException);
        
        // Introduced in DOM Level 2:
        XmlNodeList getElementsByTagNameNS(string namespaceURI, string localName);

        /// <summary>
        /// Returns the Element whose id is given by elementId. If no such element exists, returns null. Behavior is not defined if more than one element has this id.
        /// </summary>
        /// <param name="elementId">The unique id value for an element.</param>
        /// <returns>The matching element.</returns>
        XmlElement getElementById(string elementId);
    }
}
