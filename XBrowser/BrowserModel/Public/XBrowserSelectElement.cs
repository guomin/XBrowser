using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserSelectElement : XBrowserElement
	{
		public XBrowserSelectElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Select, null)
		{
		}
	}
}