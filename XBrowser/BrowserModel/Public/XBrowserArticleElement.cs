using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserArticleElement : XBrowserElement
	{
		public XBrowserArticleElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Article, null)
		{
		}
	}
}