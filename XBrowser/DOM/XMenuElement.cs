using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XMenuElement : XBrowserElement
	{
		public XMenuElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Menu, null)
		{
		}
	}
}