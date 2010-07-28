namespace XBrowserProject.HtmlDom
{
    public interface IHTMLScriptElement : IHTMLElement
    {
        string text { get; set; }
        string htmlFor { get; set; }
        string @event { get; set; }
        string charset { get; set; }
        bool defer { get; set; }
        string src { get; set; }
        string type { get; set; }
    }
}
