using System.Collections.Generic;
using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserTitleElement : XBrowserElement
	{
		static readonly HashSet<string> AllowedChildNodes = new HashSet<string> { "?" };
		public XBrowserTitleElement(XBrowserDocument doc, XElement xElement) : base(doc, xElement, XBrowserElementType.Title, AllowedChildNodes)
		{
			
		}
	}
}