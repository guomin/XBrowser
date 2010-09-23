using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XLegendElement : XBrowserElement
	{
		public XLegendElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Legend, null)
		{
		}
	}
}