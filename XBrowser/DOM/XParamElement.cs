using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XParamElement : XBrowserElement
	{
		public XParamElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Param, null)
		{
		}
	}
}