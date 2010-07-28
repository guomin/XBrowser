using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserCaptionElement : XBrowserElement
	{
		public XBrowserCaptionElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Caption, null)
		{
		}
	}
}