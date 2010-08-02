using XBrowserProject.HtmlDom;

namespace XBrowserProject.BrowserModel.Internal.HtmlDom
{
    internal class HtmlTableElement : HtmlElement, IHTMLTableElement
    {
        public HtmlTableElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        private HtmlTableCaptionElement _caption;
        private HtmlTableSectionElement _header;
        private HtmlTableSectionElement _footer;
        private HtmlCollection _rows;
        private HtmlCollection _bodies;

        public IHTMLTableCaptionElement caption
        {
            get { return _caption; }
            set { _caption = value as HtmlTableCaptionElement; }
        }

        public IHTMLTableSectionElement tHead
        {
            get { return _header; }
            set { _header = value as HtmlTableSectionElement; }
        }

        public IHTMLTableSectionElement tFoot
        {
            get { return _footer; }
            set { _footer = value as HtmlTableSectionElement; }
        }

        public IHTMLCollection rows
        {
            get { return _rows; }
        }

        public IHTMLCollection tBodies
        {
            get { return _bodies; }
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

        public string border
        {
            get { return GetAttribute(HtmlAttributeNames.BorderAttributeName); }
            set { SetAttribute(HtmlAttributeNames.BorderAttributeName, value); }
        }

        public string cellPadding
        {
            get { return GetAttribute(HtmlAttributeNames.CellPaddingAttributeName); }
            set { SetAttribute(HtmlAttributeNames.CellPaddingAttributeName, value); }
        }

        public string cellSpacing
        {
            get { return GetAttribute(HtmlAttributeNames.CellSpacingAttributeName); }
            set { SetAttribute(HtmlAttributeNames.CellSpacingAttributeName, value); }
        }

        public string frame
        {
            get { return GetAttribute(HtmlAttributeNames.FrameAttributeName); }
            set { SetAttribute(HtmlAttributeNames.FrameAttributeName, value); }
        }

        public string rules
        {
            get { return GetAttribute(HtmlAttributeNames.RulesAttributeName); }
            set { SetAttribute(HtmlAttributeNames.RulesAttributeName, value); }
        }

        public string summary
        {
            get { return GetAttribute(HtmlAttributeNames.SummaryAttributeName); }
            set { SetAttribute(HtmlAttributeNames.SummaryAttributeName, value); }
        }

        public string width
        {
            get { return GetAttribute(HtmlAttributeNames.WidthAttributeName); }
            set { SetAttribute(HtmlAttributeNames.WidthAttributeName, value); }
        }

        public IHTMLElement createTHead()
        {
            return null;
        }

        public void deleteTHead()
        {
        }

        public IHTMLElement createTFoot()
        {
            return null;
        }

        public void deleteTFoot()
        {
        }

        public IHTMLElement createCaption()
        {
            //_caption = new HtmlTableCaptionElement(HtmlElementTagNames.CaptionElementTagName, OwnerDocument);
            AppendChild(_caption);
            return null;
        }

        public void deleteCaption()
        {

        }

        public IHTMLElement insertRow(int index)
        {
            return null;
        }

        public void deleteRow(int index)
        {
        }

        //// Modified in DOM Level 2:
        //         attribute HTMLTableCaptionElement caption;
        //                                    // raises(dom::DOMException) on setting

        //// Modified in DOM Level 2:
        //         attribute HTMLTableSectionElement tHead;
        //                                    // raises(dom::DOMException) on setting

        //// Modified in DOM Level 2:
        //         attribute HTMLTableSectionElement tFoot;
        //                                    // raises(dom::DOMException) on setting

        //readonly attribute HTMLCollection  rows;
        //readonly attribute HTMLCollection  tBodies;
        //         attribute DOMString       align;
        //         attribute DOMString       bgColor;
        //         attribute DOMString       border;
        //         attribute DOMString       cellPadding;
        //         attribute DOMString       cellSpacing;
        //         attribute DOMString       frame;
        //         attribute DOMString       rules;
        //         attribute DOMString       summary;
        //         attribute DOMString       width;
        //HTMLElement        createTHead();
        //void               deleteTHead();
        //HTMLElement        createTFoot();
        //void               deleteTFoot();
        //HTMLElement        createCaption();
        //void               deleteCaption();
        //// Modified in DOM Level 2:
        //HTMLElement        insertRow(in long index)
        //                                    raises(dom::DOMException);
        //// Modified in DOM Level 2:
        //void               deleteRow(in long index)
        //                                    raises(dom::DOMException);
    }
}
