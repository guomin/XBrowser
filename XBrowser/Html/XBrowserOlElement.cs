using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserOlElement : XBrowserElement
	{
		public XBrowserOlElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Ol, null)
		{
		}
	}
}