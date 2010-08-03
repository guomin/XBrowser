using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XBrowserProject.Tests
{
	[TestClass]
	public class XBrowserWindowTests
	{
		[TestMethod]
		public void New_window_starts_at_about_blank()
		{
			var browser = new XBrowser();
			var win = browser.CreateWindow();
			Assert.IsNotNull(win.Document, "New window should not have a null document");
			Assert.AreEqual("about:blank", win.Document.Url.ToString(), "New window default document should be about:blank");
			Assert.IsFalse(win.IsNavigating, "New window default document should not be navigating");
		}
	}
}
