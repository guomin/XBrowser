using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XLabelElement : XBrowserElement
	{
		public XLabelElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Label, null)
		{
		}
	}
}