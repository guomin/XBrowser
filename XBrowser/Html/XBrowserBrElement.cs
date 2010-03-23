using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserBrElement : XBrowserElement
	{
		public XBrowserBrElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Br, null)
		{
		}
	}
}