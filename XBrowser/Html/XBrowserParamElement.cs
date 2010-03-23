using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserParamElement : XBrowserElement
	{
		public XBrowserParamElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Param, null)
		{
		}
	}
}