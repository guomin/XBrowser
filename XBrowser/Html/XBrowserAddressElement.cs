using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserAddressElement : XBrowserElement
	{
		public XBrowserAddressElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Address, null)
		{
		}
	}
}