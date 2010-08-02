using XBrowserProject.HtmlDom;

namespace XBrowserProject.BrowserModel.Internal.HtmlDom
{
    internal class HtmlTableSectionElement : HtmlElement, IHTMLTableSectionElement
    {
        public HtmlTableSectionElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        private HtmlCollection _rows;

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

        public string vAlign
        {
            get { return GetAttribute(HtmlAttributeNames.VAlignAttributeName); }
            set { SetAttribute(HtmlAttributeNames.VAlignAttributeName, value); }
        }

        public IHTMLCollection rows
        {
            get { return _rows; }
        }

        public IHTMLElement insertRow(int index)
        {
            return null;
        }

        public void deleteRow(int index)
        {
        }

        //         attribute DOMString       align;
        //         attribute DOMString       ch;
        //         attribute DOMString       chOff;
        //         attribute DOMString       vAlign;
        //readonly attribute HTMLCollection  rows;
        //// Modified in DOM Level 2:
        //HTMLElement        insertRow(in long index)
        //                                    raises(dom::DOMException);
        //// Modified in DOM Level 2:
        //void               deleteRow(in long index)
        //                                    raises(dom::DOMException);
    }
}
