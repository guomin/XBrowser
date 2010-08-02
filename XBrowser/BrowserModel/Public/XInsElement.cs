using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XInsElement : XBrowserElement
	{
		public XInsElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Ins, null)
		{
		}
	}
}