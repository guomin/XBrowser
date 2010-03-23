using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserBoldElement : XBrowserElement
	{
		public XBrowserBoldElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.B, null)
		{
		}
	}
}