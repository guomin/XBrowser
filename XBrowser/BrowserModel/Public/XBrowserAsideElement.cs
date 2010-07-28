using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserAsideElement : XBrowserElement
	{
		public XBrowserAsideElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Aside, null)
		{
		}
	}
}