using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserButtonElement : XBrowserElement
	{
		public XBrowserButtonElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Button, null)
		{
		}
	}
}