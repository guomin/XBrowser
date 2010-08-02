using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XMeterElement : XBrowserElement
	{
		public XMeterElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Meter, null)
		{
		}
	}
}