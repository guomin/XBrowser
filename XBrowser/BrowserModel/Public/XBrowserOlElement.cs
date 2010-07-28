using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserOlElement : XBrowserElement
	{
		public XBrowserOlElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Ol, null)
		{
		}
	}
}