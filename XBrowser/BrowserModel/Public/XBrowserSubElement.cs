using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserSubElement : XBrowserElement
	{
		public XBrowserSubElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Sub, null)
		{
		}
	}
}