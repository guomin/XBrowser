using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserNoscriptElement : XBrowserElement
	{
		public XBrowserNoscriptElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Noscript, null)
		{
		}
	}
}