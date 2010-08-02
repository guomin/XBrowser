using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBaseElement : XBrowserElement
	{
		public XBaseElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Base, null)
		{
		}
	}
}