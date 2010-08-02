using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XEmbedElement : XBrowserElement
	{
		public XEmbedElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Embed, null)
		{
		}
	}
}