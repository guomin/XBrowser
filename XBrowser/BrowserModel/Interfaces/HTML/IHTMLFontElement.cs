using System.Xml;
namespace XBrowserProject.HtmlDom
{

    /**
     * Local change to font. See the FONT element definition in Html 4.01. This 
     * element is deprecated in Html 4.01.
     * <p>See also the <a href='http://www.w3.org/TR/2003/REC-DOM-Level-2-Html-20030109'>Document Object Model (DOM) Level 2 Html Specification</a>.
     */
    internal interface IHTMLFontElement : IHTMLElement
    {
        /**
         * Font color. See the color attribute definition in Html 4.01. This 
         * attribute is deprecated in Html 4.01.
         */
        string color
        {
            get;
            set;
        }

        /**
         * Font face identifier. See the face attribute definition in Html 4.01. 
         * This attribute is deprecated in Html 4.01.
         */
        string face
        {
            get;
            set;
        }

        /**
         * Font size. See the size attribute definition in Html 4.01. This 
         * attribute is deprecated in Html 4.01.
         */
        int size
        {
            get;
            set;
        }

    }

}