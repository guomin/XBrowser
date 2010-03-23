using System;

namespace AxeFrog.Net
{
	public class XBrowserConfiguration
	{
		internal XBrowserConfiguration(XBrowser browser)
		{
			UserAgent = string.Concat("AxeFrog.Net.XBrowser (r", browser.Version, ")");
			MaxRedirectsPerRequest = 10;
			DefaultRequestTimeout = 30000;
			AllowNonConformingDocumentStructure = true;
		}

		/// <summary>
		/// Gets or sets the user agent string that XBrowser will send during web requests. The default is "AxeFrog.Net.XBrowser".
		/// </summary>
		public string UserAgent { get; set; }

		/// <summary>
		/// Gets or sets the maximum number of http 301/302 redirects allowed in a single request. The default is 10.
		/// </summary>
		public int MaxRedirectsPerRequest { get; set; }

		/// <summary>
		/// Gets or sets the default timeout for requests (in milliseconds). This timeout is applied per individual web request,
		/// even if multiple redirects are involved; therefore it is possible that the overall request as a group may take longer
		/// than the value specified here. The default is 30000 (30 seconds).
		/// </summary>
		public int DefaultRequestTimeout { get; set; }

		/// <summary>
		/// Forces the parsed document structure to conform to the document type specification. At present, all documents are
		/// read according to the HTML5 specification; a future version of XBrowser may adjust the way documents are read according
		/// to the DOCTYPE for the given page. The default value for this property is true.
		/// </summary>
		public bool AllowNonConformingDocumentStructure { get; set; }
	}
}