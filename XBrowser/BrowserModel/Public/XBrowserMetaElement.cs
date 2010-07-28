using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserMetaElement : XBrowserElement
	{
		public XBrowserMetaElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Meta, null)
		{
		}
	}
}