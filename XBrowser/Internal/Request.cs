using System;
using System.Net;

namespace XBrowserProject.Internal
{
	public class Request
	{
		private readonly Uri _uri;
		private readonly XBrowserRequestMethod _method;
		private CookieStore _cookies;
		private Uri _referrer;

		static Request()
		{
			// most website requests fail without this setting disabled
			if(ServicePointManager.Expect100Continue)
				ServicePointManager.Expect100Continue = false;

			// requests to addresses with invalid certificates will throw exceptions without this
			ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
		}

		internal Request(Uri uri, XBrowserRequestMethod method)
		{
			_uri = uri;
			_method = method;
			MaxRedirects = 10;
			Timeout = 30000;
		}

		/// <summary>
		/// Gets or sets the maximum number of 301 or 302 redirects allowed during the request
		/// </summary>
		public int MaxRedirects { get; set; }

		/// <summary>
		/// Gets or sets the timeout (in milliseconds) for the request. Note that this timeout will be independently
		/// used for every request that occurs during a set of request redirects (e.g. for a set of 302 redirects).
		/// </summary>
		public int Timeout { get; set; }

		public void SetCookies(CookieStore cookies)
		{
			_cookies = cookies;
		}

		public void SetReferrer(Uri url)
		{
			_referrer = url;
		}

		public Response Execute()
		{
			HttpWebResponse response;

			var referrer = _referrer == null ? null : _referrer.ToString();
			var maxRedirects = MaxRedirects;
			var uri = _uri;
			var method = _method == XBrowserRequestMethod.Get ? "GET" : "POST";

			bool handleRedirect; // when true, we need to manually process a redirect
			do
			{
				if(maxRedirects-- == 0)
					throw new XBrowserRequestException(this, "Too many redirects were attempted during the request for " + _uri);
				handleRedirect = false;

				var req = (HttpWebRequest)WebRequest.Create(uri);
				var cookieHeader = _cookies.GetHeader(uri);
				if(cookieHeader != null)
					req.Headers.Add("Cookie", cookieHeader);
				req.ContentType = "application/x-www-form-urlencoded";
				req.Accept = "*/*";
				req.AllowAutoRedirect = false;
				req.Referer = referrer;
				req.Method = method;
				req.Timeout = Timeout;

				// TO DO:
				// cookies, headers, proxy
				// alternate content type for forms with input[type=file] fields
				// accept (when does this need to change?)

				try
				{
					response = (HttpWebResponse)req.GetResponse();
				}
				catch(WebException ex)
				{
					return new Response(ex);
				}
				_cookies.UpdateFromHeader(response.Headers[HttpResponseHeader.SetCookie]);

				var status = (int)response.StatusCode;
				if(status == 301 || status == 302 || status == 303 || status == 307)
				{
					handleRedirect = true;
					method = "GET";
					referrer = uri.ToString();
					uri = new Uri(uri, response.Headers["Location"]);
				}

			} while(handleRedirect);

			return new Response(response);
		}
	}

	public class XBrowserRequestException : Exception
	{
		public Request Request { get; private set; }

		internal XBrowserRequestException(Request request, string message)
			: base(message)
		{
			Request = request;
		}
	}

	public enum XBrowserRequestMethod
	{
		Get,
		Post
	}
}