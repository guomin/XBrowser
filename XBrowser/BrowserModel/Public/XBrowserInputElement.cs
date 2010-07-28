using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserInputElement : XBrowserElement
	{
		public XBrowserInputElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Input, null)
		{
		}
	}
}