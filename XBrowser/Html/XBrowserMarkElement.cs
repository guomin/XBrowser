using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserMarkElement : XBrowserElement
	{
		public XBrowserMarkElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Mark, null)
		{
		}
	}
}