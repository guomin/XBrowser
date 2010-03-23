using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserOptionElement : XBrowserElement
	{
		public XBrowserOptionElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Option, null)
		{
		}
	}
}