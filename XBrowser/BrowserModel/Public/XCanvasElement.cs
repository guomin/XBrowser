using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XCanvasElement : XBrowserElement
	{
		public XCanvasElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Canvas, null)
		{
		}
	}
}