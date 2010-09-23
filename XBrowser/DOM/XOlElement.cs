using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XOlElement : XBrowserElement
	{
		public XOlElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Ol, null)
		{
		}
	}
}