using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XSubElement : XBrowserElement
	{
		public XSubElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Sub, null)
		{
		}
	}
}