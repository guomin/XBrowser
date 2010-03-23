using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserH2Element : XBrowserElement
	{
		public XBrowserH2Element(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.H2, null)
		{
		}
	}
}