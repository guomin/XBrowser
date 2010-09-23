
namespace XBrowserProject.Internal.Html.Interfaces.DOM
{

    /**
     * Preformatted text. See the PRE element definition in Html 4.01.
     * <p>See also the <a href='http://www.w3.org/TR/2003/REC-DOM-Level-2-Html-20030109'>Document Object Model (DOM) Level 2 Html Specification</a>.
     */
    internal interface IHTMLPreElement : IHTMLElement
    {
        /**
         * Width. See the width attribute definition in Html 
         * 4.01. This attribute is deprecated in Html 4.01.
         */
        int width
        {
            get;
            set;
        }

    }

}