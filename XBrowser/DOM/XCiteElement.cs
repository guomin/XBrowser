using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XCiteElement : XBrowserElement
	{
		public XCiteElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Cite, null)
		{
		}
	}
}