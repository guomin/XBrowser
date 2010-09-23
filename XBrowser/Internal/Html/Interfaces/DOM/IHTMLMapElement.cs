

namespace XBrowserProject.Internal.Html.Interfaces.DOM
{
    internal interface IHTMLMapElement : IHTMLElement
    {
        IHTMLCollection areas { get; }
        string name { get; set; }
    }
}
