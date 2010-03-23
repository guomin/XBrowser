using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserH3Element : XBrowserElement
	{
		public XBrowserH3Element(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.H3, null)
		{
		}
	}
}