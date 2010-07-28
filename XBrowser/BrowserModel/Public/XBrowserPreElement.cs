using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserPreElement : XBrowserElement
	{
		public XBrowserPreElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Pre, null)
		{
		}
	}
}