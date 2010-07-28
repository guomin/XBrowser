using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserSampElement : XBrowserElement
	{
		public XBrowserSampElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Samp, null)
		{
		}
	}
}