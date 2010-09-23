using System;

namespace XBrowserProject
{
	public class XBrowserWindowException : Exception
	{
		public WindowContainer Window { get; set; }

		internal XBrowserWindowException(WindowContainer window, string message) : base(message)
		{
			Window = window;
		}

		internal XBrowserWindowException(WindowContainer window, string message, Exception ex) : base(message, ex)
		{
			Window = window;
		}

		internal XBrowserWindowException(WindowContainer window, Exception ex) : base(ex.Message, ex)
		{
			Window = window;
		}
	}
}