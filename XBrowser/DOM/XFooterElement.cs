using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XFooterElement : XBrowserElement
	{
		public XFooterElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Footer, null)
		{
		}
	}
}