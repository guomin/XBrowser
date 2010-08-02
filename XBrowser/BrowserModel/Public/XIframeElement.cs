using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XIframeElement : XBrowserElement
	{
		public XIframeElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Iframe, null)
		{
		}
	}
}