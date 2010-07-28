using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml.Linq;
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

		[TestMethod]
		public void Test_HtmlTokenizer_Clean()
		{
			var html = GetHtml(TestHtml.Basic2);
			var tokens = HtmlTokenizer.Parse(html);
			foreach(var token in tokens)
				if(!(token.Type == TokenType.Text && (token.Raw ?? "").Trim().Length == 0))
					Console.WriteLine("{0}: {1}", token.Type, Regex.Replace(token.Raw ?? "", @"\s"," "));

			var doc = DocumentBuilder.Parse(tokens);
		}

		[TestMethod]
		public void Test_HtmlTokenizer_Malformed()
		{
			var html = GetHtml(TestHtml.Malformed2);
			var tokens = HtmlTokenizer.Parse(html);
			foreach(var token in tokens)
				if(!(token.Type == TokenType.Text && (token.Raw ?? "").Trim().Length == 0))
					Console.WriteLine("{0}: {1}", token.Type, Regex.Replace(token.Raw ?? "", @"\s"," "));
		}

		[TestMethod]
		public void Test_DocumentBuilder_Clean()
		{
			var html = GetHtml(TestHtml.Basic2);
			var tokens = HtmlTokenizer.Parse(html);
			var doc = DocumentBuilder.Parse(tokens);
			Console.WriteLine(doc);
		}

		[TestMethod]
		public void Test_DocumentBuilder_Malformed()
		{
			var html = GetHtml(TestHtml.Malformed2);
			var tokens = HtmlTokenizer.Parse(html);
			var doc = DocumentBuilder.Parse(tokens);
			Console.WriteLine(doc);
		}
	}
}
