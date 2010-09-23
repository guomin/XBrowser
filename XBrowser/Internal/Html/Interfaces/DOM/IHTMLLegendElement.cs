namespace XBrowserProject.Internal.Html.Interfaces.DOM
{
    internal interface IHTMLLegendElement : IHTMLElement
    {
        IHTMLFormElement form { get; }
        string accessKey { get; set; }
        string align { get; set; }
    }
}
