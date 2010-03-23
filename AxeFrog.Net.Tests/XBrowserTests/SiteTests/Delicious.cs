using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AxeFrog.Net.Tests.XBrowserTests.SiteTests
{
	[TestClass]
	public class Delicious
	{
		public TestContext TestContext { get; set; }

		[TestMethod]
		public void Load_Front_Page()
		{
			var browser = new XBrowser();
			var window = browser.CreateWindow();
			window.Navigate("http://www.delicious.com");
			Assert.AreEqual((window.Document.Url ?? new Uri("about:blank")).ToString(), new Uri("http://delicious.com").ToString(), "Should have redirected to http://delicious.com");
			Assert.AreEqual(window.Document.Title, "Delicious", "Browser title should have been \"Delicious\"");
		}
	}
}
