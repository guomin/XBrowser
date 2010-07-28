using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserIframeElement : XBrowserElement
	{
		public XBrowserIframeElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Iframe, null)
		{
		}
	}
}