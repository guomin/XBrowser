using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserBrElement : XBrowserElement
	{
		public XBrowserBrElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Br, null)
		{
		}
	}
}