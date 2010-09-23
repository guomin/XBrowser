namespace XBrowserProject.Internal.Html.Interfaces
{
    internal interface IDocumentType : INode
    {
        /// <summary>
        /// The name of DTD; i.newNode., the name immediately following the DOCTYPE keyword.
        /// </summary>
        string name
        {
            get;
        }

        /// <summary>
        /// A NamedNodeMap containing the general entities, both external and internal, declared in the DTD.
        /// </summary>
        INamedNodeMap entities
        {
            get;
        }

        /// <summary>
        /// A NamedNodeMap containing the notations declared in the DTD. Duplicates are discarded. Every node in this map also implements the Notation interface.
        /// The DOM Level 1 does not support editing notations, therefore notations cannot be altered in any way. 
        /// </summary>
        INamedNodeMap notations
        {
            get;
        }

        // Introduced in DOM Level 2:
        string publicId { get; }

        // Introduced in DOM Level 2:
        string systemId { get; }

        // Introduced in DOM Level 2:
        string internalSubset { get; }
    }
}
