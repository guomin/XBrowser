using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserFigcaptionElement : XBrowserElement
	{
		public XBrowserFigcaptionElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Figcaption, null)
		{
		}
	}
}