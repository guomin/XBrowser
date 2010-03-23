using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserTextareaElement : XBrowserElement
	{
		public XBrowserTextareaElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Textarea, null)
		{
		}
	}
}