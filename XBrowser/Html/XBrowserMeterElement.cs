using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserMeterElement : XBrowserElement
	{
		public XBrowserMeterElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Meter, null)
		{
		}
	}
}