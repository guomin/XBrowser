using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XAddressElement : XBrowserElement
	{
		public XAddressElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Address, null)
		{
		}
	}
}