namespace XBrowserProject.Internal.Html.Interfaces.DOM
{
    internal interface IHTMLBaseElement : IHTMLElement
    {
        /**
         * The URI [<a href='http://www.ietf.org/rfc/rfc2396.txt'>IETF RFC 2396</a>] of the linked resource. See the href attribute definition in 
         * Html 4.01.
         */
        string href { get; set; }

        /**
         * Frame to render the resource in. See the target attribute definition in 
         * Html 4.01.
         */
        string target { get; set; }
    }
}
