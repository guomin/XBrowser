namespace XBrowserProject.Internal.Html.Interfaces
{
    internal interface IProcessingInstruction : INode
    {
        /// <summary>
        /// The target of this processing instruction. XML defines this as being the first token following the markup that begins the processing instruction.
        /// </summary>
        string target
        {
            get;
        }

        /// <summary>
        /// The content of this processing instruction. This is from the first non white space character after the target to the character immediately preceding the ?>.
        /// </summary>
        string data
        {
            get;
            set;
        }
                                      // raises(DOMException) on setting
    }
}
