using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserH3Element : XBrowserElement
	{
		public XBrowserH3Element(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.H3, null)
		{
		}
	}
}