using System;
namespace XBrowserProject.HtmlDom
{
    public interface IHTMLBaseFontElement : IHTMLElement
    {
        string color { get; set; }
        string face { get; set; }
        int size { get; set; }
    }
}
