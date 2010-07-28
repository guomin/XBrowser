using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserOutputElement : XBrowserElement
	{
		public XBrowserOutputElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Output, null)
		{
		}
	}
}