using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserLegendElement : XBrowserElement
	{
		public XBrowserLegendElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Legend, null)
		{
		}
	}
}