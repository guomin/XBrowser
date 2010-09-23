namespace XBrowserProject.Internal.Html.Interfaces.DOM
{
    internal interface IHTMLHRElement : IHTMLElement
    {
        /**
         * Align the rule on the page. See the align attribute definition in Html 
         * 4.01. This attribute is deprecated in Html 4.01.
         */
        string align { get; set; }

        /**
         * Indicates to the user agent that there should be no shading in the 
         * rendering of this element. See the noshade attribute definition in 
         * Html 4.01. This attribute is deprecated in Html 4.01.
         */
        bool noShade { get; set; }

        /**
         * The height of the rule. See the size attribute definition in Html 4.01. 
         * This attribute is deprecated in Html 4.01.
         */
        string size { get; set; }

        /**
         * The width of the rule. See the width attribute definition in Html 4.01. 
         * This attribute is deprecated in Html 4.01.
         */
        string width { get; set; }
    }
}
