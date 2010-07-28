using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XBrowserProject.Tests
{
	[TestClass]
	public class CookieStoreTests
	{
		[TestMethod]
		public void Test_Cookie_Count_For_One_Cookie()
		{
			var list = XBrowserCookieStore.Parse("name=test; expires=Sun, 31-Dec-2099 23:59:59 GMT; path=/; domain=.example.net");
			Assert.AreEqual(1, list.Count, "Exactly one cookie should have been returned");
		}

		[TestMethod]
		public void Test_Cookie_Count_For_Two_Cookies()
		{
			var list = XBrowserCookieStore.Parse("name=test; expires=Sun, 31-Dec-2099 23:59:59 GMT; path=/; domain=.example.net; name2=test2; expires=Fri, 31-Dec-2099 23:59:59 GMT; path=/; domain=.example.net");
			Assert.AreEqual(2, list.Count, "Exactly two cookies should have been returned");
		}

		[TestMethod]
		public void Test_Cookie_Count_For_Three_Cookies()
		{
			var list = XBrowserCookieStore.Parse("skin=noskin; path=/; domain=.amazon.com; expires=Mon, 22-Mar-2010 18:40:34 GMT,session-id-time=1269846000l; path=/; domain=.amazon.com; expires=Mon Mar 29 07:00:00 2010 GMT,session-id=191-5750312-0633062; path=/; domain=.amazon.com; expires=Mon Mar 29 07:00:00 2010 GMT");
			Assert.AreEqual(3, list.Count, "Exactly three cookies should have been returned");
		}

		[TestMethod]
		public void Header_With_Three_Different_Cookies_Parses_Cookie_Attributes_Correctly()
		{
			var list = XBrowserCookieStore.Parse("name=test; expires=Sun, 31-Dec-2079 23:59:59 GMT; path=/test; domain=.example.net; name2=test2; expires=Sat, 31-Dec-2089 23:59:59 GMT; path=/; domain=.example.net; name3=test3; expires=Thu, 31-Dec-2099 23:59:59 GMT; path=/; domain=.example.net");
			Assert.AreEqual(3, list.Count, "Exactly three cookies should have been returned");
			Assert.AreEqual(new DateTime(2079, 12, 31, 23, 59, 59), list[0].Expires, "Cookie date parsed incorrectly or not parsed");
			Assert.AreEqual(".example.net", list[0].Domain);
			Assert.AreEqual("/test", list[0].Path);
		}
	}
}
