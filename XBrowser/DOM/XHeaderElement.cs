using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XHeaderElement : XBrowserElement
	{
		public XHeaderElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Header, null)
		{
		}
	}
}