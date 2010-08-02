using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XItalicElement : XBrowserElement
	{
		public XItalicElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.I, null)
		{
		}
	}
}