using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XSummaryElement : XBrowserElement
	{
		public XSummaryElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Summary, null)
		{
		}
	}
}