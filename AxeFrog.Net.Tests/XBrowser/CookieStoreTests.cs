using System;
using System.Net;
using System.Web;
using AxeFrog.Net.XBrowser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AxeFrog.Net.Tests.XBrowser
{
	/// <summary>
	/// Summary description for CookieStoreTests
	/// </summary>
	[TestClass]
	public class CookieStoreTests
	{
		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext { get; set; }

		[TestMethod]
		public void Test_Cookie_Count_For_One_Cookie()
		{
			var uri = new Uri("http://www.example.net");
			var list = CookieStore.Parse("name=test; expires=Sun, 31-Dec-2099 23:59:59 GMT; path=/; domain=.example.net");
			Assert.AreEqual(1, list.Count, "Exactly one cookie should have been returned");
		}

		[TestMethod]
		public void Test_Cookie_Count_For_Two_Cookies()
		{
			var uri = new Uri("http://www.example.net");
			var list = CookieStore.Parse("name=test; expires=Sun, 31-Dec-2099 23:59:59 GMT; path=/; domain=.example.net; name2=test2; expires=Fri, 31-Dec-2099 23:59:59 GMT; path=/; domain=.example.net");
			Assert.AreEqual(2, list.Count, "Exactly two cookies should have been returned");
		}

		[TestMethod]
		public void Test_Cookie_Count_For_Three_Cookies()
		{
			var uri = new Uri("http://www.example.net");
			var list = CookieStore.Parse("skin=noskin; path=/; domain=.amazon.com; expires=Mon, 22-Mar-2010 18:40:34 GMT,session-id-time=1269846000l; path=/; domain=.amazon.com; expires=Mon Mar 29 07:00:00 2010 GMT,session-id=191-5750312-0633062; path=/; domain=.amazon.com; expires=Mon Mar 29 07:00:00 2010 GMT");
			Assert.AreEqual(3, list.Count, "Exactly three cookies should have been returned");
		}

		[TestMethod]
		public void Header_With_Three_Different_Cookies_Parses_Cookie_Attributes_Correctly()
		{
			var uri = new Uri("http://www.example.net");
			var list = CookieStore.Parse("name=test; expires=Sun, 31-Dec-2079 23:59:59 GMT; path=/test; domain=.example.net; name2=test2; expires=Sat, 31-Dec-2089 23:59:59 GMT; path=/; domain=.example.net; name3=test3; expires=Thu, 31-Dec-2099 23:59:59 GMT; path=/; domain=.example.net");
			Assert.AreEqual(3, list.Count, "Exactly three cookies should have been returned");
			Assert.AreEqual(new DateTime(2079, 12, 31, 23, 59, 59), list[0].Expires, "Cookie date parsed incorrectly or not parsed");
			Assert.AreEqual(".example.net", list[0].Domain);
			Assert.AreEqual("/test", list[0].Path);
		}

		[TestMethod]
		public void Test_Amazon()
		{
			var wc = new WebClient();
			var uri = new Uri("http://www.amazon.com");
			wc.DownloadString(uri);
			var sc = wc.ResponseHeaders[HttpResponseHeader.SetCookie];
			var list = CookieStore.Parse(sc);

		}
	}
}
