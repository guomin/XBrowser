using System.Collections.Generic;
using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBodyElement : XBrowserElement
	{
		static readonly HashSet<string> AllowedChildNodes = new HashSet<string> { "?" };
		public XBodyElement(XBrowserDocument doc, XElement xElement) : base(doc, xElement, XBrowserElementType.Body, AllowedChildNodes)
		{
			
		}
	}
}