using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XMetaElement : XBrowserElement
	{
		public XMetaElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Meta, null)
		{
		}
	}
}