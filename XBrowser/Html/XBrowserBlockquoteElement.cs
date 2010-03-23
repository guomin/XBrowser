using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserBlockquoteElement : XBrowserElement
	{
		public XBrowserBlockquoteElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Blockquote, null)
		{
		}
	}
}