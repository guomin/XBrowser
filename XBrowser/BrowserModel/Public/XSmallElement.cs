using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XSmallElement : XBrowserElement
	{
		public XSmallElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Small, null)
		{
		}
	}
}