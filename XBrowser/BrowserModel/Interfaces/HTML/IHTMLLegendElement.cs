using System;
using System.Collections.Generic;
using System.Text;

namespace XBrowserProject.HtmlDom
{
    public interface IHTMLLegendElement : IHTMLElement
    {
        IHTMLFormElement form { get; }
        string accessKey { get; set; }
        string align { get; set; }
    }
}
