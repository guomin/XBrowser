using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserProgressElement : XBrowserElement
	{
		public XBrowserProgressElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Progress, null)
		{
		}
	}
}