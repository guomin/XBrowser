using System;
namespace XBrowserProject.HtmlDom
{
    public interface IHTMLIsIndexElement : IHTMLElement
    {
        IHTMLFormElement form { get; }
        string prompt { get; set; }
    }
}
