using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserRpElement : XBrowserElement
	{
		public XBrowserRpElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Rp, null)
		{
		}
	}
}