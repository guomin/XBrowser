using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XSupElement : XBrowserElement
	{
		public XSupElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Sup, null)
		{
		}
	}
}