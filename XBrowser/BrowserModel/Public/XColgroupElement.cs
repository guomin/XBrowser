using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XColgroupElement : XBrowserElement
	{
		public XColgroupElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Colgroup, null)
		{
		}
	}
}