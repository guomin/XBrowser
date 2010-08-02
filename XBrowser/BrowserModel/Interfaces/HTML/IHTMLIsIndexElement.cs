using System;
namespace XBrowserProject.HtmlDom
{
    internal interface IHTMLIsIndexElement : IHTMLElement
    {
        IHTMLFormElement form { get; }
        string prompt { get; set; }
    }
}
