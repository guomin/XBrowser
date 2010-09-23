using XBrowserProject.Internal.Html.Interfaces;
using XBrowserProject.Internal.Html.Interfaces.DOM;

namespace XBrowserProject.Internal.Html.DOM
{
    internal class HtmlFrameElement : HtmlElement, IHTMLFrameElement
    {
        public HtmlFrameElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        public string frameBorder
        {
            get { return GetAttribute(HtmlAttributeNames.FrameBorderAttributeName); }
            set { SetAttribute(HtmlAttributeNames.FrameBorderAttributeName, value); }
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

        public bool noResize
        {
            get { return HasAttribute(HtmlAttributeNames.NoResizeAttributeName); }
            set { SetBooleanAttribute(HtmlAttributeNames.NoResizeAttributeName, value); }
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

        public IDocument contentDocument
        {
            get { return null; }
        }

        //         attribute DOMString       frameBorder;
        //         attribute DOMString       longDesc;
        //         attribute DOMString       marginHeight;
        //         attribute DOMString       marginWidth;
        //         attribute DOMString       name;
        //         attribute boolean         noResize;
        //         attribute DOMString       scrolling;
        //         attribute DOMString       src;
        //// Introduced in DOM Level 2:
        //readonly attribute Document        contentDocument;
    }
}
