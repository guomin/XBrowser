using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserScriptElement : XBrowserElement
	{
		public XBrowserScriptElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Script, null)
		{
		}
	}
}