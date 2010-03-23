using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserCodeElement : XBrowserElement
	{
		public XBrowserCodeElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Code, null)
		{
		}
	}
}