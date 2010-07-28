using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserRubyElement : XBrowserElement
	{
		public XBrowserRubyElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Ruby, null)
		{
		}
	}
}