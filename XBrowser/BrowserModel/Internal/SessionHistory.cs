namespace XBrowserProject.BrowserModel.Internal
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
	}
}