using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XParagraphElement : XBrowserElement
	{
		public XParagraphElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.P, null)
		{
		}
	}
}