using System;
using System.Collections.Generic;
using XBrowserProject.Internal.Html.DOM;
using XBrowserProject.Internal.Html.DOM.Parsing;
using XBrowserProject.Internal.Html.Interfaces.DOM;

namespace XBrowserProject.Internal
{
    internal class History : IHTMLHistory
    {
    	public class Entry
		{
			public Uri Url { get; set; }
			public object State { get; set; }
			public HtmlDocument Document { get; set; }
			public Window Window { get; set; }

    		private string _title;
			public string Title
			{
				set { _title = value; }
				get { return _title ?? Document.title ?? (Url != null ? Url.ToString() : ""); }
			}
		}

		private List<Entry> _entries = new List<Entry>();
		private int _current;

		public History()
		{
			var url = new Uri("about:blank");
			var window = new Window(url);
			var firstEntry = new Entry
			{
				Url = url,
				Window = window,
				Document = window.document
			};
			_current = 0;
			_entries.Add(firstEntry);
		}

		public Entry Current { get { return _entries[_current]; } }

		public int length
        {
            get { throw new NotImplementedException(); }
        }

        public void go(int delta)
        {
            throw new NotImplementedException();
        }

        public void back()
        {
            throw new NotImplementedException();
        }

        public void forward()
        {
            throw new NotImplementedException();
        }

        public void pushState(object data, string title, string url)
        {
            throw new NotImplementedException();
        }

        public void replaceState(object data, string title, string url)
        {
            throw new NotImplementedException();
        }
    }
}
