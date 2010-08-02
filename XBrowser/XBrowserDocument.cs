using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using XBrowserProject.Html;

namespace XBrowserProject
{
	public class XBrowserDocument
	{
		internal XBrowserDocument(Uri url, XBrowserWindow window, XBrowserResponse response)
		{
			Url = url;
			Window = window;
			Response = response;
			//XDocument = (response.ResponseText ?? "").ParseHtml();
			
			if(XDocument.Root == null)
			    RootElement = new XHtmlElement(this, new XElement("html"));
			else
			{
			    if(XDocument.Root.Name.LocalName.ToLower() != "html")
			        if(window.Browser.Config.AllowNonConformingDocumentStructure)
			            RootElement = XBrowserElement.Create(this, XDocument.Root);
			        else
			            RootElement = new XHtmlElement(this, new XElement("html"));
			    else
			        RootElement = new XHtmlElement(this, XDocument.Root);
			}
		}

		/// <summary>
		/// Gets the response object that was returned as a result of the request that generated this XBrowserDocument instance
		/// </summary>
		public XBrowserResponse Response { get; private set; }

		/// <summary>
		/// Gets the underlying XDocument that represents the originally-parsed HTML document structure
		/// </summary>
		public XDocument XDocument { get; private set; }

		/// <summary>
		/// Gets the parent window object to which the document belongs
		/// </summary>
		public XBrowserWindow Window { get; private set; }

		/// <summary>
		/// Gets the root element for the document. This is usually an XBrowserHtmlElement instance.
		/// </summary>
		public XBrowserElement RootElement { get; private set; }

		/// <summary>
		/// Gets or sets the URL of the current page. This value is also used internally by XBrowser for determining
		/// the location of the current page, such as for setting the "Referer" heading. Manually setting this value
		/// does not cause the browser to navigate.
		/// </summary>
		public Uri Url { get; set; }

		/// <summary>
		/// Gets the document title for the current page. If no page has been navigated to, or the current page does
		/// not have a title tag, an empty string is returned.
		/// </summary>
		/// <remarks>Spec: http://dev.w3.org/html5/spec/Overview.html#document.title</remarks>
		public string Title { get { return ((XDocument.Descendants().FirstOrDefault(e => e.Name.LocalName.ToLower() == "title") ?? new XElement("title")).Value ?? "").Trim(); } }

	}
}