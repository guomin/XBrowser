using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserLinkElement : XBrowserElement
	{
		public XBrowserLinkElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Link, null)
		{
		}
	}
}