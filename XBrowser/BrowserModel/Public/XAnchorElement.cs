using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XAnchorElement : XBrowserElement
	{
		public XAnchorElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.A, null)
		{
		}
	}
}