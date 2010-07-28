using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserFormElement : XBrowserElement
	{
		public XBrowserFormElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Form, null)
		{
		}
	}
}