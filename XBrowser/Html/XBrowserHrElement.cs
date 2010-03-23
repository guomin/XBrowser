using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserHrElement : XBrowserElement
	{
		public XBrowserHrElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Hr, null)
		{
		}
	}
}