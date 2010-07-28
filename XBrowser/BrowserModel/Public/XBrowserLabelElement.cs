using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserLabelElement : XBrowserElement
	{
		public XBrowserLabelElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Label, null)
		{
		}
	}
}