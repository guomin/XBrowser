using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserBaseElement : XBrowserElement
	{
		public XBrowserBaseElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Base, null)
		{
		}
	}
}