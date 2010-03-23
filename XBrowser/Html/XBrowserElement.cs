using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace AxeFrog.Net.Html
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
			return new XBrowserTextNode(xText);
		}

		private static XBrowserElement GetElementInternal(XBrowserDocument doc, XElement xElement)
		{
			switch(xElement.Name.LocalName.ToLower())
			{
				case "a": return new XBrowserAnchorElement(doc, xElement);
				case "abbr": return new XBrowserAbbrElement(doc, xElement);
				case "address": return new XBrowserAddressElement(doc, xElement);
				case "area": return new XBrowserAreaElement(doc, xElement);
				case "article": return new XBrowserArticleElement(doc, xElement);
				case "aside": return new XBrowserAsideElement(doc, xElement);
				case "audio": return new XBrowserAudioElement(doc, xElement);
				case "b": return new XBrowserBoldElement(doc, xElement);
				case "base": return new XBrowserBaseElement(doc, xElement);
				case "bdo": return new XBrowserBdoElement(doc, xElement);
				case "blockquote": return new XBrowserBlockquoteElement(doc, xElement);
				case "body": return new XBrowserBodyElement(doc, xElement);
				case "br": return new XBrowserBrElement(doc, xElement);
				case "button": return new XBrowserButtonElement(doc, xElement);
				case "canvas": return new XBrowserCanvasElement(doc, xElement);
				case "caption": return new XBrowserCaptionElement(doc, xElement);
				case "cite": return new XBrowserCiteElement(doc, xElement);
				case "col": return new XBrowserColElement(doc, xElement);
				case "colgroup": return new XBrowserColgroupElement(doc, xElement);
				case "command": return new XBrowserCommandElement(doc, xElement);
				case "datalist": return new XBrowserDatalistElement(doc, xElement);
				case "dd": return new XBrowserDdElement(doc, xElement);
				case "del": return new XBrowserDelElement(doc, xElement);
				case "details": return new XBrowserDetailsElement(doc, xElement);
				case "dfn": return new XBrowserDfnElement(doc, xElement);
				case "div": return new XBrowserDivElement(doc, xElement);
				case "dl": return new XBrowserDlElement(doc, xElement);
				case "dt": return new XBrowserDtElement(doc, xElement);
				case "em": return new XBrowserEmElement(doc, xElement);
				case "embed": return new XBrowserEmbedElement(doc, xElement);
				case "fieldset": return new XBrowserFieldsetElement(doc, xElement);
				case "figcaption": return new XBrowserFigcaptionElement(doc, xElement);
				case "figure": return new XBrowserFigureElement(doc, xElement);
				case "footer": return new XBrowserFooterElement(doc, xElement);
				case "form": return new XBrowserFormElement(doc, xElement);
				case "h1": return new XBrowserH1Element(doc, xElement);
				case "h2": return new XBrowserH2Element(doc, xElement);
				case "h3": return new XBrowserH3Element(doc, xElement);
				case "h4": return new XBrowserH4Element(doc, xElement);
				case "h5": return new XBrowserH5Element(doc, xElement);
				case "h6": return new XBrowserH6Element(doc, xElement);
				case "head": return new XBrowserHeadElement(doc, xElement);
				case "header": return new XBrowserHeaderElement(doc, xElement);
				case "hgroup": return new XBrowserHgroupElement(doc, xElement);
				case "hr": return new XBrowserHrElement(doc, xElement);
				case "html": return new XBrowserHtmlElement(doc, xElement);
				case "i": return new XBrowserItalicElement(doc, xElement);
				case "iframe": return new XBrowserIframeElement(doc, xElement);
				case "img": return new XBrowserImgElement(doc, xElement);
				case "input": return new XBrowserInputElement(doc, xElement);
				case "ins": return new XBrowserInsElement(doc, xElement);
				case "kbd": return new XBrowserKbdElement(doc, xElement);
				case "keygen": return new XBrowserKeygenElement(doc, xElement);
				case "label": return new XBrowserLabelElement(doc, xElement);
				case "legend": return new XBrowserLegendElement(doc, xElement);
				case "li": return new XBrowserLiElement(doc, xElement);
				case "link": return new XBrowserLinkElement(doc, xElement);
				case "map": return new XBrowserMapElement(doc, xElement);
				case "mark": return new XBrowserMarkElement(doc, xElement);
				case "menu": return new XBrowserMenuElement(doc, xElement);
				case "meta": return new XBrowserMetaElement(doc, xElement);
				case "meter": return new XBrowserMeterElement(doc, xElement);
				case "nav": return new XBrowserNavElement(doc, xElement);
				case "noscript": return new XBrowserNoscriptElement(doc, xElement);
				case "object": return new XBrowserObjectElement(doc, xElement);
				case "ol": return new XBrowserOlElement(doc, xElement);
				case "optgroup": return new XBrowserOptgroupElement(doc, xElement);
				case "option": return new XBrowserOptionElement(doc, xElement);
				case "output": return new XBrowserOutputElement(doc, xElement);
				case "p": return new XBrowserParagraphElement(doc, xElement);
				case "param": return new XBrowserParamElement(doc, xElement);
				case "pre": return new XBrowserPreElement(doc, xElement);
				case "progress": return new XBrowserProgressElement(doc, xElement);
				case "q": return new XBrowserQuoteElement(doc, xElement);
				case "rp": return new XBrowserRpElement(doc, xElement);
				case "rt": return new XBrowserRtElement(doc, xElement);
				case "ruby": return new XBrowserRubyElement(doc, xElement);
				case "samp": return new XBrowserSampElement(doc, xElement);
				case "script": return new XBrowserScriptElement(doc, xElement);
				case "section": return new XBrowserSectionElement(doc, xElement);
				case "select": return new XBrowserSelectElement(doc, xElement);
				case "small": return new XBrowserSmallElement(doc, xElement);
				case "source": return new XBrowserSourceElement(doc, xElement);
				case "span": return new XBrowserSpanElement(doc, xElement);
				case "strong": return new XBrowserStrongElement(doc, xElement);
				case "style": return new XBrowserStyleElement(doc, xElement);
				case "sub": return new XBrowserSubElement(doc, xElement);
				case "summary": return new XBrowserSummaryElement(doc, xElement);
				case "sup": return new XBrowserSupElement(doc, xElement);
				case "table": return new XBrowserTableElement(doc, xElement);
				case "tbody": return new XBrowserTbodyElement(doc, xElement);
				case "td": return new XBrowserTdElement(doc, xElement);
				case "textarea": return new XBrowserTextareaElement(doc, xElement);
				case "tfoot": return new XBrowserTfootElement(doc, xElement);
				case "thead": return new XBrowserTheadElement(doc, xElement);
				case "time": return new XBrowserTimeElement(doc, xElement);
				case "title": return new XBrowserTitleElement(doc, xElement);
				case "tr": return new XBrowserTrElement(doc, xElement);
				case "ul": return new XBrowserUlElement(doc, xElement);
				case "var": return new XBrowserVarElement(doc, xElement);
				case "video": return new XBrowserVideoElement(doc, xElement);
	
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