using System;
using System.Collections.Generic;
using System.Text;

namespace XBrowserProject.HtmlDom
{
    internal interface IHTMLSelectElement
    {
        string type { get; }
        int selectedIndex { get; set; }
        string value { get; set; }

        // Modified in DOM Level 2:
        int length { get; set; }
        IHTMLFormElement form { get; }
       
        // Modified in DOM Level 2:
        IHTMLOptionsCollection options { get; }
        bool disabled { get; set; }
        bool multiple { get; set; }
        string name { get; set; }
        int size { get; set; }
        int tabIndex { get; set; }
        
        void add(IHTMLElement element, IHTMLElement before);
        //raises(dom::DOMException);
     
        void remove(int index);
        void blur();
        void focus();
    }
}
