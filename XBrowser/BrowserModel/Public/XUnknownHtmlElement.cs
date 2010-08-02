using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XUnknownHtmlElement : XBrowserElement
	{
		public XUnknownHtmlElement(XBrowserDocument doc, XElement xElement) : base(doc, xElement, XBrowserElementType.Unknown, null)
		{
				
		}
	}
}