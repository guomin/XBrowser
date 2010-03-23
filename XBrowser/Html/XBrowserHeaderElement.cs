using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserHeaderElement : XBrowserElement
	{
		public XBrowserHeaderElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Header, null)
		{
		}
	}
}