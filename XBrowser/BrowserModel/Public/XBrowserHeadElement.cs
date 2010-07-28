using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserHeadElement : XBrowserElement
	{
		static readonly HashSet<string> AllowedChildNodes = new HashSet<string> { "base", "command", "link", "title", "meta", "noscript", "script", "style", "title" };
		public XBrowserHeadElement(XBrowserDocument doc, XElement xElement) : base(doc, xElement, XBrowserElementType.Head, AllowedChildNodes)
		{
		}
	}
}