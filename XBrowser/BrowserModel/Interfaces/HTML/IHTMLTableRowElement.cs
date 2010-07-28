
namespace XBrowserProject.HtmlDom
{
    public interface IHTMLTableRowElement : IHTMLElement
    {
        /**
         * Horizontal alignment of data within cells of this row. See the align 
         * attribute definition in Html 4.01.
         */
        string align { get; set; }

        /**
         * Background color for rows. See the bgcolor attribute definition in Html 
         * 4.01. This attribute is deprecated in Html 4.01.
         */
        string bgColor { get; set; }

        /**
         * The collection of cells in this row. 
         * @version DOM Level 2
         */
        IHTMLCollection cells { get; }

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
         * Delete a cell from the current row.
         * @param index The index of the cell to delete, starting from 0. If the 
         *   index is -1 the last cell in the row is deleted.
         * @exception DOMException
         *   INDEX_SIZE_ERR: Raised if the specified <code>index</code> is greater 
         *   than or equal to the number of cells or if the index is a negative 
         *   number other than -1.
         * @version DOM Level 2
         */
        void deleteCell(int index);

        /**
         * Insert an empty <code>TD</code> cell into this row. If 
         * <code>index</code> is -1 or equal to the number of cells, the new 
         * cell is appended.
         * @param index The place to insert the cell, starting from 0.
         * @return The newly created cell.
         * @exception DOMException
         *   INDEX_SIZE_ERR: Raised if the specified <code>index</code> is greater 
         *   than the number of cells or if the index is a negative number other 
         *   than -1.
         * @version DOM Level 2
         */
        IHTMLElement insertCell(int index);

        /**
         * This is in logical order and not in document order. The 
         * <code>rowIndex</code> does take into account sections (
         * <code>THEAD</code>, <code>TFOOT</code>, or <code>TBODY</code>) within 
         * the table, placing <code>THEAD</code> rows first in the index, 
         * followed by <code>TBODY</code> rows, followed by <code>TFOOT</code> 
         * rows.
         * @version DOM Level 2
         */
        int rowIndex { get; }

        /**
         * The index of this row, relative to the current section (
         * <code>THEAD</code>, <code>TFOOT</code>, or <code>TBODY</code>), 
         * starting from 0.
         * @version DOM Level 2
         */
        int sectionRowIndex { get; }

        /**
         * Vertical alignment of data within cells of this row. See the valign 
         * attribute definition in Html 4.01.
         */
        string vAlign { get; set; }
    }
}
