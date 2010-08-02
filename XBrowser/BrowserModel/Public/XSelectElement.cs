using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XSelectElement : XBrowserElement
	{
		public XSelectElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Select, null)
		{
		}
	}
}