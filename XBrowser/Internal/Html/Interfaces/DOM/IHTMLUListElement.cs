
namespace XBrowserProject.Internal.Html.Interfaces.DOM
{
    /**
     * Unordered list. See the UL element definition in Html 4.01.
     * <p>See also the <a href='http://www.w3.org/TR/2003/REC-DOM-Level-2-Html-20030109'>Document Object Model (DOM) Level 2 Html Specification</a>.
     */
    internal interface IHTMLUListElement : IHTMLElement
    {
        /**
         * Reduce spacing between list items. See the compact attribute definition 
         * in Html 4.01. This attribute is deprecated in Html 4.01.
         */
        bool compact
        {
            get;
            set;
        }

        /**
         * Bullet style. See the type attribute definition in Html 4.01. This 
         * attribute is deprecated in Html 4.01.
         */
        string type
        {
            get;
            set;
        }

    }

}