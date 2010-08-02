using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XTextareaElement : XBrowserElement
	{
		public XTextareaElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Textarea, null)
		{
		}
	}
}