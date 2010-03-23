using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserAreaElement : XBrowserElement
	{
		public XBrowserAreaElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Area, null)
		{
		}
	}
}