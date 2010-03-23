using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserMapElement : XBrowserElement
	{
		public XBrowserMapElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Map, null)
		{
		}
	}
}