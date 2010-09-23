using System.Collections.Generic;
using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XTitleElement : XBrowserElement
	{
		static readonly HashSet<string> AllowedChildNodes = new HashSet<string> { "?" };
		public XTitleElement(XBrowserDocument doc, XElement xElement) : base(doc, xElement, XBrowserElementType.Title, AllowedChildNodes)
		{
			
		}
	}
}