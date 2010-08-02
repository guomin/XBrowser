using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XDlElement : XBrowserElement
	{
		public XDlElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Dl, null)
		{
		}
	}
}