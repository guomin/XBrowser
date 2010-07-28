using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserDetailsElement : XBrowserElement
	{
		public XBrowserDetailsElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Details, null)
		{
		}
	}
}