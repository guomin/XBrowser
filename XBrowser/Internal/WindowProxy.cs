using System;
using XBrowserProject.Internal.Html.DOM;
using XBrowserProject.Internal.Html.Interfaces.DOM;

namespace XBrowserProject.Internal
{
	/// <summary>
	/// http://www.w3.org/TR/html5/browsers.html#the-windowproxy-object
	/// </summary>
	internal class WindowProxy : IHTMLWindow
	{
		private readonly BrowsingContext _browsingContext;

		public WindowProxy(BrowsingContext browsingContext)
		{
			_browsingContext = browsingContext;
		}

		private Window GetActiveWindow()
		{
			return _browsingContext.History.Current.Window;
		}

		#region IHTMLWindow Members

		public IHTMLWindow window
		{
			get { return GetActiveWindow().window; }
		}

		public IHTMLWindow self
		{
			get { return GetActiveWindow().self; }
		}

		public HtmlDocument document
		{
			get { return GetActiveWindow().document; }
		}

		public string name
		{
			get { return GetActiveWindow().name; }
			set { GetActiveWindow().name = value; }
		}

		public IHTMLLocation location
		{
			get { return GetActiveWindow().location; }
		}

		public IHTMLHistory history
		{
			get { return GetActiveWindow().history; }
		}

		public void close()
		{
			GetActiveWindow().close();
		}

		public void focus()
		{
			GetActiveWindow().focus();
		}

		public void blur()
		{
			GetActiveWindow().blur();
		}

		public IHTMLWindow frames
		{
			get { return GetActiveWindow().frames; }
		}

		public int length
		{
			get { return GetActiveWindow().length; }
		}

		public IHTMLWindow top
		{
			get { return GetActiveWindow().top; }
		}

		public IHTMLWindow opener
		{
			get { return GetActiveWindow().opener; }
		}

		public IHTMLWindow parent
		{
			get { return GetActiveWindow().parent; }
		}

		public IHTMLElement frameElement
		{
			get { return GetActiveWindow().frameElement; }
		}

		public IHTMLWindow open(string url, string target, string features, string replace)
		{
			return GetActiveWindow().open(url, target, features, replace);
		}

		IHTMLWindow IHTMLWindow.this[int index]
		{
			get { return GetActiveWindow()[index]; }
		}

		IHTMLWindow IHTMLWindow.this[string name]
		{
			get { return GetActiveWindow()[name]; }
		}

		public void alert(string message)
		{
			GetActiveWindow().alert(message);
		}

		public bool confirm(string message)
		{
			return GetActiveWindow().confirm(message);
		}

		public string prompt(string message, string defaultValue)
		{
			return GetActiveWindow().prompt(message, defaultValue);
		}

		#endregion
	}
}