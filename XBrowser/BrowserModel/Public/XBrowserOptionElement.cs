using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserOptionElement : XBrowserElement
	{
		public XBrowserOptionElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Option, null)
		{
		}
	}
}