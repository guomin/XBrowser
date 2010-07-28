using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserBaseElement : XBrowserElement
	{
		public XBrowserBaseElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Base, null)
		{
		}
	}
}