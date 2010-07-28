using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserTbodyElement : XBrowserElement
	{
		public XBrowserTbodyElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Tbody, null)
		{
		}
	}
}