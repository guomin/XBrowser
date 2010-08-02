using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XCommandElement : XBrowserElement
	{
		public XCommandElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Command, null)
		{
		}
	}
}