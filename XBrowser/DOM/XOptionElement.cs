using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XOptionElement : XBrowserElement
	{
		public XOptionElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Option, null)
		{
		}
	}
}