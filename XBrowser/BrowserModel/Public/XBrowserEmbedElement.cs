using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserEmbedElement : XBrowserElement
	{
		public XBrowserEmbedElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Embed, null)
		{
		}
	}
}