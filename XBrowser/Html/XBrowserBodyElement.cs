using System.Collections.Generic;
using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserBodyElement : XBrowserElement
	{
		static readonly HashSet<string> AllowedChildNodes = new HashSet<string> { "?" };
		public XBrowserBodyElement(XBrowserDocument doc, XElement xElement) : base(doc, xElement, XBrowserElementType.Body, AllowedChildNodes)
		{
			
		}
	}
}