using XBrowserProject.Internal.Html.Interfaces.DOM;

namespace XBrowserProject.Internal.Html.DOM
{
    internal class HtmlImageElement : HtmlElement, IHTMLImageElement
    {
        public HtmlImageElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        public string name
        {
            get { return GetAttribute(HtmlAttributeNames.NameAttributeName); }
            set { SetAttribute(HtmlAttributeNames.NameAttributeName, value); }
        }

        public string align
        {
            get { return GetAttribute(HtmlAttributeNames.AlignAttributeName); }
            set { SetAttribute(HtmlAttributeNames.AlignAttributeName, value); }
        }

        public string alt
        {
            get { return GetAttribute(HtmlAttributeNames.AltAttributeName); }
            set { SetAttribute(HtmlAttributeNames.AltAttributeName, value); }
        }

        public string border
        {
            get { return GetAttribute(HtmlAttributeNames.BorderAttributeName); }
            set { SetAttribute(HtmlAttributeNames.BorderAttributeName, value); }
        }

        public string longDesc
        {
            get { return GetAttribute(HtmlAttributeNames.LongDescAttributeName); }
            set { SetAttribute(HtmlAttributeNames.LongDescAttributeName, value); }
        }

        public string src
        {
            get { return GetAttribute(HtmlAttributeNames.SrcAttributeName); }
            set { SetAttribute(HtmlAttributeNames.SrcAttributeName, value); }
        }

        public bool isMap
        {
            get { return HasAttribute(HtmlAttributeNames.IsMapAttributeName); }
            set { SetBooleanAttribute(HtmlAttributeNames.IsMapAttributeName, value); }
        }

        public string useMap
        {
            get { return GetAttribute(HtmlAttributeNames.UseMapAttributeName); }
            set { SetAttribute(HtmlAttributeNames.UseMapAttributeName, value); }
        }

        public int height
        {
            get { return int.Parse(GetAttribute(HtmlAttributeNames.HeightAttributeName)); }
            set { SetAttribute(HtmlAttributeNames.HeightAttributeName, value.ToString()); }
        }

        public int width
        {
            get { return int.Parse(GetAttribute(HtmlAttributeNames.WidthAttributeName)); }
            set { SetAttribute(HtmlAttributeNames.WidthAttributeName, value.ToString()); }
        }

        public int hspace
        {
            get { return int.Parse(GetAttribute(HtmlAttributeNames.HSpaceAttributeName)); }
            set { SetAttribute(HtmlAttributeNames.HSpaceAttributeName, value.ToString()); }
        }

        public int vspace
        {
            get { return int.Parse(GetAttribute(HtmlAttributeNames.VSpaceAttributeName)); }
            set { SetAttribute(HtmlAttributeNames.VSpaceAttributeName, value.ToString()); }
        }
        //         attribute DOMString       name;
        //         attribute DOMString       align;
        //         attribute DOMString       alt;
        //         attribute DOMString       border;
        //// Modified in DOM Level 2:
        //         attribute long            height;
        //// Modified in DOM Level 2:
        //         attribute long            hspace;
        //         attribute boolean         isMap;
        //         attribute DOMString       longDesc;
        //         attribute DOMString       src;
        //         attribute DOMString       useMap;
        //// Modified in DOM Level 2:
        //         attribute long            vspace;
        //// Modified in DOM Level 2:
        //         attribute long            width;
    }
}
