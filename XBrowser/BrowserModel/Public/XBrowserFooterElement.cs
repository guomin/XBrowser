using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserFooterElement : XBrowserElement
	{
		public XBrowserFooterElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Footer, null)
		{
		}
	}
}