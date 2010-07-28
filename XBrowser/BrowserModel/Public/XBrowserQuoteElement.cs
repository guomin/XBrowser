using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserQuoteElement : XBrowserElement
	{
		public XBrowserQuoteElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Q, null)
		{
		}
	}
}