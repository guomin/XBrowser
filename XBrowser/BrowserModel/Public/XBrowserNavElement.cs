using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserNavElement : XBrowserElement
	{
		public XBrowserNavElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Nav, null)
		{
		}
	}
}