using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XObjectElement : XBrowserElement
	{
		public XObjectElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Object, null)
		{
		}
	}
}