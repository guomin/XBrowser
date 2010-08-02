using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XMarkElement : XBrowserElement
	{
		public XMarkElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Mark, null)
		{
		}
	}
}