using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserItalicElement : XBrowserElement
	{
		public XBrowserItalicElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.I, null)
		{
		}
	}
}