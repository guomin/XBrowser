using System;
using XBrowserProject.Internal;

namespace XBrowserProject
{
	/// <summary>
	/// A window represents a container encapsulating a browsing session with a navigation history.
	/// </summary>
	public class WindowContainer
	{
		internal BrowsingContext BrowsingContext { get; set; }

		internal WindowContainer(XBrowser browser)
		{
			Browser = browser;
			BrowsingContext = new BrowsingContext();
		}

		internal WindowContainer(WindowContainer parentWindow)
		{
			Browser = parentWindow.Browser;
			BrowsingContext = new BrowsingContext(parentWindow.BrowsingContext);
		}

		/// <summary>
		/// Navigates to the specified url.
		/// </summary>
		/// <param name="url">The url to navigate to</param>
		/// <param name="block">If true, returns when navigation is complete, otherwise returns immediately. To be notified of completion
		/// for non-blocking calls, subscribe to Navigated or NavigateException, or call WaitUntilReady().</param>
		public void Navigate(string url, bool block = false)
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
			Navigate(uri, block);
		}

		/// <summary>
		/// Navigates to the specified url.
		/// </summary>
		/// <param name="url">The url to navigate to</param>
		/// <param name="block">If true, returns when navigation is complete, otherwise returns immediately. To be notified of completion
		/// for non-blocking calls, subscribe to Navigated or NavigateException, or call WaitUntilReady().</param>
		public void Navigate(Uri url, bool block = false)
		{
			IsNavigating = true;
			//new Thread(() => NavigateInternal(url)).Start();
			//if(block)
			//    WaitUntilReady();
		}

		/// <summary>
		/// Blocks the thread until IsNavigating is false.
		/// </summary>
		public void WaitUntilReady()
		{
			//while(IsNavigating)
			//    Thread.Sleep(25);
			
		}

		/// <summary>
		/// Blocks the thread until IsNavigating is false or the specified timeout is reached.
		/// </summary>
		/// <param name="timeout">The maximum number of milliseconds to wait</param>
		/// <returns>True if navigation completed or False if the method ended before navigation completed</returns>
		public bool WaitUntilReady(int timeout)
		{
			//var waitUntil = DateTime.Now.AddMilliseconds(timeout);
			//while(IsNavigating && DateTime.Now < waitUntil)
			//    Thread.Sleep(25);
			return IsNavigating;
		}

		void NavigateInternal(Uri url)
		{
			var navhandler = Navigating;
			if(navhandler != null)
				navhandler(this, url);

			try
			{
				var req = new Request(url, XBrowserRequestMethod.Get);
				req.SetCookies(Browser.Cookies);
				req.MaxRedirects = Browser.Config.MaxRedirectsPerRequest;
				req.Timeout = Browser.Config.DefaultRequestTimeout;
				var response = req.Execute();
				throw new NotImplementedException();
				//Document = new XBrowserDocument(response.Uri, this, response.ResponseText);
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

	public delegate void XBrowserWindowNavigateExceptionDelegate(WindowContainer window, Uri requestUri, XBrowserWindowException ex);
	public delegate void XBrowserWindowNavigatingDelegate(WindowContainer window, Uri requestUri);
	public delegate void XBrowserWindowNavigatedDelegate(WindowContainer window, Uri requestUri, Uri responseUri);
}