using System.Xml;
using XBrowserProject.HtmlDom;

namespace XBrowserProject.BrowserModel.Internal.HtmlDom
{
	internal class HtmlElement : XmlElement, IHTMLElement
	{
		private bool isFormattingScopeMarker = false;

		public HtmlElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
			: base(prefix, localName, namespaceURI, doc)
		{
		}

		internal bool IsFormattingScopeMarker
		{
			get { return isFormattingScopeMarker; }
			set { isFormattingScopeMarker = value; }
		}

		#region IHTMLElement Members

		public string id
		{
			get { return GetAttribute(HtmlAttributeNames.IdAttributeName); }
			set { SetAttribute(HtmlAttributeNames.IdAttributeName, value); }
		}

		public string title
		{
			get { return GetAttribute(HtmlAttributeNames.TitleAttributeName); }
			set { SetAttribute(HtmlAttributeNames.TitleAttributeName, value); }
		}

		public string lang
		{
			get { return GetAttribute(HtmlAttributeNames.LangAttributeName); }
			set { SetAttribute(HtmlAttributeNames.LangAttributeName, value); }
		}

		public string dir
		{
			get { return GetAttribute(HtmlAttributeNames.DirAttributeName); }
			set { SetAttribute(HtmlAttributeNames.DirAttributeName, value); }
		}

		public string className
		{
			get { return GetAttribute(HtmlAttributeNames.ClassAttributeName); }
			set { SetAttribute(HtmlAttributeNames.ClassAttributeName, value); }
		}

		#endregion

		#region IElement Members

		public string tagName
		{
			get { return Name; }
		}

		public string getAttribute(string name)
		{
			return GetAttribute(name);
		}

		public void setAttribute(string name, string value)
		{
			SetAttribute(name, value);
		}

		public void removeAttribute(string name)
		{
			RemoveAttribute(name);
		}

		public XmlAttribute getAttributeNode(string name)
		{
			return GetAttributeNode(name);
		}

		public XmlAttribute setAttributeNode(XmlAttribute newAttr)
		{
			return SetAttributeNode(newAttr);
		}

		public XmlAttribute removeAttributeNode(XmlAttribute oldAttr)
		{
			return RemoveAttributeNode(oldAttr);
		}

		public XmlNodeList getElementsByTagName(string name)
		{
			return GetElementsByTagName(name);
		}

		public string getAttributeNS(string namespaceURI, string localName)
		{
			return GetAttribute(localName, namespaceURI);
		}

		public void setAttributeNS(string namespaceURI, string qualifiedName, string value)
		{
			SetAttribute(qualifiedName, namespaceURI, value);
		}

		public void removeAttributeNS(string namespaceURI, string localName)
		{
			RemoveAttribute(localName, namespaceURI);
		}

		public XmlAttribute getAttributeNodeNS(string namespaceURI, string localName)
		{
			return GetAttributeNode(localName, namespaceURI);
		}

		public XmlAttribute setAttributeNodeNS(XmlAttribute newAttr)
		{
			return SetAttributeNode(newAttr);
		}

		public XmlNodeList getElementsByTagNameNS(string namespaceURI, string localName)
		{
			return GetElementsByTagName(localName, namespaceURI);
		}

		public bool hasAttribute(string name)
		{
			return HasAttribute(name);
		}

		public bool hasAttributeNS(string namespaceURI, string localName)
		{
			return HasAttribute(localName, namespaceURI);
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
			return true;
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
			get { return HasAttributes; }
		}

		#endregion

		protected void SetBooleanAttribute(string attributeName, bool attributeValue)
		{
			if(attributeValue)
			{
				SetAttribute(attributeName, attributeName);
			}
			else
			{
				RemoveAttribute(attributeName);
			}
		}

		public void Click()
		{
			if(HasAttribute("onclick"))
			{
				string onClickAttribute = GetAttribute("onclick");
				HtmlDocument parentDocument = OwnerDocument as HtmlDocument;
				if(parentDocument != null)
				{
					parentDocument.Window.RunScript(onClickAttribute);
				}
			}
		}
	}
}
