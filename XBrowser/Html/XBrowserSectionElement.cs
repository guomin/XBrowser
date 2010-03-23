using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserSectionElement : XBrowserElement
	{
		public XBrowserSectionElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Section, null)
		{
		}
	}
}