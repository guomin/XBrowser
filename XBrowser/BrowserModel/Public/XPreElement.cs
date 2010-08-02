using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XPreElement : XBrowserElement
	{
		public XPreElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Pre, null)
		{
		}
	}
}