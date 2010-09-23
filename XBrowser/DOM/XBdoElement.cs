using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBdoElement : XBrowserElement
	{
		public XBdoElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Bdo, null)
		{
		}
	}
}