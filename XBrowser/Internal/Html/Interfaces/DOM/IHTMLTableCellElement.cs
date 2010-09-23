
namespace XBrowserProject.Internal.Html.Interfaces.DOM
{
    /**
     * The object used to represent the <code>TH</code> and <code>TD</code> 
     * elements. See the TD element definition in Html 4.01.
     * <p>See also the <a href='http://www.w3.org/TR/2003/REC-DOM-Level-2-Html-20030109'>Document Object Model (DOM) Level 2 Html Specification</a>.
     */
    internal interface IHTMLTableCellElement : IHTMLElement
    {
        /**
         * Abbreviation for header cells. See the abbr attribute definition in 
         * Html 4.01.
         */
        string abbr { get; set; }

        /**
         * Horizontal alignment of data in cell. See the align attribute definition
         *  in Html 4.01.
         */
        string align { get; set; }

        /**
         * Names group of related headers. See the axis attribute definition in 
         * Html 4.01.
         */
        string axis { get; set; }

        /**
         * Cell background color. See the bgcolor attribute definition in Html 
         * 4.01. This attribute is deprecated in Html 4.01.
         */
        string bgColor { get; set; }

        /**
         * The index of this cell in the row, starting from 0. This index is in 
         * document tree order and not display order.
         */
        int cellIndex { get; }

        /**
         * Alignment character for cells in a column. See the char attribute 
         * definition in Html 4.01.
         */
        string ch { get; set; }

        /**
         * Offset of alignment character. See the charoff attribute definition in 
         * Html 4.01.
         */
        string chOff { get; set; }

        /**
         * Number of columns spanned by cell. See the colspan attribute definition 
         * in Html 4.01.
         */
        int colSpan { get; set; }

        /**
         * List of <code>id</code> attribute values for header cells. See the 
         * headers attribute definition in Html 4.01.
         */
        string headers { get; set; }

        /**
         * Cell height. See the height attribute definition in Html 4.01. This 
         * attribute is deprecated in Html 4.01.
         */
        string height { get; set; }

        /**
         * Suppress word wrapping. See the nowrap attribute definition in Html 
         * 4.01. This attribute is deprecated in Html 4.01.
         */
        bool noWrap { get; set; }

        /**
         * Number of rows spanned by cell. See the rowspan attribute definition in 
         * Html 4.01.
         */
        int rowSpan { get; set; }

        /**
         * Scope covered by header cells. See the scope attribute definition in 
         * Html 4.01.
         */
        string scope { get; set; }

        /**
         * Vertical alignment of data in cell. See the valign attribute definition 
         * in Html 4.01.
         */
        string vAlign { get; set; }

        /**
         * Cell width. See the width attribute definition in Html 4.01. This 
         * attribute is deprecated in Html 4.01.
         */
        string width { get; set; }
    }
}
