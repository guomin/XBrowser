using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserSummaryElement : XBrowserElement
	{
		public XBrowserSummaryElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Summary, null)
		{
		}
	}
}