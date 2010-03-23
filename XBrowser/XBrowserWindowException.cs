using System;

namespace AxeFrog.Net
{
	public class XBrowserWindowException : Exception
	{
		public XBrowserWindow Window { get; set; }

		internal XBrowserWindowException(XBrowserWindow window, string message) : base(message)
		{
			Window = window;
		}

		internal XBrowserWindowException(XBrowserWindow window, string message, Exception ex) : base(message, ex)
		{
			Window = window;
		}

		internal XBrowserWindowException(XBrowserWindow window, Exception ex) : base(ex.Message, ex)
		{
			Window = window;
		}
	}
}