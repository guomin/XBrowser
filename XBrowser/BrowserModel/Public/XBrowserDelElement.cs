using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserDelElement : XBrowserElement
	{
		public XBrowserDelElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Del, null)
		{
		}
	}
}