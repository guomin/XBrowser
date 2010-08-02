using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XHrElement : XBrowserElement
	{
		public XHrElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Hr, null)
		{
		}
	}
}