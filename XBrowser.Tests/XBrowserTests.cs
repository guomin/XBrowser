using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XBrowserProject.Tests
{
	[TestClass]
	public class XBrowserTests
	{
		[TestMethod]
		public void Can_construct()
		{
			new XBrowser();
		}

		[TestMethod]
		public void Browser_window_collection_is_read_only()
		{
			var browser = new XBrowser();
			Assert.IsInstanceOfType(browser.Windows, typeof(ReadOnlyCollection<XBrowserWindow>));
		}

		[TestMethod]
		public void New_browser_contains_non_null_empty_cookie_store()
		{
			var browser = new XBrowser();
			Assert.IsNotNull(browser.Cookies, "A new browser instance should contain an initialized cookie collection");
			Assert.AreEqual(0, browser.Cookies.Count, "New browser cookie collection should be empty");
		}

		[TestMethod]
		public void Can_create_window_and_it_is_tied_to_browser_instance_and_findable()
		{
			var browser = new XBrowser();
			var win = browser.CreateWindow();
			Assert.AreSame(browser, win.Browser);
			Assert.AreEqual(1, browser.Windows.Count(w => ReferenceEquals(win, w)), "Newly-created window should exist exactly once in browser windows collection");
		}
	}
}
