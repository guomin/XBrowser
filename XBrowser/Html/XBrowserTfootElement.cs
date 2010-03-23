using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserTfootElement : XBrowserElement
	{
		public XBrowserTfootElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Tfoot, null)
		{
		}
	}
}