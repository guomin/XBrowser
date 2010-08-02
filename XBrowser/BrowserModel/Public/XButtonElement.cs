using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XButtonElement : XBrowserElement
	{
		public XButtonElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Button, null)
		{
		}
	}
}