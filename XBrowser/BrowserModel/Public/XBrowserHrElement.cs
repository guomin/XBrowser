using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserHrElement : XBrowserElement
	{
		public XBrowserHrElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Hr, null)
		{
		}
	}
}