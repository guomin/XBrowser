using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserDtElement : XBrowserElement
	{
		public XBrowserDtElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Dt, null)
		{
		}
	}
}