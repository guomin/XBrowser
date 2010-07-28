using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserInsElement : XBrowserElement
	{
		public XBrowserInsElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Ins, null)
		{
		}
	}
}