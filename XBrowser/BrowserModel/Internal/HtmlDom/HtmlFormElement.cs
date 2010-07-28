using XBrowserProject.HtmlDom;

namespace XBrowserProject.BrowserModel.Internal.HtmlDom
{
    public class HtmlFormElement : HtmlElement, IHTMLFormElement
    {
        public HtmlFormElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        public IHTMLCollection elements
        {
            get { return new HtmlCollection(ChildNodes); }
        }

        public int length
        {
            get { return elements.length; }
        }

        public string name
        {
            get { return GetAttribute(HtmlAttributeNames.NameAttributeName); }
            set { SetAttribute(HtmlAttributeNames.NameAttributeName, value); }
        }

        public string acceptCharset
        {
            get { return GetAttribute(HtmlAttributeNames.AcceptCharsetAttributeName); }
            set { SetAttribute(HtmlAttributeNames.AcceptCharsetAttributeName, value); }
        }

        public string action
        {
            get { return GetAttribute(HtmlAttributeNames.ActionAttributeName); }
            set { SetAttribute(HtmlAttributeNames.ActionAttributeName, value); }
        }

        public string enctype
        {
            get { return GetAttribute(HtmlAttributeNames.EncTypeAttributeName); }
            set { SetAttribute(HtmlAttributeNames.EncTypeAttributeName, value); }
        }

        public string method
        {
            get { return GetAttribute(HtmlAttributeNames.MethodAttributeName); }
            set { SetAttribute(HtmlAttributeNames.MethodAttributeName, value); }
        }

        public string target
        {
            get { return GetAttribute(HtmlAttributeNames.TargetAttributeName); }
            set { SetAttribute(HtmlAttributeNames.TargetAttributeName, value); }
        }

        public void submit()
        {
        }

        public void reset()
        {
        }

        //readonly attribute HTMLCollection  elements;
        //readonly attribute long            length;
        //         attribute DOMString       name;
        //         attribute DOMString       acceptCharset;
        //         attribute DOMString       action;
        //         attribute DOMString       enctype;
        //         attribute DOMString       method;
        //         attribute DOMString       target;
        //void               submit();
        //void               reset();
    }
}
