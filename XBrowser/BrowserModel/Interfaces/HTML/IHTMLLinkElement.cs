using System;
namespace XBrowserProject.HtmlDom
{
    public interface IHTMLLinkElement : IHTMLElement
    {
        /**
         * The character encoding of the resource being linked to. See the charset 
         * attribute definition in Html 4.01.
         */
        string charset { get; set; }

        /**
         * Enables/disables the link. This is currently only used for style sheet 
         * links, and may be used to activate or deactivate style sheets. 
         */
        bool disabled { get; set; }

        /**
         * The URI [<a href='http://www.ietf.org/rfc/rfc2396.txt'>IETF RFC 2396</a>] of the linked resource. See the href attribute definition in 
         * Html 4.01.
         */
        string href { get; set; }

        /**
         * Language code of the linked resource. See the hreflang attribute 
         * definition in Html 4.01.
         */
        string hreflang { get; set; }

        /**
         * Designed for use with one or more target media. See the media attribute 
         * definition in Html 4.01.
         */
        string media { get; set; }

        /**
         * Forward link type. See the rel attribute definition in Html 4.01.
         */
        string rel { get; set; }

        /**
         * Reverse link type. See the rev attribute definition in Html 4.01.
         */
        string rev { get; set; }

        /**
         * Frame to render the resource in. See the target attribute definition in 
         * Html 4.01.
         */
        string target { get; set; }

        /**
         * Advisory content type. See the type attribute definition in Html 4.01.
         */
        string type { get; set; }
    }
}
