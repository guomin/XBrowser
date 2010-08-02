using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XHgroupElement : XBrowserElement
	{
		public XHgroupElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Hgroup, null)
		{
		}
	}
}