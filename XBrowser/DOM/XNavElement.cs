using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XNavElement : XBrowserElement
	{
		public XNavElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Nav, null)
		{
		}
	}
}