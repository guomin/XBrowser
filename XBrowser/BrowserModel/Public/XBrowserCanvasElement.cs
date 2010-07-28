using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserCanvasElement : XBrowserElement
	{
		public XBrowserCanvasElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Canvas, null)
		{
		}
	}
}