using System;
using System.Collections.Generic;
using System.Text;

namespace XBrowserProject.HtmlDom
{
    internal interface IHTMLFieldsetElement : IHTMLElement
    {
        IHTMLFormElement form { get; }
    }
}
