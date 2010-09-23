using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XDivElement : XBrowserElement
	{
		public XDivElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Div, null)
		{
		}
	}
}