using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using HtmlAgilityPack;

namespace AxeFrog.Net.XBrowser
{
	public static class HtmlUtil
	{
		public static string SanitizeHtml(string html)
		{
			var doc = new HtmlDocument();
			doc.LoadHtml(html ?? "");
			using (StringWriter writer = new StringWriter())
			{
				using (XmlTextWriter xtw = new XmlTextWriter(writer))
				{
					xtw.Formatting = Formatting.Indented;
					xtw.IndentChar = '\t';
					xtw.Indentation = 1;
					doc.DocumentNode.WriteTo(xtw);
					xtw.Close();
					return writer.ToString();
				}
			}
		}

		public static XDocument SanitizeHtmlToXml(string html)
		{
			html = SanitizeHtml(html);
			XDocument doc;
			try { doc = XDocument.Parse(html); }
			catch (XmlException ex)
			{
				if (ex.Message.Contains("multiple root elements"))
				{
					html = "<?xml version=\"1.0\"?>\r\n<html>" + Regex.Replace(html, @"\<\?xml[^\>]*\?\>", "") + "\r\n</html>";
					doc = XDocument.Parse(html);
				}
				else
					throw;
			}
			return doc;
		}

		public enum RemoveMode
		{
			Include,
			Exclude
		}

		private static Regex _findTags = new Regex(@"\</?(?<name>[^\s\>]+)[^\>]*\>");
		public static string RemoveHtmlTags(string html, RemoveMode mode, params string[] tags)
		{
			var lcmp = new StringUtil.LowerCaseComparer();
			foreach (Match match in _findTags.Matches(html))
			{
				var listed = tags.Contains(match.Groups["name"].Value, lcmp);
				if ((listed && mode == RemoveMode.Exclude) || (!listed && mode == RemoveMode.Include))
					html = html.Replace(match.Value, "");
			}
			return html;
		}

		private static Regex _findURLs = new Regex(@"(?<http>https?\://[^\s]+)"
		                                           + @"|(?<www>(?<!\w)www(?:\.[\w\-]+){2,})"
		                                           + @"|(?<tld>(?:[\w\-]+\.)+(?:com|co|org|net|biz|name|info|gov|edu)(?:\.[a-zA-Z]{2})?)"
		                                           , RegexOptions.IgnoreCase | RegexOptions.Compiled);
		private static char[] _urlEnders = new [] { '.', '}', ')', '"', '!', ':', ']', ',' };
		public static string ConvertURLsToLinks(string html)
		{
			foreach (Match match in _findURLs.Matches(html))
			{
				string find, replace;
				if (match.Groups["http"].Success)
				{
					find = NormalizeUrlEnding(match.Value);
					replace = string.Format("<a href=\"{0}\" target=\"_blank\">{1}</a>", find, (find ?? "").ShortenTo(60, true));
				}
				else
				{
					if (match.Groups["www"].Success)
						find = NormalizeUrlEnding(match.Value);
					else
						find = match.Value;
					replace = string.Format("<a href=\"http://{0}\" target=\"_blank\">{1}</a>", find, (find ?? "").ShortenTo(60, true));
				}
				html = html.Replace(find, replace);
			}
			return html;
		}
		private static string NormalizeUrlEnding(string url)
		{
			return _urlEnders.Contains(url[url.Length - 1]) ? url.Substring(0, url.Length - 1) : url;
		}

		public static bool ContainsLinks(string html)
		{
			return _findURLs.IsMatch(html);
		}
	}
}