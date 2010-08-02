using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrElement : XBrowserElement
	{
		public XBrElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Br, null)
		{
		}
	}
}