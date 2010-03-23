using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserLegendElement : XBrowserElement
	{
		public XBrowserLegendElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Legend, null)
		{
		}
	}
}