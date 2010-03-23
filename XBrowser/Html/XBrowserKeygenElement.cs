using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserKeygenElement : XBrowserElement
	{
		public XBrowserKeygenElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Keygen, null)
		{
		}
	}
}