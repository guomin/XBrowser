using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserTfootElement : XBrowserElement
	{
		public XBrowserTfootElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Tfoot, null)
		{
		}
	}
}