using XBrowserProject.Internal.Html.Interfaces.DOM;

namespace XBrowserProject.Internal.Html.DOM
{
    internal class HtmlAppletElement : HtmlElement, IHTMLAppletElement
    {
        public HtmlAppletElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
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

        public string @object
        {
            get { return GetAttribute(HtmlAttributeNames.ObjectAttributeName); }
            set { SetAttribute(HtmlAttributeNames.ObjectAttributeName, value); }
        }

        public string archive
        {
            get { return GetAttribute(HtmlAttributeNames.ArchiveAttributeName); }
            set { SetAttribute(HtmlAttributeNames.ArchiveAttributeName, value); }
        }

        public string code
        {
            get { return GetAttribute(HtmlAttributeNames.CodeAttributeName); }
            set { SetAttribute(HtmlAttributeNames.CodeAttributeName, value); }
        }

        public string codeBase
        {
            get { return GetAttribute(HtmlAttributeNames.CodeBaseAttributeName); }
            set { SetAttribute(HtmlAttributeNames.CodeBaseAttributeName, value); }
        }

        public string height
        {
            get { return GetAttribute(HtmlAttributeNames.HeightAttributeName); }
            set { SetAttribute(HtmlAttributeNames.HeightAttributeName, value); }
        }

        public string width
        {
            get { return GetAttribute(HtmlAttributeNames.WidthAttributeName); }
            set { SetAttribute(HtmlAttributeNames.WidthAttributeName, value); }
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

        //         attribute DOMString       align;
        //         attribute DOMString       alt;
        //         attribute DOMString       archive;
        //         attribute DOMString       code;
        //         attribute DOMString       codeBase;
        //         attribute DOMString       height;
        //// Modified in DOM Level 2:
        //         attribute long            hspace;
        //         attribute DOMString       name;
        //// Modified in DOM Level 2:
        //         attribute DOMString       object;
        //// Modified in DOM Level 2:
        //         attribute long            vspace;
        //         attribute DOMString       width;
    }
}
