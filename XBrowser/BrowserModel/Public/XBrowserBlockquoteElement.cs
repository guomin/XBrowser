using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserBlockquoteElement : XBrowserElement
	{
		public XBrowserBlockquoteElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Blockquote, null)
		{
		}
	}
}