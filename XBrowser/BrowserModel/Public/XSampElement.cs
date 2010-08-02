using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XSampElement : XBrowserElement
	{
		public XSampElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Samp, null)
		{
		}
	}
}