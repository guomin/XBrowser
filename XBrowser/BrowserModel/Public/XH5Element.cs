using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XH5Element : XBrowserElement
	{
		public XH5Element(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.H5, null)
		{
		}
	}
}