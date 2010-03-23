using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserTimeElement : XBrowserElement
	{
		public XBrowserTimeElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Time, null)
		{
		}
	}
}