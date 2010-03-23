using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserHeadElement : XBrowserElement
	{
		static readonly HashSet<string> AllowedChildNodes = new HashSet<string> { "base", "command", "link", "title", "meta", "noscript", "script", "style", "title" };
		public XBrowserHeadElement(XBrowserDocument doc, XElement xElement) : base(doc, xElement, AllowedChildNodes)
		{
		}
	}
}