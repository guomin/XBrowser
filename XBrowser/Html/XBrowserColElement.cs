using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserColElement : XBrowserElement
	{
		public XBrowserColElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Col, null)
		{
		}
	}
}