using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserVideoElement : XBrowserElement
	{
		public XBrowserVideoElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Video, null)
		{
		}
	}
}