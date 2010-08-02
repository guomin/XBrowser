using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XVideoElement : XBrowserElement
	{
		public XVideoElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Video, null)
		{
		}
	}
}