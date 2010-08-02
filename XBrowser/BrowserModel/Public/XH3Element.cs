using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XH3Element : XBrowserElement
	{
		public XH3Element(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.H3, null)
		{
		}
	}
}