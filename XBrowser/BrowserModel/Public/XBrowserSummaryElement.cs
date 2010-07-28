using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserSummaryElement : XBrowserElement
	{
		public XBrowserSummaryElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Summary, null)
		{
		}
	}
}