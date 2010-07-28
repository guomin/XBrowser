using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserEmElement : XBrowserElement
	{
		public XBrowserEmElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Em, null)
		{
		}
	}
}