using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XSectionElement : XBrowserElement
	{
		public XSectionElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Section, null)
		{
		}
	}
}