using XBrowserProject.Internal.Html.Interfaces.DOM;

namespace XBrowserProject.Internal.Html.DOM
{
    internal class HtmlFontElement : HtmlElement, IHTMLFontElement
    {
        public HtmlFontElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        public string color
        {
            get { return GetAttribute(HtmlAttributeNames.ColorAttributeName); }
            set { SetAttribute(HtmlAttributeNames.ColorAttributeName, value); }
        }

        public string face
        {
            get { return GetAttribute(HtmlAttributeNames.FaceAttributeName); }
            set { SetAttribute(HtmlAttributeNames.FaceAttributeName, value); }
        }

        public int size
        {
            get { return int.Parse(GetAttribute(HtmlAttributeNames.SizeAttributeName)); }
            set { SetAttribute(HtmlAttributeNames.SizeAttributeName, value.ToString()); }
        }

        //         attribute DOMString       color;
        //         attribute DOMString       face;
        //// Modified in DOM Level 2:
        //         attribute long            size;
    }
}
