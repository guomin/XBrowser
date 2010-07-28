using XBrowserProject.BrowserModel.Internal.HtmlDom;

namespace XBrowserProject.BrowserModel.Internal
{
	/// <summary>
	/// A browsing context is an environment in which Document objects are presented to the user. For more information, see http://www.w3.org/TR/html5/browsers.html#windows
	/// </summary>
	internal class BrowsingContext
	{
		public WindowProxy Window { get; private set; }
		public SessionHistory History { get; private set; }

		public HtmlDocument Document
		{
			get { return Window.document; }
		}
	}
}