using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XH2Element : XBrowserElement
	{
		public XH2Element(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.H2, null)
		{
		}
	}
}