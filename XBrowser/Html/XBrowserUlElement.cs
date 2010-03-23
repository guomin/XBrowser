using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserUlElement : XBrowserElement
	{
		public XBrowserUlElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Ul, null)
		{
		}
	}
}