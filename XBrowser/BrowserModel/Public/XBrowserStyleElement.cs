using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserStyleElement : XBrowserElement
	{
		public XBrowserStyleElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Style, null)
		{
		}
	}
}