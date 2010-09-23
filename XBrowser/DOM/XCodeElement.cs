using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XCodeElement : XBrowserElement
	{
		public XCodeElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Code, null)
		{
		}
	}
}