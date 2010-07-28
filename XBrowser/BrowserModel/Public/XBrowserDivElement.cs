using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserDivElement : XBrowserElement
	{
		public XBrowserDivElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Div, null)
		{
		}
	}
}