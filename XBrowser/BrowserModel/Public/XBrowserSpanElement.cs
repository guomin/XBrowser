using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserSpanElement : XBrowserElement
	{
		public XBrowserSpanElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Span, null)
		{
		}
	}
}