using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XDelElement : XBrowserElement
	{
		public XDelElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Del, null)
		{
		}
	}
}