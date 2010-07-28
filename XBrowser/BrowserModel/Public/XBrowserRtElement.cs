using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserRtElement : XBrowserElement
	{
		public XBrowserRtElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Rt, null)
		{
		}
	}
}