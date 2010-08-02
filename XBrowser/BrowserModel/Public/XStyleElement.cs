using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XStyleElement : XBrowserElement
	{
		public XStyleElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Style, null)
		{
		}
	}
}