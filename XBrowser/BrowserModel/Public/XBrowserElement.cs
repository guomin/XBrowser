using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public abstract class XBrowserElement : XBrowserNode
	{

		protected XBrowserElement(XBrowserDocument doc, XElement node, XBrowserElementType elementType, IEnumerable<string> allowedElementNames) : base(node)
		{
			ElementType = elementType;
			Document = doc;
			Name = node.Name.LocalName.ToLower();
			NodesInternal = new List<XBrowserNode>();
			AddNodesInternal(allowedElementNames);
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
			return new XTextNode(xText);
		}

		private static XBrowserElement GetElementInternal(XBrowserDocument doc, XElement xElement)
		{
			switch(xElement.Name.LocalName.ToLower())
			{
				case "a": return new XAnchorElement(doc, xElement);
				case "abbr": return new XAbbrElement(doc, xElement);
				case "address": return new XAddressElement(doc, xElement);
				case "area": return new XAreaElement(doc, xElement);
				case "article": return new XArticleElement(doc, xElement);
				case "aside": return new XAsideElement(doc, xElement);
				case "audio": return new XAudioElement(doc, xElement);
				case "b": return new XBoldElement(doc, xElement);
				case "base": return new XBaseElement(doc, xElement);
				case "bdo": return new XBdoElement(doc, xElement);
				case "blockquote": return new XBlockquoteElement(doc, xElement);
				case "body": return new XBodyElement(doc, xElement);
				case "br": return new XBrElement(doc, xElement);
				case "button": return new XButtonElement(doc, xElement);
				case "canvas": return new XCanvasElement(doc, xElement);
				case "caption": return new XCaptionElement(doc, xElement);
				case "cite": return new XCiteElement(doc, xElement);
				case "col": return new XColElement(doc, xElement);
				case "colgroup": return new XColgroupElement(doc, xElement);
				case "command": return new XCommandElement(doc, xElement);
				case "datalist": return new XDatalistElement(doc, xElement);
				case "dd": return new XDdElement(doc, xElement);
				case "del": return new XDelElement(doc, xElement);
				case "details": return new XDetailsElement(doc, xElement);
				case "dfn": return new XDfnElement(doc, xElement);
				case "div": return new XDivElement(doc, xElement);
				case "dl": return new XDlElement(doc, xElement);
				case "dt": return new XDtElement(doc, xElement);
				case "em": return new XEmElement(doc, xElement);
				case "embed": return new XEmbedElement(doc, xElement);
				case "fieldset": return new XFieldsetElement(doc, xElement);
				case "figcaption": return new XFigcaptionElement(doc, xElement);
				case "figure": return new XFigureElement(doc, xElement);
				case "footer": return new XFooterElement(doc, xElement);
				case "form": return new XFormElement(doc, xElement);
				case "h1": return new XH1Element(doc, xElement);
				case "h2": return new XH2Element(doc, xElement);
				case "h3": return new XH3Element(doc, xElement);
				case "h4": return new XH4Element(doc, xElement);
				case "h5": return new XH5Element(doc, xElement);
				case "h6": return new XH6Element(doc, xElement);
				case "head": return new XHeadElement(doc, xElement);
				case "header": return new XHeaderElement(doc, xElement);
				case "hgroup": return new XHgroupElement(doc, xElement);
				case "hr": return new XHrElement(doc, xElement);
				case "html": return new XHtmlElement(doc, xElement);
				case "i": return new XItalicElement(doc, xElement);
				case "iframe": return new XIframeElement(doc, xElement);
				case "img": return new XImgElement(doc, xElement);
				case "input": return new XInputElement(doc, xElement);
				case "ins": return new XInsElement(doc, xElement);
				case "kbd": return new XKbdElement(doc, xElement);
				case "keygen": return new XKeygenElement(doc, xElement);
				case "label": return new XLabelElement(doc, xElement);
				case "legend": return new XLegendElement(doc, xElement);
				case "li": return new XLiElement(doc, xElement);
				case "link": return new XLinkElement(doc, xElement);
				case "map": return new XMapElement(doc, xElement);
				case "mark": return new XMarkElement(doc, xElement);
				case "menu": return new XMenuElement(doc, xElement);
				case "meta": return new XMetaElement(doc, xElement);
				case "meter": return new XMeterElement(doc, xElement);
				case "nav": return new XNavElement(doc, xElement);
				case "noscript": return new XNoscriptElement(doc, xElement);
				case "object": return new XObjectElement(doc, xElement);
				case "ol": return new XOlElement(doc, xElement);
				case "optgroup": return new XOptgroupElement(doc, xElement);
				case "option": return new XOptionElement(doc, xElement);
				case "output": return new XOutputElement(doc, xElement);
				case "p": return new XParagraphElement(doc, xElement);
				case "param": return new XParamElement(doc, xElement);
				case "pre": return new XPreElement(doc, xElement);
				case "progress": return new XProgressElement(doc, xElement);
				case "q": return new XQuoteElement(doc, xElement);
				case "rp": return new XRpElement(doc, xElement);
				case "rt": return new XRtElement(doc, xElement);
				case "ruby": return new XRubyElement(doc, xElement);
				case "samp": return new XSampElement(doc, xElement);
				case "script": return new XScriptElement(doc, xElement);
				case "section": return new XSectionElement(doc, xElement);
				case "select": return new XSelectElement(doc, xElement);
				case "small": return new XSmallElement(doc, xElement);
				case "source": return new XSourceElement(doc, xElement);
				case "span": return new XSpanElement(doc, xElement);
				case "strong": return new XStrongElement(doc, xElement);
				case "style": return new XStyleElement(doc, xElement);
				case "sub": return new XSubElement(doc, xElement);
				case "summary": return new XSummaryElement(doc, xElement);
				case "sup": return new XSupElement(doc, xElement);
				case "table": return new XTableElement(doc, xElement);
				case "tbody": return new XTbodyElement(doc, xElement);
				case "td": return new XTdElement(doc, xElement);
				case "textarea": return new XTextareaElement(doc, xElement);
				case "tfoot": return new XTfootElement(doc, xElement);
				case "thead": return new XTheadElement(doc, xElement);
				case "time": return new XTimeElement(doc, xElement);
				case "title": return new XTitleElement(doc, xElement);
				case "tr": return new XTrElement(doc, xElement);
				case "ul": return new XUlElement(doc, xElement);
				case "var": return new XVarElement(doc, xElement);
				case "video": return new XVideoElement(doc, xElement);
	
				default: return new XUnknownHtmlElement(doc, xElement);
			}
		}

		internal static XBrowserElement Create(XBrowserDocument doc, XElement xElement)
		{
			return GetElementInternal(doc, xElement);
		}

		public string Name { get; protected set; }
		public new XElement XNode { get { return (XElement)base.XNode; } }
		public string Text { get { return XNode.Value; } }
		public XBrowserElementType ElementType { get; private set; }

		protected IEnumerable<XElement> FilterChildElements(IEnumerable<string> allowedElementNames)
		{
			return XNode.Nodes()
				.Where(n => n.NodeType == XmlNodeType.Element && allowedElementNames.Contains(((XElement)n).Name.LocalName.ToLower()))
				.Cast<XElement>();
		}

		private List<XBrowserNode> NodesInternal { get; set; }
	}
}