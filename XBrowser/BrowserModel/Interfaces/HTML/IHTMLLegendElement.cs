using System;
using System.Collections.Generic;
using System.Text;

namespace XBrowserProject.HtmlDom
{
    internal interface IHTMLLegendElement : IHTMLElement
    {
        IHTMLFormElement form { get; }
        string accessKey { get; set; }
        string align { get; set; }
    }
}
