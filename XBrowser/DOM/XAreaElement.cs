using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XAreaElement : XBrowserElement
	{
		public XAreaElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Area, null)
		{
		}
	}
}