using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserUlElement : XBrowserElement
	{
		public XBrowserUlElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Ul, null)
		{
		}
	}
}