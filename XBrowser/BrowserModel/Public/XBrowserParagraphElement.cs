using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserParagraphElement : XBrowserElement
	{
		public XBrowserParagraphElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.P, null)
		{
		}
	}
}