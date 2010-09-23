using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XH1Element : XBrowserElement
	{
		public XH1Element(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.H1, null)
		{
		}
	}
}