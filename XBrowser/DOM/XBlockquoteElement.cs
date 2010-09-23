using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBlockquoteElement : XBrowserElement
	{
		public XBlockquoteElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Blockquote, null)
		{
		}
	}
}