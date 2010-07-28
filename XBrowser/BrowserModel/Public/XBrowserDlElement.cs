using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserDlElement : XBrowserElement
	{
		public XBrowserDlElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Dl, null)
		{
		}
	}
}