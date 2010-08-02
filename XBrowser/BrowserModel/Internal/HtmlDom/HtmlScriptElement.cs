using XBrowserProject.HtmlDom;

namespace XBrowserProject.BrowserModel.Internal.HtmlDom
{
    internal class HtmlScriptElement : HtmlElement, IHTMLScriptElement
    {
        private bool alreadyStarted = false;
        private bool parserInserted = false;
        private bool readyToBeParserExecuted = false;

        public HtmlScriptElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        public string text
        {
            get { return GetAttribute(HtmlAttributeNames.TextAttributeName); }
            set { SetAttribute(HtmlAttributeNames.TextAttributeName, value); }
        }

        public string htmlFor
        {
            // Reserved for future use
            get { return string.Empty; }
            set { }
        }

        public string @event
        {
            // Reserved for future use
            get { return string.Empty; }
            set { }
        }

        public string charset
        {
            get { return GetAttribute(HtmlAttributeNames.CharsetAttributeName); }
            set { SetAttribute(HtmlAttributeNames.CharsetAttributeName, value); }
        }

        public bool defer
        {
            get { return HasAttribute(HtmlAttributeNames.DeferAttributeName); }
            set { SetBooleanAttribute(HtmlAttributeNames.DeferAttributeName, value); }
        }

        public string src
        {
            get { return GetAttribute(HtmlAttributeNames.SrcAttributeName); }
            set { SetAttribute(HtmlAttributeNames.SrcAttributeName, value); }
        }

        public string type
        {
            get { return GetAttribute(HtmlAttributeNames.TypeAttributeName); }
            set { SetAttribute(HtmlAttributeNames.TypeAttributeName, value); }
        }

        //attribute DOMString       text;
        //attribute DOMString       htmlFor;
        //attribute DOMString       event;
        //attribute DOMString       charset;
        //attribute boolean         defer;
        //attribute DOMString       src;
        //attribute DOMString       type;

        internal bool AlreadyStarted
        {
            get { return alreadyStarted; }
            set { alreadyStarted = value; }
        }

        internal bool ParserInserted
        {
            get { return parserInserted; }
            set { parserInserted = value; }
        }

        internal bool ReadyToBeParserExecuted
        {
            get { return readyToBeParserExecuted; }
            set { readyToBeParserExecuted = value; }
        }
    }
}
