using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserParamElement : XBrowserElement
	{
		public XBrowserParamElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Param, null)
		{
		}
	}
}