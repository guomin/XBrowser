using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserParagraphElement : XBrowserElement
	{
		public XBrowserParagraphElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.P, null)
		{
		}
	}
}