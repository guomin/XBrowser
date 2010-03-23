using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserSelectElement : XBrowserElement
	{
		public XBrowserSelectElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Select, null)
		{
		}
	}
}