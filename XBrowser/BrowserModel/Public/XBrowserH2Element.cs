using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserH2Element : XBrowserElement
	{
		public XBrowserH2Element(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.H2, null)
		{
		}
	}
}