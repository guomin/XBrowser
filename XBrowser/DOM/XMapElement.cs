using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XMapElement : XBrowserElement
	{
		public XMapElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Map, null)
		{
		}
	}
}