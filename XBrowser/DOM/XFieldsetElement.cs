using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XFieldsetElement : XBrowserElement
	{
		public XFieldsetElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Fieldset, null)
		{
		}
	}
}