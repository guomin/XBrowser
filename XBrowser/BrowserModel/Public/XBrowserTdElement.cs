using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserTdElement : XBrowserElement
	{
		public XBrowserTdElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Td, null)
		{
		}
	}
}