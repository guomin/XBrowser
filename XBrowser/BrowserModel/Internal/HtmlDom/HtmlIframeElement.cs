using XBrowserProject.HtmlDom;

namespace XBrowserProject.BrowserModel.Internal.HtmlDom
{
    internal class HtmlIframeElement : HtmlElement, IHTMLIframeElement
    {
        public HtmlIframeElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        public string align
        {
            get { return GetAttribute(HtmlAttributeNames.AlignAttributeName); }
            set { SetAttribute(HtmlAttributeNames.AlignAttributeName, value); }
        }

        public string frameBorder
        {
            get { return GetAttribute(HtmlAttributeNames.FrameBorderAttributeName); }
            set { SetAttribute(HtmlAttributeNames.FrameBorderAttributeName, value); }
        }

        public string height
        {
            get { return GetAttribute(HtmlAttributeNames.HeightAttributeName); }
            set { SetAttribute(HtmlAttributeNames.HeightAttributeName, value); }
        }

        public string longDesc
        {
            get { return GetAttribute(HtmlAttributeNames.LongDescAttributeName); }
            set { SetAttribute(HtmlAttributeNames.LongDescAttributeName, value); }
        }

        public string marginHeight
        {
            get { return GetAttribute(HtmlAttributeNames.MarginHeightAttributeName); }
            set { SetAttribute(HtmlAttributeNames.MarginHeightAttributeName, value); }
        }

        public string marginWidth
        {
            get { return GetAttribute(HtmlAttributeNames.MarginWidthAttributeName); }
            set { SetAttribute(HtmlAttributeNames.MarginWidthAttributeName, value); }
        }

        public string name
        {
            get { return GetAttribute(HtmlAttributeNames.NameAttributeName); }
            set { SetAttribute(HtmlAttributeNames.NameAttributeName, value); }
        }

        public string scrolling
        {
            get { return GetAttribute(HtmlAttributeNames.ScrollingAttributeName); }
            set { SetAttribute(HtmlAttributeNames.ScrollingAttributeName, value); }
        }

        public string src
        {
            get { return GetAttribute(HtmlAttributeNames.SrcAttributeName); }
            set { SetAttribute(HtmlAttributeNames.SrcAttributeName, value); }
        }

        public string width
        {
            get { return GetAttribute(HtmlAttributeNames.WidthAttributeName); }
            set { SetAttribute(HtmlAttributeNames.WidthAttributeName, value); }
        }

        public IDocument contentDocument
        {
            get { return null; }
        }
        //         attribute DOMString       align;
        //         attribute DOMString       frameBorder;
        //         attribute DOMString       height;
        //         attribute DOMString       longDesc;
        //         attribute DOMString       marginHeight;
        //         attribute DOMString       marginWidth;
        //         attribute DOMString       name;
        //         attribute DOMString       scrolling;
        //         attribute DOMString       src;
        //         attribute DOMString       width;
        //// Introduced in DOM Level 2:
        //readonly attribute Document        contentDocument;
    }
}
