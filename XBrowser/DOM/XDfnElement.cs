using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XDfnElement : XBrowserElement
	{
		public XDfnElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Dfn, null)
		{
		}
	}
}