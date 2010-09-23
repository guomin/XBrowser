namespace XBrowserProject.Internal.Html.Interfaces.DOM
{
    internal interface IHTMLHistory
    {
        int length { get; }
        void go(int delta);
        void back();
        void forward();
        void pushState(object data, string title, string url);
        void replaceState(object data, string title, string url);
        //readonly attribute long length;
        //void go(in optional long delta);
        //void back();
        //void forward();
        //void pushState(in any data, in DOMString title, in optional DOMString url);
        //void replaceState(in any data, in DOMString title, in optional DOMString url);
    }
}
