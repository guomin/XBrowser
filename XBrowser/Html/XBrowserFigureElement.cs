using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserFigureElement : XBrowserElement
	{
		public XBrowserFigureElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Figure, null)
		{
		}
	}
}