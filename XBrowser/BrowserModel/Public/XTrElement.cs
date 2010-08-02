using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XTrElement : XBrowserElement
	{
		public XTrElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Tr, null)
		{
		}
	}
}