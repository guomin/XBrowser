﻿using System.Xml.Linq;

namespace AxeFrog.Net.Html
{
	public class XBrowserCiteElement : XBrowserElement
	{
		public XBrowserCiteElement(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.Cite, null)
		{
		}
	}
}