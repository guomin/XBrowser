using System;
using System.Collections.Generic;
using System.Text;

namespace XBrowserProject.HtmlDom
{
    public interface IHTMLDocument : IDocument
    {
        IHTMLCollection this[string name] { get; }

        /// <summary>
        /// The title of a document as specified by the TITLE element in the head of the document.
        /// </summary>
        string title
        {
            get;
            set;
        }

        /// <summary>
        /// Returns the URI of the page that linked to this page. The value is an empty string if the user navigated to the page directly (not through a link, but, for example, via a bookmark).
        /// </summary>
        string referrer
        {
            get;
        }

        /// <summary>
        /// The domain name of the server that served the document, or a null string if the server cannot be identified by a domain name.
        /// </summary>
        string domain
        {
            get;
        }

        /// <summary>
        /// The complete URI of the document.
        /// </summary>
        string URL
        {
            get;
        }

        /// <summary>
        /// The element that contains the content for the document. In documents with BODY contents, returns the BODY element, and in frameset documents, this returns the outermost FRAMESET element.
        /// </summary>
        IHTMLElement body
        {
            get;
            set;
        }

        /// <summary>
        /// A collection of all the IMG elements in a document. The behavior is limited to IMG elements for backwards compatibility.
        /// </summary>
        IHTMLCollection images
        {
            get;
        }

        /// <summary>
        /// A collection of all the OBJECT elements that include applets and APPLET (deprecated) elements in a document.
        /// </summary>
        IHTMLCollection applets
        {
            get;
        }

        /// <summary>
        /// A collection of all AREA elements and anchor (A) elements in a document with a value for the href attribute.
        /// </summary>
        IHTMLCollection links
        {
            get;
        }

        /// <summary>
        /// A collection of all the forms of a document.
        /// </summary>
        IHTMLCollection forms
        {
            get;
        }

        /// <summary>
        /// A collection of all the anchor (A) elements in a document with a value for the name attribute.Note. For reasons of backwards compatibility, the returned set of anchors only contains those anchors created with the name attribute, not those created with the id attribute.
        /// </summary>
        IHTMLCollection anchors
        {
            get;
        }

        /// <summary>
        /// The cookies associated with this document. If there are none, the value is an empty string. Otherwise, the value is a string: a semicolon-delimited list of "name, value" pairs for all the cookies associated with the page. For example, name=value;expires=date.
        /// </summary>
        string cookie
        {
            get;
            set;
        }

        /// <summary>
        /// Open a document stream for writing. If a document exists in the target, this method clears it.
        /// </summary>
        void open();

        /// <summary>
        /// Closes a document stream opened by open() and forces rendering.
        /// </summary>
        void close();

        /// <summary>
        /// Write a string of text to a document stream opened by open(). The text is parsed into the document'coreRule structure model.
        /// </summary>
        /// <param name="text">The string to be parsed into some structure in the document structure model.</param>
        void write(string text);

        /// <summary>
        /// Write a string of text followed by a newline character to a document stream opened by open(). The text is parsed into the document'coreRule structure model.
        /// </summary>
        /// <param name="text">The string to be parsed into some structure in the document structure model.</param>
        void writeln(string text);

        /// <summary>
        /// Returns the (possibly empty) collection of elements whose name value is given by elementName.
        /// </summary>
        /// <param name="elementName">The name attribute value for an element.</param>
        /// <returns>The matching elements.</returns>
        INodeList getElementsByName(string elementName);
    }
}
