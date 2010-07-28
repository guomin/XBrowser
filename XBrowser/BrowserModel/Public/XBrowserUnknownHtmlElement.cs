using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserUnknownHtmlElement : XBrowserElement
	{
		public XBrowserUnknownHtmlElement(XBrowserDocument doc, XElement xElement) : base(doc, xElement, XBrowserElementType.Unknown, null)
		{
				
		}
	}
}