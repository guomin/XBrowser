using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserAreaElement : XBrowserElement
	{
		public XBrowserAreaElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Area, null)
		{
		}
	}
}