
namespace XBrowserProject.HtmlDom
{

    /**
     * The Html document body. This element is always present in the DOM API, even 
     * if the tags are not present in the source document. See the BODY element 
     * definition in Html 4.01.
     * <p>See also the <a href='http://www.w3.org/TR/2003/REC-DOM-Level-2-Html-20030109'>Document Object Model (DOM) Level 2 Html Specification</a>.
     */
    internal interface IHTMLBodyElement : IHTMLElement
    {

        /**
         * Color of active links (after mouse-button down, but before mouse-button 
         * up). See the alink attribute definition in Html 4.01. This attribute 
         * is deprecated in Html 4.01.
         */
        string aLink
        {
            get;
            set;
        }

        /**
         * URI [<a href='http://www.ietf.org/rfc/rfc2396.txt'>IETF RFC 2396</a>] of the background texture tile image. See the background attribute 
         * definition in Html 4.01. This attribute is deprecated in Html 4.01.
         */
        string background
        {
            get;
            set;
        }

        /**
         * Document background color. See the bgcolor attribute definition in Html 
         * 4.01. This attribute is deprecated in Html 4.01.
         */
        string bgColor
        {
            get;
            set;
        }

        /**
         * Color of links that are not active and unvisited. See the link 
         * attribute definition in Html 4.01. This attribute is deprecated in 
         * Html 4.01.
         */
        string link
        {
            get;
            set;
        }

        /**
         * Document text color. See the text attribute definition in Html 4.01. 
         * This attribute is deprecated in Html 4.01.
         */
        string text
        {
            get;
            set;
        }

        /**
         * Color of links that have been visited by the user. See the vlink 
         * attribute definition in Html 4.01. This attribute is deprecated in 
         * Html 4.01.
         */
        string vLink
        {
            get;
            set;
        }

    }
}
