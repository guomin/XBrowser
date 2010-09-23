using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XColElement : XBrowserElement
	{
		public XColElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Col, null)
		{
		}
	}
}