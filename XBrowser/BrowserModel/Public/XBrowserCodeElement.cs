using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserCodeElement : XBrowserElement
	{
		public XBrowserCodeElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Code, null)
		{
		}
	}
}