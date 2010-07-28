using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserH1Element : XBrowserElement
	{
		public XBrowserH1Element(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.H1, null)
		{
		}
	}
}