using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserMarkElement : XBrowserElement
	{
		public XBrowserMarkElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Mark, null)
		{
		}
	}
}