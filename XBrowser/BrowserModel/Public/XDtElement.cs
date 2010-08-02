using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XDtElement : XBrowserElement
	{
		public XDtElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Dt, null)
		{
		}
	}
}