using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XSpanElement : XBrowserElement
	{
		public XSpanElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Span, null)
		{
		}
	}
}