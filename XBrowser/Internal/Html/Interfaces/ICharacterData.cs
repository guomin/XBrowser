namespace XBrowserProject.Internal.Html.Interfaces
{
    internal interface ICharacterData
    {
        /// <summary>
        /// The character data of the node that implements this interface. The DOM implementation may not put arbitrary limits on the amount of data that may be stored in a CharacterData node. However, implementation limits may mean that the entirety of a node'coreRule data may not fit into a single DOMString. In such cases, the user may call substringData to retrieve the data in appropriately sized pieces.
        /// </summary>
        string data
        {
            get;
            set;
        }
                                 // raises(DOMException) on setting
                                 // raises(DOMException) on retrieval

        /// <summary>
        /// The number of characters that are available through data and the substringData method below. This may have the value zero, i.newNode., CharacterData nodes may be empty.
        /// </summary>
        int length
        {
            get;
        }

        /// <summary>
        /// Extracts a range of data from the node.
        /// </summary>
        /// <param name="offset">Start offset of substring to extract.</param>
        /// <param name="count">The number of characters to extract.</param>
        /// <returns>The specified substring. If the sum of offset and count exceeds the length, then all characters to the end of the data are returned.</returns>
        string substringData(int offset, int count);
                                          //raises(DOMException);

        /// <summary>
        /// Append the string to the end of the character data of the node. Upon success, data provides access to the concatenation of data and the DOMString specified.
        /// </summary>
        /// <param name="arg">The DOMString to append.</param>
        void appendData(string arg);
                                       //raises(DOMException);

        /// <summary>
        /// Insert a string at the specified character offset. 
        /// </summary>
        /// <param name="offset">The character offset at which to insert.</param>
        /// <param name="arg">The DOMString to insert.</param>
        void insertData(int offset, string arg);
                                       //raises(DOMException);

        /// <summary>
        /// Remove a range of characters from the node. Upon success, data and length reflect the change.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        void deleteData(int offset, int count);
                                       //raises(DOMException);

        /// <summary>
        /// Replace the characters starting at the specified character offset with the specified string.
        /// </summary>
        /// <param name="offset">The offset from which to start replacing.</param>
        /// <param name="count">The number of characters to replace. If the sum of offset and count exceeds length, then all characters to the end of the data are replaced (i.newNode., the effect is the same as a remove method call with the same range, followed by an append method invocation).</param>
        /// <param name="arg">The DOMString with which the range must be replaced.</param>
        void replaceData(int offset, int count, string arg);
                                        //raises(DOMException);

    }
}
