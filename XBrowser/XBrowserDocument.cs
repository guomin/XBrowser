using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using XBrowserProject.Html;
using XBrowserProject.Internal;
using XBrowserProject.Internal.Html.DOM;
using XBrowserProject.Internal.Html.DOM.Parsing;

namespace XBrowserProject
{
	public class XBrowserDocument
	{
		private readonly HtmlDocument _document;

		internal XBrowserDocument( HtmlDocument document)
		{
			_document = document;
		}

		/// <summary>
		/// Gets the parent window object to which the document belongs
		/// </summary>
		public WindowContainer Window { get; private set; }

		/// <summary>
		/// Gets the root element for the document. This is usually an XBrowserHtmlElement instance.
		/// </summary>
		public XBodyElement Body { get; private set; }

		/// <summary>
		/// Gets or sets the URL of the current page. This value is also used internally by XBrowser for determining
		/// the location of the current page, such as for setting the "Referer" heading. Manually setting this value
		/// does not cause the browser to navigate.
		/// </summary>
		public Uri Url { get { return null; } }

		///// <summary>
		///// Gets the document title for the current page. If no page has been navigated to, or the current page does
		///// not have a title tag, an empty string is returned.
		///// </summary>
		///// <remarks>Spec: http://dev.w3.org/html5/spec/Overview.html#document.title</remarks>
		//public string Title { get { return ((XDocument.Descendants().FirstOrDefault(e => e.Name.LocalName.ToLower() == "title") ?? new XElement("title")).Value ?? "").Trim(); } }
	}
}