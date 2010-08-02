using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XTimeElement : XBrowserElement
	{
		public XTimeElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Time, null)
		{
		}
	}
}