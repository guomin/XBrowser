﻿namespace XBrowserProject.Internal.Html.Interfaces.DOM
{
    internal interface IHTMLBRElement : IHTMLElement
    {
        /**
         * Control flow of text around floats. See the clear attribute definition 
         * in Html 4.01. This attribute is deprecated in Html 4.01.
         */
        string clear { get; set; }
    }
}
