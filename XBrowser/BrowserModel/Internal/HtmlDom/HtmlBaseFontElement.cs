using XBrowserProject.HtmlDom;

namespace XBrowserProject.BrowserModel.Internal.HtmlDom
{
    internal class HtmlBaseFontElement : HtmlElement, IHTMLBaseFontElement
    {
        public HtmlBaseFontElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
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
