using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserSupElement : XBrowserElement
	{
		public XBrowserSupElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Sup, null)
		{
		}
	}
}