using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XTdElement : XBrowserElement
	{
		public XTdElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Td, null)
		{
		}
	}
}