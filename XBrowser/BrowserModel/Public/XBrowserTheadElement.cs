using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserTheadElement : XBrowserElement
	{
		public XBrowserTheadElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Thead, null)
		{
		}
	}
}