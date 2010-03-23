using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserIframeElement : XBrowserElement
	{
		public XBrowserIframeElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Iframe, null)
		{
		}
	}
}