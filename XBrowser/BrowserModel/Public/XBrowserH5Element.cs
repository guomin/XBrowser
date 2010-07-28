﻿using System.Xml.Linq;

namespace XBrowserProject.Html
{
	public class XBrowserH5Element : XBrowserElement
	{
		public XBrowserH5Element(XBrowserDocument doc, XElement node) : base(doc, node, XBrowserElementType.H5, null)
		{
		}
	}
}