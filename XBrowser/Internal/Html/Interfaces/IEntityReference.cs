namespace XBrowserProject.Internal.Html.Interfaces
{
    internal interface IEntityReference : INode
    {
        // Introduced in DOM Level 2:
        string publicId { get; }

        // Introduced in DOM Level 2:
        string systemId { get; }

        string notationName { get; }
    }
}
