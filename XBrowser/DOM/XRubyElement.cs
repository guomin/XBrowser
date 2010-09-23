using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XRubyElement : XBrowserElement
	{
		public XRubyElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Ruby, null)
		{
		}
	}
}