
namespace XBrowserProject.HtmlDom
{

    /**
     *  CSSStyle information. See the STYLE element definition in Html 4.01, the CSS 
     * module [<a href='http://www.w3.org/TR/2000/REC-DOM-Level-2-CSSStyle-20001113'>DOM Level 2 CSSStyle Sheets and CSS</a>] and the <code>LinkStyle</code> class in the StyleSheets 
     * module [<a href='http://www.w3.org/TR/2000/REC-DOM-Level-2-CSSStyle-20001113'>DOM Level 2 CSSStyle Sheets and CSS</a>]. 
     * <p>See also the <a href='http://www.w3.org/TR/2003/REC-DOM-Level-2-Html-20030109'>Document Object Model (DOM) Level 2 Html Specification</a>.
     */
    public interface IHTMLStyleElement : IHTMLElement
    {
        /**
         * Enables/disables the style sheet. 
         */
        bool disabled
        {
            get;
            set;
        }

        /**
         * Designed for use with one or more target media. See the media attribute 
         * definition in Html 4.01.
         */
        string media
        {
            get;
            set;
        }

        /**
         * The content type of the style sheet language. See the type attribute 
         * definition in Html 4.01.
         */
        string type
        {
            get;
            set;
        }

    }

}