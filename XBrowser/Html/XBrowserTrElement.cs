using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserTrElement : XBrowserElement
	{
		public XBrowserTrElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Tr, null)
		{
		}
	}
}