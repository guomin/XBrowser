﻿namespace XBrowserProject.Internal.Html.Interfaces.DOM
{
    internal interface IHTMLOListElement : IHTMLElement
    {
        /**
         * Reduce spacing between list items. See the compact attribute definition 
         * in Html 4.01. This attribute is deprecated in Html 4.01.
         */
        bool compact { get; set; }

        /**
         * Starting sequence number. See the start attribute definition in Html 
         * 4.01. This attribute is deprecated in Html 4.01.
         */
        int start { get; set; }

        /**
         * Numbering style. See the type attribute definition in Html 4.01. This 
         * attribute is deprecated in Html 4.01.
         */
        string type { get; set; }
    }
}
