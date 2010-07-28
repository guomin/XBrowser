using System;
using XBrowserProject.BrowserModel.Internal.HtmlDom;
using XBrowserProject.HtmlDom;

namespace XBrowserProject.BrowserModel.Internal
{
	/// <summary>
	/// http://www.w3.org/TR/html5/browsers.html#the-windowproxy-object
	/// </summary>
	internal class WindowProxy : IHTMLWindow
	{
		private HtmlDocument _document;
		private IHTMLElement _frameElement;
		private IHTMLWindow _frames;
		private IHTMLHistory _history;
		private int _length;
		private IHTMLLocation _location;

		private IHTMLWindow _opener;

		private IHTMLWindow _parent;
		private IHTMLWindow _self;
		private IHTMLWindow _top;
		private IHTMLWindow _window;

		#region IHTMLWindow Members

		public IHTMLWindow window
		{
			get { return _window; }
		}

		public IHTMLWindow self
		{
			get { return _self; }
		}

		public HtmlDocument document
		{
			get { return _document; }
		}

		public string name { get; set; }

		public IHTMLLocation location
		{
			get { return _location; }
		}

		public IHTMLHistory history
		{
			get { return _history; }
		}

		public void close()
		{
			throw new NotImplementedException();
		}

		public void focus()
		{
			throw new NotImplementedException();
		}

		public void blur()
		{
			throw new NotImplementedException();
		}

		public IHTMLWindow frames
		{
			get { return _frames; }
		}

		public int length
		{
			get { return _length; }
		}

		public IHTMLWindow top
		{
			get { return _top; }
		}

		public IHTMLWindow opener
		{
			get { return _opener; }
		}

		public IHTMLWindow parent
		{
			get { return _parent; }
		}

		public IHTMLElement frameElement
		{
			get { return _frameElement; }
		}

		public IHTMLWindow open(string url, string target, string features, string replace)
		{
			throw new NotImplementedException();
		}

		IHTMLWindow IHTMLWindow.this[int index]
		{
			get { throw new NotImplementedException(); }
		}

		IHTMLWindow IHTMLWindow.this[string name]
		{
			get { throw new NotImplementedException(); }
		}

		public void alert(string message)
		{
			throw new NotImplementedException();
		}

		public bool confirm(string message)
		{
			throw new NotImplementedException();
		}

		public string prompt(string message, string defaultValue)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}