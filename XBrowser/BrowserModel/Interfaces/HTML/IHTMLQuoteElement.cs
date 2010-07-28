using System;
namespace XBrowserProject.HtmlDom
{
    public interface IHTMLQuoteElement : IHTMLElement
    {
        /**
         * Horizontal text alignment. See the cite attribute definition in Html 
         * 4.01. This attribute is deprecated in Html 4.01.
         */
        string cite { get; set; }
    }
}
