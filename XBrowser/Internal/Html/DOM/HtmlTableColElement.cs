using XBrowserProject.Internal.Html.Interfaces.DOM;

namespace XBrowserProject.Internal.Html.DOM
{
    internal class HtmlTableColElement : HtmlElement, IHTMLTableColElement
    {
        public HtmlTableColElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        public string align
        {
            get { return GetAttribute(HtmlAttributeNames.AlignAttributeName); }
            set { SetAttribute(HtmlAttributeNames.AlignAttributeName, value); }
        }

        public string ch
        {
            get { return GetAttribute(HtmlAttributeNames.CharAttributeName); }
            set { SetAttribute(HtmlAttributeNames.CharAttributeName, value); }
        }

        public string chOff
        {
            get { return GetAttribute(HtmlAttributeNames.CharOffAttributeName); }
            set { SetAttribute(HtmlAttributeNames.CharOffAttributeName, value); }
        }

        public int span
        {
            get { return int.Parse(GetAttribute(HtmlAttributeNames.SpanAttributeName)); }
            set { SetAttribute(HtmlAttributeNames.SpanAttributeName, value.ToString()); }
        }

        public string vAlign
        {
            get { return GetAttribute(HtmlAttributeNames.VAlignAttributeName); }
            set { SetAttribute(HtmlAttributeNames.VAlignAttributeName, value); }
        }

        public string width
        {
            get { return GetAttribute(HtmlAttributeNames.WidthAttributeName); }
            set { SetAttribute(HtmlAttributeNames.WidthAttributeName, value); }
        }

        //attribute DOMString       align;
        //attribute DOMString       ch;
        //attribute DOMString       chOff;
        //attribute long            span;
        //attribute DOMString       vAlign;
        //attribute DOMString       width;
    }
}
