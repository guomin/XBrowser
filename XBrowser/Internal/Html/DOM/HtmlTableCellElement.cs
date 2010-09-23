using XBrowserProject.Internal.Html.Interfaces.DOM;

namespace XBrowserProject.Internal.Html.DOM
{
    internal class HtmlTableCellElement : HtmlElement, IHTMLTableCellElement
    {
        public HtmlTableCellElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        private int _cellIndex;

        public int cellIndex
        {
            get { return _cellIndex; }
        }

        public string abbr
        {
            get { return GetAttribute(HtmlAttributeNames.AbbrAttributeName); }
            set { SetAttribute(HtmlAttributeNames.AbbrAttributeName, value); }
        }

        public string align
        {
            get { return GetAttribute(HtmlAttributeNames.AlignAttributeName); }
            set { SetAttribute(HtmlAttributeNames.AlignAttributeName, value); }
        }

        public string axis
        {
            get { return GetAttribute(HtmlAttributeNames.AxisAttributeName); }
            set { SetAttribute(HtmlAttributeNames.AxisAttributeName, value); }
        }

        public string bgColor
        {
            get { return GetAttribute(HtmlAttributeNames.BgColorAttributeName); }
            set { SetAttribute(HtmlAttributeNames.BgColorAttributeName, value); }
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

        public int colSpan
        {
            get { return int.Parse(GetAttribute(HtmlAttributeNames.ColSpanAttributeName)); }
            set { SetAttribute(HtmlAttributeNames.ColSpanAttributeName, value.ToString()); }
        }

        public string headers
        {
            get { return GetAttribute(HtmlAttributeNames.HeadersAttributeName); }
            set { SetAttribute(HtmlAttributeNames.HeadersAttributeName, value); }
        }

        public string height
        {
            get { return GetAttribute(HtmlAttributeNames.HeightAttributeName); }
            set { SetAttribute(HtmlAttributeNames.HeightAttributeName, value); }
        }

        public bool noWrap
        {
            get { return HasAttribute(HtmlAttributeNames.NoWrapAttributeName); }
            set { SetBooleanAttribute(HtmlAttributeNames.NoWrapAttributeName, value); }
        }

        public int rowSpan
        {
            get { return int.Parse(GetAttribute(HtmlAttributeNames.RowSpanAttributeName)); }
            set { SetAttribute(HtmlAttributeNames.RowSpanAttributeName, value.ToString()); }
        }

        public string scope
        {
            get { return GetAttribute(HtmlAttributeNames.ScopeAttributeName); }
            set { SetAttribute(HtmlAttributeNames.ScopeAttributeName, value); }
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
        //readonly attribute long            cellIndex;
        //         attribute DOMString       abbr;
        //         attribute DOMString       align;
        //         attribute DOMString       axis;
        //         attribute DOMString       bgColor;
        //         attribute DOMString       ch;
        //         attribute DOMString       chOff;
        //         attribute long            colSpan;
        //         attribute DOMString       headers;
        //         attribute DOMString       height;
        //         attribute boolean         noWrap;
        //         attribute long            rowSpan;
        //         attribute DOMString       scope;
        //         attribute DOMString       vAlign;
        //         attribute DOMString       width;
    }
}
