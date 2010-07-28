using XBrowserProject.HtmlDom;

namespace XBrowserProject.BrowserModel.Internal.HtmlDom
{
    public class HtmlHRElement : HtmlElement, IHTMLHRElement
    {
        public HtmlHRElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

       public string align
        {
            get { return GetAttribute(HtmlAttributeNames.AlignAttributeName); }
            set { SetAttribute(HtmlAttributeNames.AlignAttributeName, value); }
        }

        public bool noShade
        {
            get { return HasAttribute(HtmlAttributeNames.NoShadeAttributeName); }
            set { SetBooleanAttribute(HtmlAttributeNames.NoShadeAttributeName, value); }
        }

        public string size
        {
            get { return GetAttribute(HtmlAttributeNames.SizeAttributeName); }
            set { SetAttribute(HtmlAttributeNames.SizeAttributeName, value); }
        }

        public string width
        {
            get { return GetAttribute(HtmlAttributeNames.WidthAttributeName); }
            set { SetAttribute(HtmlAttributeNames.WidthAttributeName, value); }
        }

        //attribute DOMString       align;
        //attribute boolean         noShade;
        //attribute DOMString       size;
        //attribute DOMString       width;
    }
}
