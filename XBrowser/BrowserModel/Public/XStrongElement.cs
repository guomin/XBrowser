using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XStrongElement : XBrowserElement
	{
		public XStrongElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Strong, null)
		{
		}
	}
}