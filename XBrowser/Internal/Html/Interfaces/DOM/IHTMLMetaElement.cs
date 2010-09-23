namespace XBrowserProject.Internal.Html.Interfaces.DOM
{
    internal interface IHTMLMetaElement : IHTMLElement
    {
        /**
         * Associated information. See the content attribute definition in Html 
         * 4.01.
         */
        string content { get; set; }

        /**
         * HTTP response header name [<a href='http://www.ietf.org/rfc/rfc2616.txt'>IETF RFC 2616</a>]. See the http-equiv attribute definition in 
         * Html 4.01.
         */
        string httpEquiv { get; set; }

        /**
         * Meta information name. See the name attribute definition in Html 4.01.
         */
        string name { get; set; }

        /**
         * Select form of content. See the scheme attribute definition in Html 
         * 4.01.
         */
        string scheme { get; set; }
    }
}
