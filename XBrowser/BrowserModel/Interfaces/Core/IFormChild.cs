using System;
using System.Collections.Generic;
using System.Text;
using XBrowserProject.BrowserModel.Internal.HtmlDom;

namespace XBrowserProject.HtmlDom
{
    internal interface IFormChild
    {
        void SetForm(HtmlFormElement form);
    }
}
