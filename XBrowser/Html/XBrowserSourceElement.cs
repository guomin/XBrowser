using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserSourceElement : XBrowserElement
	{
		public XBrowserSourceElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Source, null)
		{
		}
	}
}