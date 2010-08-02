using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XRtElement : XBrowserElement
	{
		public XRtElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Rt, null)
		{
		}
	}
}