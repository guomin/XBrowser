using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserFigcaptionElement : XBrowserElement
	{
		public XBrowserFigcaptionElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Figcaption, null)
		{
		}
	}
}