using XBrowserProject.Internal.Html.DOM;

namespace XBrowserProject.Internal
{
	/// <summary>
	/// A browsing context is an environment in which Document objects are presented to the user. For more information, see http://www.w3.org/TR/html5/browsers.html#windows
	/// </summary>
	internal class BrowsingContext
	{
		public BrowsingContext()
		{
			Window = new WindowProxy(this);
		}

		public BrowsingContext(BrowsingContext parentBrowsingContext)
		{
		}

		public WindowProxy Window { get; private set; }
		public History History { get; private set; }
	}
}