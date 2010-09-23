using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XSourceElement : XBrowserElement
	{
		public XSourceElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Source, null)
		{
		}
	}
}