using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserQuoteElement : XBrowserElement
	{
		public XBrowserQuoteElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Q, null)
		{
		}
	}
}