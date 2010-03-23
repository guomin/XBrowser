using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserFooterElement : XBrowserElement
	{
		public XBrowserFooterElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Footer, null)
		{
		}
	}
}