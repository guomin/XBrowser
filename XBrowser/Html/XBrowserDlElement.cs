using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserDlElement : XBrowserElement
	{
		public XBrowserDlElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Dl, null)
		{
		}
	}
}