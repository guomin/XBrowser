using XBrowserProject.Internal.Html.Interfaces.DOM;

namespace XBrowserProject.Internal.Html.DOM
{
    internal class HtmlBodyElement : HtmlElement, IHTMLBodyElement
    {
        public HtmlBodyElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        public string aLink
        {
            get { return GetAttribute(HtmlAttributeNames.ALinkAttributeName); }
            set { SetAttribute(HtmlAttributeNames.ALinkAttributeName, value); }
        }

        public string background
        {
            get { return GetAttribute(HtmlAttributeNames.BackgroundAttributeName); }
            set { SetAttribute(HtmlAttributeNames.BackgroundAttributeName, value); }
        }

        public string bgColor
        {
            get { return GetAttribute(HtmlAttributeNames.BgColorAttributeName); }
            set { SetAttribute(HtmlAttributeNames.BgColorAttributeName, value); }
        }

        public string link
        {
            get { return GetAttribute(HtmlAttributeNames.LinkAttributeName); }
            set { SetAttribute(HtmlAttributeNames.LinkAttributeName, value); }
        }

        public string text
        {
            get { return GetAttribute(HtmlAttributeNames.TextAttributeName); }
            set { SetAttribute(HtmlAttributeNames.TextAttributeName, value); }
        }

        public string vLink
        {
            get { return GetAttribute(HtmlAttributeNames.VLinkAttributeName); }
            set { SetAttribute(HtmlAttributeNames.VLinkAttributeName, value); }
        }

        //attribute DOMString       aLink;
        //attribute DOMString       background;
        //attribute DOMString       bgColor;
        //attribute DOMString       link;
        //attribute DOMString       text;
        //attribute DOMString       vLink;
    }
}
