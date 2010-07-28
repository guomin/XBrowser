using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserDfnElement : XBrowserElement
	{
		public XBrowserDfnElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Dfn, null)
		{
		}
	}
}