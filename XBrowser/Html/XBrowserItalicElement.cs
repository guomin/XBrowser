using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserItalicElement : XBrowserElement
	{
		public XBrowserItalicElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.I, null)
		{
		}
	}
}