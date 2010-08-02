using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XFormElement : XBrowserElement
	{
		public XFormElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Form, null)
		{
		}
	}
}