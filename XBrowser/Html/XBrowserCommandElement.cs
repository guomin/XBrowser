using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserCommandElement : XBrowserElement
	{
		public XBrowserCommandElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Command, null)
		{
		}
	}
}