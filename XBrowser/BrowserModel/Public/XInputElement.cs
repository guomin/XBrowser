using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XInputElement : XBrowserElement
	{
		public XInputElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Input, null)
		{
		}
	}
}