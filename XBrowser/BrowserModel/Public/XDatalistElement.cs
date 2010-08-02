using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XDatalistElement : XBrowserElement
	{
		public XDatalistElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Datalist, null)
		{
		}
	}
}