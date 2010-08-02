using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XNoscriptElement : XBrowserElement
	{
		public XNoscriptElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Noscript, null)
		{
		}
	}
}