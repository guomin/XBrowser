using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserAnchorElement : XBrowserElement
	{
		public XBrowserAnchorElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.A, null)
		{
		}
	}
}