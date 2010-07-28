using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserAbbrElement : XBrowserElement
	{
		public XBrowserAbbrElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Abbr, null)
		{
		}
	}
}