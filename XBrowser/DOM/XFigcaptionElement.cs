using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XFigcaptionElement : XBrowserElement
	{
		public XFigcaptionElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Figcaption, null)
		{
		}
	}
}