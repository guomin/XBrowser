using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserTableElement : XBrowserElement
	{
		public XBrowserTableElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Table, null)
		{
		}
	}
}