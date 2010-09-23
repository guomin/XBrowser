using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XUlElement : XBrowserElement
	{
		public XUlElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Ul, null)
		{
		}
	}
}