using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserSpanElement : XBrowserElement
	{
		public XBrowserSpanElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Span, null)
		{
		}
	}
}