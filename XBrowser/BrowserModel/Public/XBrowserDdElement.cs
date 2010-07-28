using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserDdElement : XBrowserElement
	{
		public XBrowserDdElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Dd, null)
		{
		}
	}
}