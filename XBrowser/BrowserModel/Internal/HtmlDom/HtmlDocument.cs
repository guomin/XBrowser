using System;
using System.Xml;
using XBrowserProject.HtmlDom;

namespace XBrowserProject.BrowserModel.Internal.HtmlDom
{
    internal class HtmlDocument : XmlDocument, IHTMLDocument
    {
        private string _title = string.Empty;
        private string _referrer = string.Empty;
        private string _domain = string.Empty;
        private string _url = string.Empty;
        private string _cookie;
        private Window _window;

        public HtmlDocument()
        {
        }

        public HtmlDocument(Window hostWindow)
            : this()
        {
            _window = hostWindow;
        }

        public Window Window
        {
            get { return _window; }
            set { _window = value; }
        }

        public override XmlElement CreateElement(string prefix, string localName, string namespaceURI)
        {
            return HtmlElementFactory.CreateElement(prefix, localName, namespaceURI, this);
        }

        public override XmlComment CreateComment(string data)
        {
            return new HtmlCommentNode(data, this);
        }

        public override XmlText CreateTextNode(string text)
        {
            return new HtmlTextNode(text, this);
        }

        //         attribute DOMString       title;
        //readonly attribute DOMString       referrer;
        //readonly attribute DOMString       domain;
        //readonly attribute DOMString       URL;
        //         attribute HTMLElement     body;
        //readonly attribute HTMLCollection  images;
        //readonly attribute HTMLCollection  applets;
        //readonly attribute HTMLCollection  links;
        //readonly attribute HTMLCollection  forms;
        //readonly attribute HTMLCollection  anchors;
        //         attribute DOMString       cookie;
        //                                    // raises(dom::DOMException) on setting

        //void               open();
        //void               close();
        //void               write(in DOMString text);
        //void               writeln(in DOMString text);
        //NodeList           getElementsByName(in DOMString elementName);

        #region IHTMLDocument Members

        public IHTMLCollection this[string name]
        {
            get
            {
                return new HtmlCollection(GetNamedElements(name));
            }
        }

        private XmlNodeList GetNamedElements(string name)
        {
            string elementSelectionXpath = string.Format("//applet[@id='{0}' or @name='{0}']|//object[@id='{0}' or @name='{0}']|//embed[@name='{0}']|//form[@name='{0}']|//iframe[@name='{0}']|//img[@name='{0}' or @id='{0}' and @name]", name);
            return SelectNodes(elementSelectionXpath);
        }

        public string title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string referrer
        {
            get { return _referrer; }
        }

        public string domain
        {
            get { return _domain; }
        }

        public string URL
        {
            get { return _url; }
        }

