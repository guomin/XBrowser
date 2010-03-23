using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserSubElement : XBrowserElement
	{
		public XBrowserSubElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Sub, null)
		{
		}
	}
}