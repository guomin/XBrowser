using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserAbbrElement : XBrowserElement
	{
		public XBrowserAbbrElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Abbr, null)
		{
		}
	}
}