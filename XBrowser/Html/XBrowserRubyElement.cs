using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserRubyElement : XBrowserElement
	{
		public XBrowserRubyElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Ruby, null)
		{
		}
	}
}