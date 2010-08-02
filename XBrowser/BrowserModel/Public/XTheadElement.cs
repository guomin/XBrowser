using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XTheadElement : XBrowserElement
	{
		public XTheadElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Thead, null)
		{
		}
	}
}