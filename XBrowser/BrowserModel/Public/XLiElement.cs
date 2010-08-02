using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XLiElement : XBrowserElement
	{
		public XLiElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Li, null)
		{
		}
	}
}