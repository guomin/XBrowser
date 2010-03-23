using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public abstract class XBrowserElement : XBrowserNode
	{
		protected XBrowserElement(XBrowserDocument doc, XElement node, IEnumerable<string> allowedElementNames) : base(node)
		{
			Document = doc;
			Name = node.Name.LocalName.ToLower();
			NodesInternal = new List<XBrowserNode>();
			AddNodesInternal(allowedElementNames);
		}

		private XBrowserElement(XBrowserDocument doc, XElement node) : base(node)
		{
		}

		private void AddNodesInternal(IEnumerable<string> allowedElementNames)
		{
			bool nonConform = Document.Window.Browser.Config.AllowNonConformingDocumentStructure;
			bool allowText = nonConform || allowedElementNames == null || allowedElementNames.Contains("?");
			foreach(var node in XNode.Nodes())
			{
				switch(node.NodeType)
				{
					case XmlNodeType.Element:
						if(nonConform || allowedElementNames == null || allowedElementNames.Contains(((XElement)node).Name.LocalName.ToLower()))
							NodesInternal.Add(GetElementInternal(Document, (XElement)node));
						break;
					
					case XmlNodeType.Whitespace:
					case XmlNodeType.Text:
						if(allowText)
							NodesInternal.Add(GetTextInternal((XText)node));
						break;
				}
			}
		}

		private XBrowserNode GetTextInternal(XText xText)
		{
			return new XBrowserTextNode(xText);
		}

		private static XBrowserElement GetElementInternal(XBrowserDocument doc, XElement xElement)
		{
			switch(xElement.Name.LocalName.ToLower())
			{
				case "html": return new XBrowserHtmlElement(doc, xElement);
				case "head": return new XBrowserHeadElement(doc, xElement);
				case "title": return new XBrowserTitleElement(doc, xElement);
				case "body": return new XBrowserBodyElement(doc, xElement);
				default: return new XBrowserUnknownHtmlElement(doc, xElement);
			}
		}

		internal static XBrowserElement Create(XBrowserDocument doc, XElement xElement)
		{
			return GetElementInternal(doc, xElement);
		}

		public string Name { get; protected set; }
		public new XElement XNode { get { return (XElement)base.XNode; } }
		public string Text { get { return XNode.Value; } }

		protected IEnumerable<XElement> FilterChildElements(IEnumerable<string> allowedElementNames)
		{
			return XNode.Nodes()
				.Where(n => n.NodeType == XmlNodeType.Element && allowedElementNames.Contains(((XElement)n).Name.LocalName.ToLower()))
				.Cast<XElement>();
		}

		private List<XBrowserNode> NodesInternal { get; set; }
	}
}