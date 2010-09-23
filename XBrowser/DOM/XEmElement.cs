using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XEmElement : XBrowserElement
	{
		public XEmElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Em, null)
		{
		}
	}
}