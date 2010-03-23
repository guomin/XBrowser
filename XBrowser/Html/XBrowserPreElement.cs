using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserPreElement : XBrowserElement
	{
		public XBrowserPreElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Pre, null)
		{
		}
	}
}