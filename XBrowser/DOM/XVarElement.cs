using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XVarElement : XBrowserElement
	{
		public XVarElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Var, null)
		{
		}
	}
}