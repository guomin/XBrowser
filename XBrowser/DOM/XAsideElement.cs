using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XAsideElement : XBrowserElement
	{
		public XAsideElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Aside, null)
		{
		}
	}
}