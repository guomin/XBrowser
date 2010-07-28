using System;
using System.Collections.Generic;
using System.Text;

namespace XBrowserProject.HtmlDom
{
    public interface IHTMLLocation
    {
        string href { get; set; }
        void assign(string url);
        void replace(string url);
        void reload();
        string resolveURL(string url);

        string protocol { get; set; }
        string host { get; set; }
        string hostname { get; set; }
        string port { get; set; }
        string pathname { get; set; }
        string search { get; set; }
        string hash { get; set; }

        //stringifier attribute DOMString href;
        //void assign(in DOMString url);
        //void replace(in DOMString url);
        //void reload();

        //// URL decomposition IDL attributes 
        //       attribute DOMString protocol;
        //       attribute DOMString host;
        //       attribute DOMString hostname;
        //       attribute DOMString port;
        //       attribute DOMString pathname;
        //       attribute DOMString search;
        //       attribute DOMString hash;

        //// resolving relative URLs
        //DOMString resolveURL(in DOMString url);
    }
}
