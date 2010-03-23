using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserColgroupElement : XBrowserElement
	{
		public XBrowserColgroupElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Colgroup, null)
		{
		}
	}
}