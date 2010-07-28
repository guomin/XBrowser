using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;

namespace XBrowserProject
{
	public static class StringUtil
	{
		public static string GenerateRandomString(int chars)
		{
			Random r = new Random();
			string s = "";
			for (int i = 0; i < chars; i++)
			{
				int x = r.Next(2);
				switch (x)
				{
					case 0: s += (char)(r.Next(10) + 48); break; // 0-9
					case 1: s += (char)(r.Next(26) + 65); break; // A-Z
					case 2: s += (char)(r.Next(26) + 97); break; // a-z
				}
			}
			return s;
		}

		public class LowerCaseComparer : IEqualityComparer<string>
		{
			public bool Equals(string x, string y)
			{
				return string.Compare(x, y, true) == 0;
			}

			public int GetHashCode(string obj)
			{
				return obj.ToLower().GetHashCode();
			}
		}

		public static string MakeQueryString(IDictionary values)
		{
			if (values == null)
				return string.Empty;
			List<string> list = new List<string>();
			foreach (object key in values.Keys)
				list.Add(HttpUtility.UrlEncode(key.ToString()) + "=" + HttpUtility.UrlEncode(values[key].ToString()));
			return list.Concat("&");
		}

		public static string MakeQueryString(NameValueCollection values)
		{
			if (values == null)
				return string.Empty;
			List<string> list = new List<string>();
			foreach (string key in values.Keys)
				list.Add(HttpUtility.UrlEncode(key) + "=" + HttpUtility.UrlEncode(values[key]));
			return list.Concat("&");
		}

		public static string MakeQueryString(params KeyValuePair<string, string>[] values)
		{
			Dictionary<string, string> v = new Dictionary<string, string>();
			foreach (KeyValuePair<string, string> kvp in values)
				v[kvp.Key] = kvp.Value;
			return MakeQueryString(v);
		}
	}
}