using System;
using System.Net;
using System.Threading;

namespace AxeFrog.Net
{
	/// <summary>
	/// A window represents a container encapsulating a document.
	/// </summary>
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

		/// <summary>
		/// Starts the window navigating to the specified url. This is a non-blocking operation. To be notified of completion, subscribe
		/// to Navigated or NavigateException, or call WaitUntilReady().
		/// </summary>
		/// <param name="url">The url to navigate to</param>
		public void Navigate(string url)
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
			Navigate(uri);
		}

		/// <summary>
		/// Starts the window navigating to the specified url. This is a non-blocking operation. To be notified of completion, subscribe
		/// to Navigated or NavigateException, or call WaitUntilReady().
		/// </summary>
		/// <param name="url">The url to navigate to</param>
		public void Navigate(Uri url)
		{
			IsNavigating = true;
			new Thread(() => NavigateInternal(url)).Start();
		}

		/// <summary>
		/// Blocks the thread until IsNavigating is false.
		/// </summary>
		public void WaitUntilReady()
		{
			while(IsNavigating)
				Thread.Sleep(25);
		}

		/// <summary>
		/// Blocks the thread until IsNavigating is false or the specified timeout is reached.
		/// </summary>
		/// <param name="timeout">The maximum number of milliseconds to wait</param>
		/// <returns>True if navigation completed or False if the method ended before navigation completed</returns>
		public bool WaitUntilReady(int timeout)
		{
			var waitUntil = DateTime.Now.AddMilliseconds(timeout);
			while(IsNavigating && DateTime.Now < waitUntil)
				Thread.Sleep(25);
			return IsNavigating;
		}

		void NavigateInternal(Uri url)
		{
			var navhandler = Navigating;
			if(navhandler != null)
				navhandler(this, url);

			try
			{
				var req = new XBrowserRequest(url, XBrowserRequestMethod.Get);
				req.SetCookies(Browser.Cookies);
				req.MaxRedirects = Browser.Config.MaxRedirectsPerRequest;
				req.Timeout = Browser.Config.DefaultRequestTimeout;
				var response = req.Execute();
				Document = new XBrowserDocument(response.Uri, this, response);
			}
			catch(Exception ex)
			{
				var exhandler = NavigateException;
				if(exhandler != null)
					exhandler(this, url, new XBrowserWindowException(this, ex));
				return;
			}
			finally
			{
				IsNavigating = false;
			}

			var endhandler = Navigated;
			if(endhandler != null)
				endhandler(this, url, Document.Url);
		}

		/// <summary>
		/// The main document for the window
		/// </summary>
		public XBrowserDocument Document { get; private set; }
		public XBrowser Browser { get; set; }
		public bool IsNavigating { get; private set; }

		public event XBrowserWindowNavigatingDelegate Navigating;
		public event XBrowserWindowNavigatedDelegate Navigated;
		public event XBrowserWindowNavigateExceptionDelegate NavigateException;
	}

	public delegate void XBrowserWindowNavigateExceptionDelegate(XBrowserWindow window, Uri requestUri, XBrowserWindowException ex);
	public delegate void XBrowserWindowNavigatingDelegate(XBrowserWindow window, Uri requestUri);
	public delegate void XBrowserWindowNavigatedDelegate(XBrowserWindow window, Uri requestUri, Uri responseUri);
}