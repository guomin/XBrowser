using System;
using System.Xml;
using XBrowserProject.HtmlDom;

namespace XBrowserProject.BrowserModel.Internal.HtmlDom
{
    public class HtmlSelectElement : HtmlElement, IHTMLSelectElement, IFormChild, IResettable
    {
        public HtmlSelectElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        private HtmlOptionsCollection _options = new HtmlOptionsCollection();
        private HtmlFormElement _form;
        private int _selectedIndex = -1;

        public int selectedIndex
        {
            get { return _selectedIndex; }
            set { _selectedIndex = value; }
        }

        public IHTMLFormElement form
        {
            get { return _form; }
        }

        public bool disabled
        {
            get { return HasAttribute(HtmlAttributeNames.DisabledAttributeName); }
            set { SetBooleanAttribute(HtmlAttributeNames.DisabledAttributeName, value); }
        }

        public bool multiple
        {
            get { return HasAttribute(HtmlAttributeNames.MultipleAttributeName); }
            set { SetBooleanAttribute(HtmlAttributeNames.MultipleAttributeName, value); }
        }

        public int length
        {
            get { return _options.length; }
            set { throw new NotSupportedException(); }
        }

        public int size
        {
            get { return int.Parse(GetAttribute(HtmlAttributeNames.SizeAttributeName)); }
            set { SetAttribute(HtmlAttributeNames.SizeAttributeName, value.ToString()); }
        }

        public int tabIndex
        {
            get { return int.Parse(GetAttribute(HtmlAttributeNames.TabIndexAttributeName)); }
            set { SetAttribute(HtmlAttributeNames.TabIndexAttributeName, value.ToString()); }
        }

        public string name
        {
            get { return GetAttribute(HtmlAttributeNames.NameAttributeName); }
            set { SetAttribute(HtmlAttributeNames.NameAttributeName, value); }
        }

        public string type
        {
            get { return multiple ? "select-multiple" : "select-one"; }
        }

        public string value
        {
            get { return GetAttribute(HtmlAttributeNames.ValueAttributeName); }
            set { SetAttribute(HtmlAttributeNames.ValueAttributeName, value); }
        }

        public IHTMLOptionsCollection options
        {
            get { return _options; }
        }

        public void add(IHTMLElement element, IHTMLElement before)
        {
            XmlElement baseElement = element as XmlElement;
            XmlElement baseBefore = before as XmlElement;
            if (before == null)
            {
                AppendChild(baseElement);
            }
            else
            {
                InsertBefore(baseElement, baseBefore);
            }
        }

        public void remove(int index)
        {
            
        }

        public void blur()
        {
        }

        public void focus()
        {
        }

        //readonly attribute DOMString       type;
        //         attribute long            selectedIndex;
        //         attribute DOMString       value;
        //// Modified in DOM Level 2:
        //         attribute unsigned long   length;
        //                                    // raises(dom::DOMException) on setting

        //readonly attribute HTMLFormElement form;
        //// Modified in DOM Level 2:
        //readonly attribute HTMLOptionsCollection options;
        //         attribute boolean         disabled;
        //         attribute boolean         multiple;
        //         attribute DOMString       name;
        //         attribute long            size;
        //         attribute long            tabIndex;
        //void               add(in HTMLElement element, 
        //                       in HTMLElement before)
        //                                    raises(dom::DOMException);
        //void               remove(in long index);
        //void               blur();
        //void               focus();

        #region IFormChild Members

        public void SetForm(HtmlFormElement form)
        {
            _form = form;
        }

        #endregion

        #region IResettable Members

        public void Reset()
        {
            _selectedIndex = -1;
        }

        #endregion
    }
}
