using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserCanvasElement : XBrowserElement
	{
		public XBrowserCanvasElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Canvas, null)
		{
		}
	}
}