using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XRpElement : XBrowserElement
	{
		public XRpElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Rp, null)
		{
		}
	}
}