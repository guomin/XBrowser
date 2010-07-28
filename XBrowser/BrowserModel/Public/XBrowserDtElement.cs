using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserDtElement : XBrowserElement
	{
		public XBrowserDtElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Dt, null)
		{
		}
	}
}