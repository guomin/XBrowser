using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserSourceElement : XBrowserElement
	{
		public XBrowserSourceElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Source, null)
		{
		}
	}
}