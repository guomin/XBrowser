using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XKeygenElement : XBrowserElement
	{
		public XKeygenElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Keygen, null)
		{
		}
	}
}