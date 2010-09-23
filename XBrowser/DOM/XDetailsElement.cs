using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XDetailsElement : XBrowserElement
	{
		public XDetailsElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Details, null)
		{
		}
	}
}