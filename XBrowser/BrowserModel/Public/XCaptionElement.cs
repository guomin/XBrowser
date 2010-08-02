using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XCaptionElement : XBrowserElement
	{
		public XCaptionElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Caption, null)
		{
		}
	}
}