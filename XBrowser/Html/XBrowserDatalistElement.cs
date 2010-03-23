using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserDatalistElement : XBrowserElement
	{
		public XBrowserDatalistElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Datalist, null)
		{
		}
	}
}