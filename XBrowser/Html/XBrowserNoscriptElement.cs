using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserNoscriptElement : XBrowserElement
	{
		public XBrowserNoscriptElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Noscript, null)
		{
		}
	}
}