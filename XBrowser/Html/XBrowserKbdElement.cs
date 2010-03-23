using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserKbdElement : XBrowserElement
	{
		public XBrowserKbdElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Kbd, null)
		{
		}
	}
}