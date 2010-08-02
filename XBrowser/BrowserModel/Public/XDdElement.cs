using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XDdElement : XBrowserElement
	{
		public XDdElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Dd, null)
		{
		}
	}
}