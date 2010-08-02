using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XScriptElement : XBrowserElement
	{
		public XScriptElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Script, null)
		{
		}
	}
}