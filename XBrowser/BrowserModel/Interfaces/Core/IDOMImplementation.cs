using System;
using System.Collections.Generic;
using System.Text;

namespace XBrowserProject.HtmlDom
{
    internal interface IDOMImplementation
    {
        /// <summary>
        /// Test if the DOM implementation implements a specific feature.
        /// </summary>
        /// <param name="feature">The package name of the feature to test. In Level 1, the legal values are "HTML" and "XML" (case-insensitive).</param>
        /// <param name="version">This is the version number of the package name to test. In Level 1, this is the string "1.0". If the version is not specified, supporting any version of the feature will cause the method to return true.</param>
        /// <returns>true if the feature is implemented in the specified version, false otherwise.</returns>
        bool hasFeature(string feature, string version);

        // Introduced in DOM Level 2:
        IDocumentType createDocumentType(string qualifiedName, string publicId, string systemId);
        // raises(DOMException);
        
        // Introduced in DOM Level 2:
        IDocument createDocument(string namespaceURI, string qualifiedName, IDocumentType doctype);
        //raises(DOMException);
    }
}
