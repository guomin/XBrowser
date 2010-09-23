
namespace XBrowserProject.Internal.Html.Interfaces.DOM
{
    internal interface IHTMLTableColElement : IHTMLElement
    {

        /**
         * Horizontal alignment of cell data in column. See the align attribute 
         * definition in Html 4.01.
         */
        string align { get; set; }

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
         * Indicates the number of columns in a group or affected by a grouping. 
         * See the span attribute definition in Html 4.01.
         */
        int span { get; set; }

        /**
         * Vertical alignment of cell data in column. See the valign attribute 
         * definition in Html 4.01.
         */
        string vAlign { get; set; }

        /**
         * Default column width. See the width attribute definition in Html 4.01.
         */
        string width { get; set; }
    }
}
