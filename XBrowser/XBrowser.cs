﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace AxeFrog.Net
{
	public class XBrowser : IEnumerable<XBrowserWindow>
	{
		private List<XBrowserWindow> _windows = new List<XBrowserWindow>();

		public XBrowser()
		{
			Config = new XBrowserConfiguration(this);
			Cookies = new XBrowserCookieStore();
		}

		public IEnumerator<XBrowserWindow> GetEnumerator() { return _windows.GetEnumerator(); }
		IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

		/// <summary>
		/// Gets the configuration for the current XBrowser instance
		/// </summary>
		public XBrowserConfiguration Config { get; private set; }

		/// <summary>
		/// Gets the cookie cache for the browser
		/// </summary>
		public XBrowserCookieStore Cookies { get; private set; }

		/// <summary>
		/// Gets the version string for the current release, in the format [major-release-number].[minor-release-number].
		/// Not intended to be read as a decimal. e.g. 1.11 would be a greater version than 1.1.
		/// </summary>
		public string Version { get { return "0.1"; } }

		/// <summary>
		/// Creates a new top-level "window" and stores it in this browser instance. To remove it from the browser, call Close() on the window.
		/// </summary>
		/// <returns>The new XBrowserWindow instance</returns>
		public XBrowserWindow CreateWindow()
		{
			var win = new XBrowserWindow(this);
			_windows.Add(win);
			return win;
		}
	}
}