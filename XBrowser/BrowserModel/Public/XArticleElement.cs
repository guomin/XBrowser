using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XArticleElement : XBrowserElement
	{
		public XArticleElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Article, null)
		{
		}
	}
}