﻿namespace XBrowserProject.Internal.Html.Interfaces.DOM
{
    internal interface IHTMLHeadingElement : IHTMLElement
    {
        /**
         * Horizontal text alignment. See the align attribute definition in Html 
         * 4.01. This attribute is deprecated in Html 4.01.
         */
        string align { get; set; }
    }
}
