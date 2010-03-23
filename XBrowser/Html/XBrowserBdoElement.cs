using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserBdoElement : XBrowserElement
	{
		public XBrowserBdoElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Bdo, null)
		{
		}
	}
}