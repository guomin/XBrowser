using System;
using System.Collections.Generic;
using System.Text;

namespace XBrowserProject.HtmlDom
{
    public interface IHTMLFieldsetElement : IHTMLElement
    {
        IHTMLFormElement form { get; }
    }
}
