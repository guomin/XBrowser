using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XTbodyElement : XBrowserElement
	{
		public XTbodyElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Tbody, null)
		{
		}
	}
}