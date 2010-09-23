using System;
using System.Windows.Forms;

namespace XBrowserProject.Internal
{
	/// <summary>
	/// http://www.w3.org/TR/html5/history.html#history
	/// </summary>
	internal class SessionHistory
	{
		public int Index { get; set; }
		public int Count { get; set; }
		public SessionHistoryItem Current { get; set; }
	}

	/// <summary>
	/// http://www.w3.org/TR/html5/history.html#session-history-entry
	/// </summary>
	internal class SessionHistoryItem
	{
		public Uri Url { get; set; }
		public SessionHistoryState State { get; set; }
		public HtmlDocument Document { get; set; }
	}

	internal class SessionHistoryState
	{
	}
}