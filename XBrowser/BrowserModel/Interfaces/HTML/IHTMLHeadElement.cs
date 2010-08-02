using System.Xml;
namespace XBrowserProject.HtmlDom
{

    /**
     * Document head information. See the HEAD element definition in Html 4.01.
     * <p>See also the <a href='http://www.w3.org/TR/2003/REC-DOM-Level-2-Html-20030109'>Document Object Model (DOM) Level 2 Html Specification</a>.
     */
    internal interface IHTMLHeadElement : IHTMLElement
    {
        /**
         * URI [<a href='http://www.ietf.org/rfc/rfc2396.txt'>IETF RFC 2396</a>] designating a metadata profile. See the profile attribute 
         * definition in Html 4.01.
         */
        string profile
        {
            get;
            set;
        }

    }

}