using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XAbbrElement : XBrowserElement
	{
		public XAbbrElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Abbr, null)
		{
		}
	}
}