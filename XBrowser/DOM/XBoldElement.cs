using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBoldElement : XBrowserElement
	{
		public XBoldElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.B, null)
		{
		}
	}
}