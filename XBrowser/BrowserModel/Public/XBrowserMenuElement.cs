using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserMenuElement : XBrowserElement
	{
		public XBrowserMenuElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Menu, null)
		{
		}
	}
}