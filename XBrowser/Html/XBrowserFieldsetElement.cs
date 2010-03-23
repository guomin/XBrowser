using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserFieldsetElement : XBrowserElement
	{
		public XBrowserFieldsetElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Fieldset, null)
		{
		}
	}
}