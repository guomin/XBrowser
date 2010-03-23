using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserSupElement : XBrowserElement
	{
		public XBrowserSupElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Sup, null)
		{
		}
	}
}