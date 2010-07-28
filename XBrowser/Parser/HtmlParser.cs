using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace AxeFrog.Net.Parser
{
	public static class HtmlParser
	{
		public static XDocument ParseHtml(this string html)
		{
			var tokens = HtmlTokenizer.Parse(html);
			var doc = DocumentBuilder.Parse(tokens);
			DocumentCleaner.Rebuild(doc);
			return doc;
		}
	}
}
