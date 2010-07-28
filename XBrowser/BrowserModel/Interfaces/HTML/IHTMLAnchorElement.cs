

namespace XBrowserProject.HtmlDom
{
    public interface IHTMLAnchorElement : IHTMLElement
    {
        /**
         * A single character access key to give access to the form control. See 
         * the accesskey attribute definition in Html 4.01.
         */
        string accessKey { get; set; }

        /**
         * The character encoding of the linked resource. See the charset 
         * attribute definition in Html 4.01.
         */
        string charset { get; set; }

        /**
         * Comma-separated list of lengths, defining an active region geometry. 
         * See also <code>shape</code> for the shape of the region. See the 
         * coords attribute definition in Html 4.01.
         */
        string coords { get; set; }

        /**
         * The absolute URI [<a href='http://www.ietf.org/rfc/rfc2396.txt'>IETF RFC 2396</a>] of the linked resource. See the href attribute 
         * definition in Html 4.01.
         */
        string href { get; set; }

        /**
         * Language code of the linked resource. See the hreflang attribute 
         * definition in Html 4.01.
         */
        string hreflang { get; set; }

        /**
         * Anchor name. See the name attribute definition in Html 4.01.
         */
        string name { get; set; }

        /**
         * Forward link type. See the rel attribute definition in Html 4.01.
         */
        string rel { get; set; }

        /**
         * Reverse link type. See the rev attribute definition in Html 4.01.
         */
        string rev { get; set; }

        /**
         * The shape of the active area. The coordinates are given by 
         * <code>coords</code>. See the shape attribute definition in Html 4.01.
         */
        string shape { get; set; }

        /**
         * Index that represents the element'coreRule position in the tabbing order. See 
         * the tabindex attribute definition in Html 4.01.
         */
        int tabIndex { get; set; }

        /**
         * Frame to render the resource in. See the target attribute definition in 
         * Html 4.01.
         */
        string target { get; set; }

        /**
         * Advisory content type. See the type attribute definition in Html 4.01.
         */
        string type { get; set; }

        /**
         * Removes keyboard focus from this element.
         */
        void blur();

        /**
         * Gives keyboard focus to this element.
         */
        void focus();
    }
}
