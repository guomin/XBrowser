namespace XBrowserProject.Internal.Html.Interfaces.DOM
{
    internal interface IHTMLIsIndexElement : IHTMLElement
    {
        IHTMLFormElement form { get; }
        string prompt { get; set; }
    }
}
