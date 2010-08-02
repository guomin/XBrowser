namespace XBrowserProject.HtmlDom
{
    internal interface IHTMLAreaElement : IHTMLElement
    {
        string accessKey { get; set; }
        string alt { get; set; }
        string coords { get; set; }
        string href { get; set; }
        bool noHref { get; set; }
        string shape { get; set; }
        int tabIndex { get; set; }
        string target { get; set; }

    }
}
