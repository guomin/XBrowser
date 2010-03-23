using System;
using System.Net;

namespace AxeFrog.Net
{
	public class XBrowserWindow
	{
		private readonly XBrowserWindow _parentWindow;

		internal XBrowserWindow(XBrowser browser)
		{
			Browser = browser;
		}

		internal XBrowserWindow(XBrowserWindow parentWindow)
		{
			_parentWindow = parentWindow;
			Browser = _parentWindow.Browser;
		}

		public bool Navigate(string url)
		{
			Uri uri;
			try
			{
				uri = new Uri(url);
			}
			catch(Exception ex)
			{
				throw new XBrowserWindowException(this, ex);
			}
			return Navigate(uri);
		}

		public bool Navigate(Uri url)
		{
			try
			{
				var req = new XBrowserRequest(url, XBrowserRequestMethod.Get);
				req.SetCookies(Browser.Cookies);
				req.MaxRedirects = Browser.Config.MaxRedirectsPerRequest;
				req.Timeout = Browser.Config.DefaultRequestTimeout;
				var response = req.Execute();
				if(response.Status == HttpStatusCode.OK)
				{
					Document = new XBrowserDocument(response.Uri, this, response);
					return true;
				}
				return false;
			}
			catch(Exception ex)
			{
				throw new XBrowserWindowException(this, ex);
			}
		}

		/// <summary>
		/// The main document for the window
		/// </summary>
		public XBrowserDocument Document { get; private set; }
		public XBrowser Browser { get; set; }
	}
}