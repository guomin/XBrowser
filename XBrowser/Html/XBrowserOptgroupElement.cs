using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserOptgroupElement : XBrowserElement
	{
		public XBrowserOptgroupElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Optgroup, null)
		{
		}
	}
}