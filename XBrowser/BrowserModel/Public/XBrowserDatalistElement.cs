using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserDatalistElement : XBrowserElement
	{
		public XBrowserDatalistElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Datalist, null)
		{
		}
	}
}