using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace AxeFrog.Net.XBrowser
{
	public class CookieStore : IEnumerable<Cookie>
	{
		private static readonly Regex RxSplitCookiesByComma = new Regex(@"(?<!expires\s*=\s*[A-Za-z]{3}),", RegexOptions.ExplicitCapture);
		private static readonly Regex RxMatchCookieDomain = new Regex(@"^\.[A-Za-z]+(\.[A-Za-z]+)+$", RegexOptions.ExplicitCapture);
		private static readonly Regex RxOldDateCheck = new Regex(@"^(?<weekday>[A-Za-z]{3})\s+(?<month>[A-Za-z]{3})\s+(?<day>[0-9]{1,2})\s+(?<hour>[0-9]{1,2}):(?<minute>[0-9]{1,2}):(?<second>[0-9]{1,2})\s+(?<year>[0-9]{4})(\s+(?<timezone>[A-Z]+))?$", RegexOptions.ExplicitCapture);

		private List<Cookie> _cookies;

		public int Count { get { return _cookies.Count; } }
		public IEnumerator<Cookie> GetEnumerator() { return _cookies.GetEnumerator(); }
		IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

		public CookieStore()
		{
			_cookies = new List<Cookie>();
		}

		public CookieStore(string cookieHeaderValue)
		{
			_cookies = Parse(cookieHeaderValue);
			RemoveExpiredCookies();
		}

		public static List<Cookie> Parse(string cookieHeaderValue)
		{
			var list = new List<Cookie>();
			Cookie cookie;
			var commaDelimitedSections = RxSplitCookiesByComma.Split(cookieHeaderValue ?? "");
			foreach(var chunk in commaDelimitedSections)
			{
				cookie = null;
				foreach(var section in chunk.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
				{
					var arr = section.Split(new[] { '=' }, 2);
					var key = HttpUtility.UrlDecode(arr[0]).Trim();
					var value = arr.Length > 1 ? HttpUtility.UrlDecode(arr[1]).Trim() : "";
					if(cookie != null)
						switch(key.ToLower())
						{
							case "expires":
								DateTime dt;
								var match = RxOldDateCheck.Match(value);
								if(match.Success)
									value = string.Format("{0}, {1:00}-{2}-{3} {4:00}:{5:00}:{6:00} {7}",
									                      match.Groups["weekday"],
									                      int.Parse(match.Groups["day"].Value),
									                      match.Groups["month"].Value,
									                      match.Groups["year"].Value,
									                      int.Parse(match.Groups["hour"].Value),
									                      int.Parse(match.Groups["minute"].Value),
									                      int.Parse(match.Groups["second"].Value),
									                      match.Groups["timezone"].Success ? match.Groups["timezone"].Value : "GMT"
										);
								if(DateTime.TryParse(value, out dt))
									cookie.Expires = dt;
								break;

							case "domain":
								if(!value.StartsWith("."))
									value = "." + value;
								if(!RxMatchCookieDomain.IsMatch(value)) // must be in cookie domain format
									continue;
								cookie.Domain = value;
								break;

							case "path":
								cookie.Path = value;
								break;

							case "secure":
								cookie.Secure = true;
								break;

							case "httponly":
								cookie.HttpOnly = true;
								break;

							default:
								cookie = null;
								break;
						}

					if(cookie == null)
					{
						cookie = new Cookie(key, value);
						list.Add(cookie);
					}
				}
			}
			return list;
		}

		public void RemoveExpiredCookies()
		{
			//if(!requestUri.Host.ToLower().EndsWith(value.ToLower().Substring(1))) // must be a subset of the requesting domain
			//    continue;
		}
	}
}
