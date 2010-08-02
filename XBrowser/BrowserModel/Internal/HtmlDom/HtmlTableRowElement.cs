using XBrowserProject.HtmlDom;

namespace XBrowserProject.BrowserModel.Internal.HtmlDom
{
    internal class HtmlTableRowElement : HtmlElement, IHTMLTableRowElement
    {
        public HtmlTableRowElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        private int _rowIndex;
        private int _sectionRowIndex;
        private HtmlCollection _cells;

        public int rowIndex
        {
            get { return _rowIndex; }
        }

        public int sectionRowIndex
        {
            get { return _sectionRowIndex; }
        }

        public string align
        {
            get { return GetAttribute(HtmlAttributeNames.AlignAttributeName); }
            set { SetAttribute(HtmlAttributeNames.AlignAttributeName, value); }
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

        public string vAlign
        {
            get { return GetAttribute(HtmlAttributeNames.VAlignAttributeName); }
            set { SetAttribute(HtmlAttributeNames.VAlignAttributeName, value); }
        }

        public IHTMLCollection cells
        {
            get { return _cells; }
        }

        public IHTMLElement insertCell(int index)
        {
            return null;
        }

        public void deleteCell(int index)
        {
        }

        //// Modified in DOM Level 2:
        //readonly attribute long            rowIndex;
        //// Modified in DOM Level 2:
        //readonly attribute long            sectionRowIndex;
        //// Modified in DOM Level 2:
        //readonly attribute HTMLCollection  cells;
        //         attribute DOMString       align;
        //         attribute DOMString       bgColor;
        //         attribute DOMString       ch;
        //         attribute DOMString       chOff;
        //         attribute DOMString       vAlign;
        //// Modified in DOM Level 2:
        //HTMLElement        insertCell(in long index)
        //                                    raises(dom::DOMException);
        //// Modified in DOM Level 2:
        //void               deleteCell(in long index)
        //                                    raises(dom::DOMException);
    }
}
