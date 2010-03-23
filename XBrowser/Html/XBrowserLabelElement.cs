using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserLabelElement : XBrowserElement
	{
		public XBrowserLabelElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Label, null)
		{
		}
	}
}