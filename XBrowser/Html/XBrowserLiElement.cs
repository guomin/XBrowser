using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserLiElement : XBrowserElement
	{
		public XBrowserLiElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Li, null)
		{
		}
	}
}