using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserStyleElement : XBrowserElement
	{
		public XBrowserStyleElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Style, null)
		{
		}
	}
}