using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XImgElement : XBrowserElement
	{
		public XImgElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Img, null)
		{
		}
	}
}