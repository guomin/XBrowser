using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserTheadElement : XBrowserElement
	{
		public XBrowserTheadElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Thead, null)
		{
		}
	}
}