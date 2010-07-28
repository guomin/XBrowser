using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserObjectElement : XBrowserElement
	{
		public XBrowserObjectElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Object, null)
		{
		}
	}
}