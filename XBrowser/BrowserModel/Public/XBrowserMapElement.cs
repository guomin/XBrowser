using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserMapElement : XBrowserElement
	{
		public XBrowserMapElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Map, null)
		{
		}
	}
}