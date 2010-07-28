using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserSectionElement : XBrowserElement
	{
		public XBrowserSectionElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Section, null)
		{
		}
	}
}