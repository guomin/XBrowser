using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XTableElement : XBrowserElement
	{
		public XTableElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Table, null)
		{
		}
	}
}