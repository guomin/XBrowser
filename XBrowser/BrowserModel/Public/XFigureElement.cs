using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XFigureElement : XBrowserElement
	{
		public XFigureElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Figure, null)
		{
		}
	}
}