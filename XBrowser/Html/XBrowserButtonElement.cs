using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserButtonElement : XBrowserElement
	{
		public XBrowserButtonElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Button, null)
		{
		}
	}
}