using System;
namespace XBrowserProject.HtmlDom
{
    internal interface IHTMLBaseFontElement : IHTMLElement
    {
        string color { get; set; }
        string face { get; set; }
        int size { get; set; }
    }
}
