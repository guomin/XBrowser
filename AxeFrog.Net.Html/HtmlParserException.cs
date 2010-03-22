using System;

namespace AxeFrog.Net.Html
{
	public class HtmlParserException : Exception
	{
		public string Html { get; private set; }

		public HtmlParserException(string message, string html) : base(message)
		{
			Html = html;
		}

		public HtmlParserException(string message, string html, Exception ex) : base(message, ex)
		{
			Html = html;
		}
	}
}