using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XOutputElement : XBrowserElement
	{
		public XOutputElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Output, null)
		{
		}
	}
}