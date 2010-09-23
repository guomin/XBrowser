using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XTfootElement : XBrowserElement
	{
		public XTfootElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Tfoot, null)
		{
		}
	}
}