using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace XBrowserProject
{
	public class XBrowser
	{
		private List<WindowContainer> _windows = new List<WindowContainer>();

		public XBrowser()
		{
			Config = new XBrowserConfiguration(this);
			Cookies = new CookieStore();
		}

		/// <summary>
		/// Gets a list of the current active browser windows/tabs controlled by this browser instance
		/// </summary>
		public IEnumerable<WindowContainer> Windows { get { return _windows.AsReadOnly(); } }

			/// <summary>
		/// Gets the configuration for the current XBrowser instance
		/// </summary>
		public XBrowserConfiguration Config { get; private set; }

		/// <summary>
		/// Gets the cookie cache for the browser
		/// </summary>
		public CookieStore Cookies { get; private set; }

		/// <summary>
		/// Gets the version string for the current release, in the format [major-release-number].[minor-release-number].
		/// Not intended to be read as a decimal. e.g. 1.11 would be a greater version than 1.1.
		/// </summary>
		public const string Version = "0.1";

		/// <summary>
		/// Creates a new top-level "window" and stores it in this browser instance. To remove it from the browser, call Close() on the window.
		/// </summary>
		/// <returns>The new XBrowserWindow instance</returns>
		public WindowContainer CreateWindow()
		{
			var win = new WindowContainer(this);
			_windows.Add(win);
			return win;
		}
	}
}
