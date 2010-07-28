using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserBdoElement : XBrowserElement
	{
		public XBrowserBdoElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Bdo, null)
		{
		}
	}
}