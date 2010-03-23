using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserDivElement : XBrowserElement
	{
		public XBrowserDivElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Div, null)
		{
		}
	}
}