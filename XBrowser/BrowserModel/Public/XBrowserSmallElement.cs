using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserSmallElement : XBrowserElement
	{
		public XBrowserSmallElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Small, null)
		{
		}
	}
}