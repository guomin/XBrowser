using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XHtmlElement : XBrowserElement
	{
		static readonly HashSet<string> AllowedChildNodes = new HashSet<string> { "head", "body" };
		public XHtmlElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Html, AllowedChildNodes)
		{
		}
	}
}