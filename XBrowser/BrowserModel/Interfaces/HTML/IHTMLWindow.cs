using System;
using System.Collections.Generic;
using System.Text;
using XBrowserProject.BrowserModel.Internal.HtmlDom;

namespace XBrowserProject.HtmlDom
{
    internal interface IHTMLWindow
    {
        IHTMLWindow window { get; }
        IHTMLWindow self { get; }
        HtmlDocument document { get; }
        string name { get; set; }
        IHTMLLocation location { get; }
        IHTMLHistory history { get; }
        void close();
        void focus();
        void blur();
        IHTMLWindow frames { get; }
        int length { get; }
        IHTMLWindow top { get;}
        IHTMLWindow opener { get; }
        IHTMLWindow parent { get; }
        IHTMLElement frameElement { get; }
        IHTMLWindow open(string url, string target, string features, string replace);
        IHTMLWindow this[int index] { get; }
        IHTMLWindow this[string name] { get; }

        void alert(string message);
        bool confirm(string message);
        string prompt(string message, string defaultValue);

        //// the current browsing context
        //readonly attribute WindowProxy window;
        //readonly attribute WindowProxy self;
        //readonly attribute Document document;
        //       attribute DOMString name;
        //[PutForwards=href] readonly attribute Location location;
        //readonly attribute History history;
        //readonly attribute UndoManager undoManager;
        //Selection getSelection();
        //[Replaceable] readonly attribute BarProp locationbar;
        //[Replaceable] readonly attribute BarProp menubar;
        //[Replaceable] readonly attribute BarProp personalbar;
        //[Replaceable] readonly attribute BarProp scrollbars;
        //[Replaceable] readonly attribute BarProp statusbar;
        //[Replaceable] readonly attribute BarProp toolbar;
        //void close();
        //void focus();
        //void blur();

        //// other browsing contexts
        //[Replaceable] readonly attribute WindowProxy frames;
        //[Replaceable] readonly attribute unsigned long length;
        //readonly attribute WindowProxy top;
        //[Replaceable] readonly attribute WindowProxy opener;
        //readonly attribute WindowProxy parent;
        //readonly attribute Element frameElement;
        //WindowProxy open(in optional DOMString url, in optional DOMString target, in optional DOMString features, in optional DOMString replace);
        //getter WindowProxy (in unsigned long index);
        //getter WindowProxy (in DOMString name);

        //// the user agent
        //readonly attribute Navigator navigator; 
        //readonly attribute ApplicationCache applicationCache;

        //// user prompts
        //void alert(in DOMString message);
        //boolean confirm(in DOMString message);
        //DOMString prompt(in DOMString message, in optional DOMString default);
        //void print();
        //any showModalDialog(in DOMString url, in optional any argument);

        //// event handler IDL attributes
        //attribute Function onabort;
        //attribute Function onafterprint;
        //attribute Function onbeforeprint;
        //attribute Function onbeforeunload;
        //attribute Function onblur;
        //attribute Function oncanplay;
        //attribute Function oncanplaythrough;
        //attribute Function onchange;
        //attribute Function onclick;
        //attribute Function oncontextmenu;
        //attribute Function ondblclick;
        //attribute Function ondrag;
        //attribute Function ondragend;
        //attribute Function ondragenter;
        //attribute Function ondragleave;
        //attribute Function ondragover;
        //attribute Function ondragstart;
        //attribute Function ondrop;
        //attribute Function ondurationchange;
        //attribute Function onemptied;
        //attribute Function onended;
        //attribute Function onerror;
        //attribute Function onfocus;
        //attribute Function onformchange;
        //attribute Function onforminput;
        //attribute Function onhashchange;
        //attribute Function oninput;
        //attribute Function oninvalid;
        //attribute Function onkeydown;
        //attribute Function onkeypress;
        //attribute Function onkeyup;
        //attribute Function onload;
        //attribute Function onloadeddata;
        //attribute Function onloadedmetadata;
        //attribute Function onloadstart;
        //attribute Function onmessage;
        //attribute Function onmousedown;
        //attribute Function onmousemove;
        //attribute Function onmouseout;
        //attribute Function onmouseover;
        //attribute Function onmouseup;
        //attribute Function onmousewheel;
        //attribute Function onoffline;
        //attribute Function ononline;
        //attribute Function onpause;
        //attribute Function onplay;
        //attribute Function onplaying;
        //attribute Function onpagehide;
        //attribute Function onpageshow;
        //attribute Function onpopstate;
        //attribute Function onprogress;
        //attribute Function onratechange;
        //attribute Function onreadystatechange;
        //attribute Function onredo;
        //attribute Function onresize;
        //attribute Function onscroll;
        //attribute Function onseeked;
        //attribute Function onseeking;
        //attribute Function onselect;
        //attribute Function onshow;
        //attribute Function onstalled;
        //attribute Function onstorage;
        //attribute Function onsubmit;
        //attribute Function onsuspend;
        //attribute Function ontimeupdate;
        //attribute Function onundo;
        //attribute Function onunload;
        //attribute Function onvolumechange;
        //attribute Function onwaiting;
    }
}
