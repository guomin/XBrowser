using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XQuoteElement : XBrowserElement
	{
		public XQuoteElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Q, null)
		{
		}
	}
}