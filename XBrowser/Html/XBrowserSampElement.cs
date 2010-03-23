using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserSampElement : XBrowserElement
	{
		public XBrowserSampElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Samp, null)
		{
		}
	}
}