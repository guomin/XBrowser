using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserOutputElement : XBrowserElement
	{
		public XBrowserOutputElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Output, null)
		{
		}
	}
}