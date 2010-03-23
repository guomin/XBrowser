using System;
using System.Linq;
using System.Xml.Linq;
using AxeFrog.Net.Html;

namespace AxeFrog.Net
{
	public class XBrowserDocument
	{
		private readonly XBrowserResponse _response;
		private readonly XDocument _doc;
		private XBrowserElement _rootElement;

		internal XBrowserDocument(Uri url, XBrowserWindow window, XBrowserResponse response)
		{
			Url = url;
			Window = window;
			_response = response;
			try
			{
				_doc = HtmlParser.SanitizeHtml(response.ResponseText);
			}
			catch(HtmlParserException)
			{
				_doc = HtmlParser.CreateBlankHtmlDocument();
			}
			
			if(_doc.Root == null)
				_rootElement = new XBrowserHtmlElement(this, new XElement("html"));
			else
			{
				if(_doc.Root.Name.LocalName.ToLower() != "html")
					if(window.Browser.Config.AllowNonConformingDocumentStructure)
						_rootElement = XBrowserElement.Create(this, _doc.Root);
					else
						_rootElement = new XBrowserHtmlElement(this, new XElement("html"));
				else
					_rootElement = new XBrowserHtmlElement(this, _doc.Root);
			}
		}

		public XBrowserResponse Response { get { return _response; } }
		public XDocument XDocument { get { return _doc; } }
		public XBrowserWindow Window { get; set; }

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