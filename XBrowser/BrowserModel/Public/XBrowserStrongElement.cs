using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserStrongElement : XBrowserElement
	{
		public XBrowserStrongElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Strong, null)
		{
		}
	}
}