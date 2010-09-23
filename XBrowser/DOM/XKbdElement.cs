using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XKbdElement : XBrowserElement
	{
		public XKbdElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Kbd, null)
		{
		}
	}
}