        /// <summary>
        /// The element that contains the content for the document. In documents with BODY contents, 
        /// returns the BODY element. In frameset documents, this returns the outermost FRAMESET element.
        /// </summary>
        public IHTMLElement body
        {
            // TODO: process framesets and return proper element.
            //get { return SelectSingleNode("//h:" + HtmlElementFactory.BodyElementTagName, _namespaceManager) as HtmlBodyElement;  }
            get { return SelectSingleNode("//" + HtmlElementFactory.BodyElementTagName) as HtmlBodyElement;  }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// A collection of all the IMG elements in a document. The behavior is limited to IMG 
        /// elements for backwards compatibility. 
        /// </summary>
        public IHTMLCollection images
        {
            get { return new HtmlCollection(SelectNodes("//" + HtmlElementFactory.ImgElementTagName)); }
        }

        /// <summary>
        /// A collection of all the OBJECT elements that include applets and APPLET (deprecated) elements in a document.
        /// </summary>
        public IHTMLCollection applets
        {
            // TODO: include object elements that contain applets (how to determine if applet?)
            get { return new HtmlCollection(SelectNodes("//" + HtmlElementFactory.AppletElementTagName)); }
        }

        /// <summary>
        /// A collection of all AREA elements and anchor (A) elements in a document with a value for the href attribute.
        /// </summary>
        public IHTMLCollection links
        {
            // TODO: Include A elements and to limit to those with href element values.
            get { return new HtmlCollection(SelectNodes("//" + HtmlElementFactory.AreaElementTagName)); }
        }

        /// <summary>
        /// A collection of all the forms of a document.
        /// </summary>
        public IHTMLCollection forms
        {
            get { return new HtmlCollection(SelectNodes("//" + HtmlElementFactory.FormElementTagName)); }
        }

        /// <summary>
        /// A collection of all the anchor (A) elements in a document with a value for the name attribute.
        /// </summary>
        public IHTMLCollection anchors
        {
            // TODO: Limit returned collection to those with "name" attribute set.
            get { return new HtmlCollection(SelectNodes("//" + HtmlElementFactory.AnchorElementTagName)); }
        }

        public string cookie
        {
            get { return _cookie; }
            set { _cookie = value; }
        }

        public void open()
        {
            throw new NotImplementedException();
        }

        public void close()
        {
            throw new NotImplementedException();
        }

        public void write(string text)
        {
            throw new NotImplementedException();
        }

        public void writeln(string text)
        {
            throw new NotImplementedException();
        }

        public INodeList getElementsByName(string elementName)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDocument Members

        public XmlDocumentType doctype
        {
            get { return DocumentType; }
        }

        public XmlImplementation implementation
        {
            get { return Implementation; }
        }

        public XmlElement documentElement
        {
            get { return DocumentElement; }
        }

        public XmlElement createElement(string tagName)
        {
            return CreateElement(string.Empty, tagName, string.Empty);
        }

        public XmlDocumentFragment createDocumentFragment()
        {
            throw new NotImplementedException();
        }

        public XmlText createTextNode(string data)
        {
            return CreateTextNode(data);
        }

        public XmlComment createComment(string data)
        {
            return CreateComment(data);
        }

        public XmlCDataSection createCDATASection(string data)
        {
            throw new NotImplementedException();
        }

        public XmlProcessingInstruction createProcessingInstruction(string target, string data)
        {
            throw new NotImplementedException();
        }

        public XmlAttribute createAttribute(string name)
        {
            return CreateAttribute(name);
        }

        public XmlEntityReference createEntityReference(string name)
        {
            throw new NotImplementedException();
        }

        public XmlNodeList getElementsByTagName(string tagname)
        {
            return GetElementsByTagName(tagname);
        }

        public XmlNode importNode(XmlNode importedNode, bool deep)
        {
            return ImportNode(importedNode, deep);
        }

        public XmlElement createElementNS(string namespaceURI, string qualifiedName)
        {
            throw new NotImplementedException();
        }

        public XmlAttribute createAttributeNS(string namespaceURI, string qualifiedName)
        {
            throw new NotImplementedException();
        }

        public XmlNodeList getElementsByTagNameNS(string namespaceURI, string localName)
        {
            return GetElementsByTagName(localName, namespaceURI);
        }

        public XmlElement getElementById(string elementId)
        {
            return GetElementById(elementId);
        }

        #endregion

        #region INode Members

        public string nodeName
        {
            get { return Name; }
        }

        public string nodeValue
        {
            get { return Value; }
            set { Value = value; }
        }

        public XmlNodeType nodeType
        {
            get { return NodeType; }
        }

        public XmlNode parentNode
        {
            get { return ParentNode; }
        }

        public XmlNodeList childNodes
        {
            get { return ChildNodes; }
        }

        public XmlNode firstChild
        {
            get { return FirstChild; }
        }

        public XmlNode lastChild
        {
            get { return LastChild; }
        }

        public XmlNode previousSibling
        {
            get { return PreviousSibling; }
        }

        public XmlNode nextSibling
        {
            get { return NextSibling; }
        }

        public XmlNamedNodeMap attributes
        {
            get { return Attributes; }
        }

        public XmlDocument ownerDocument
        {
            get { return OwnerDocument; }
        }

        public XmlNode insertBefore(XmlNode newChild, XmlNode refChild)
        {
            return InsertBefore(newChild, refChild);
        }

        public XmlNode replaceChild(XmlNode newChild, XmlNode oldChild)
        {
            return ReplaceChild(newChild, oldChild);
        }

        public XmlNode removeChild(XmlNode oldChild)
        {
            return RemoveChild(oldChild);
        }

        public XmlNode appendChild(XmlNode newChild)
        {
            return AppendChild(newChild);
        }

        public bool hasChildNodes
        {
            get { return HasChildNodes; }
        }

        public XmlNode cloneNode(bool deep)
        {
            return CloneNode(deep);
        }

        public void normalize()
        {
            Normalize();
        }

        public bool isSupported(string feature, string version)
        {
            throw new NotImplementedException();
        }

        public string namespaceURI
        {
            get { return NamespaceURI; }
        }

        public string prefix
        {
            get { return Prefix; }
        }

        public string localName
        {
            get { return LocalName; }
        }

        public bool hasAttributes
        {
            get { return Attributes != null && Attributes.Count > 0; }
        }

        #endregion
    }
}
