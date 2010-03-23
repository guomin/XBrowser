using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserHtmlElement : XBrowserElement
	{
		static readonly HashSet<string> AllowedChildNodes = new HashSet<string> { "head", "body" };
		public XBrowserHtmlElement(XBrowserDocument doc, XElement node) : base(doc, node, AllowedChildNodes)
		{
		}
	}
}