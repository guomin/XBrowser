using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserProgressElement : XBrowserElement
	{
		public XBrowserProgressElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Progress, null)
		{
		}
	}
}