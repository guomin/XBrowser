using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserImgElement : XBrowserElement
	{
		public XBrowserImgElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Img, null)
		{
		}
	}
}