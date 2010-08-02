using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XProgressElement : XBrowserElement
	{
		public XProgressElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Progress, null)
		{
		}
	}
}