using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserVarElement : XBrowserElement
	{
		public XBrowserVarElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Var, null)
		{
		}
	}
}