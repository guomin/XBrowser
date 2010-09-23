using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XLinkElement : XBrowserElement
	{
		public XLinkElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Link, null)
		{
		}
	}
}