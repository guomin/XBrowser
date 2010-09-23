namespace XBrowserProject.Internal.Html.DOM
{
    internal class HtmlFrameSetElement : HtmlElement
    {
        public HtmlFrameSetElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        public string Cols
        {
            get { return GetAttribute(HtmlAttributeNames.ColsAttributeName); }
            set { SetAttribute(HtmlAttributeNames.ColsAttributeName, value); }
        }

        public string Rows
        {
            get { return GetAttribute(HtmlAttributeNames.RowsAttributeName); }
            set { SetAttribute(HtmlAttributeNames.RowsAttributeName, value); }
        }

        //attribute DOMString       cols;
        //attribute DOMString       rows;
    }
}
