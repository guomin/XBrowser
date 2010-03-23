using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserInputElement : XBrowserElement
	{
		public XBrowserInputElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Input, null)
		{
		}
	}
}