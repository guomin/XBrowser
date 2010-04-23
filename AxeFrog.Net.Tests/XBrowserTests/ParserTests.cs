using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using AxeFrog.Net.Parser;
using AxeFrog.Net.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AxeFrog.Net.Tests.XBrowserTests
{
	[TestClass]
	public class ParserTests
	{
		static class TestHtml
		{
			public static string Basic2 { get { return "http://xbrowser.axefrog.com/htmltests/basic2.htm"; } }
			public static string Malformed2 { get { return "http://xbrowser.axefrog.com/htmltests/malformed2.htm"; } }
		}

		string GetHtml(string url)
		{
			return new WebClient().DownloadString(url);
		}

		/// <summary>
		/// All Selector (“*”) - Selects all elements.
		/// </summary>
		[TestMethod]
		public void Test_HtmlTokenizer()
		{
			var html = GetHtml(TestHtml.Malformed2);
			var tokens = HtmlTokenizer.Parse(html);
			foreach(var token in tokens)
				if(!(token.Type == TokenType.Text && (token.Raw ?? "").Trim().Length == 0))
					//if(token.B != null)
					//    Console.WriteLine("{0}: {1} = {2}", token.Type, Regex.Replace(token.A ?? "", @"\s"," "), token.B);
					//else
						Console.WriteLine("{0}: {1}", token.Type, Regex.Replace(token.Raw ?? "", @"\s"," "));
		}
	}
}
