using XBrowserProject.HtmlDom;

namespace XBrowserProject.BrowserModel.Internal.HtmlDom
{
    public class HtmlObjectElement : HtmlElement, IHTMLObjectElement, IFormChild
    {
        public HtmlObjectElement(string prefix, string localName, string namespaceURI, HtmlDocument doc)
            : base(prefix, localName, namespaceURI, doc)
        {
        }

        private HtmlFormElement _form;

        public IHTMLFormElement form
        {
            get { return _form; }
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

        public string archive
        {
            get { return GetAttribute(HtmlAttributeNames.ArchiveAttributeName); }
            set { SetAttribute(HtmlAttributeNames.ArchiveAttributeName, value); }
        }

        public string border
        {
            get { return GetAttribute(HtmlAttributeNames.BorderAttributeName); }
            set { SetAttribute(HtmlAttributeNames.BorderAttributeName, value); }
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

        public string codeType
        {
            get { return GetAttribute(HtmlAttributeNames.CodeTypeAttributeName); }
            set { SetAttribute(HtmlAttributeNames.CodeTypeAttributeName, value); }
        }

        public string data
        {
            get { return GetAttribute(HtmlAttributeNames.DataAttributeName); }
            set { SetAttribute(HtmlAttributeNames.DataAttributeName, value); }
        }

        public string standby
        {
            get { return GetAttribute(HtmlAttributeNames.StandbyAttributeName); }
            set { SetAttribute(HtmlAttributeNames.StandbyAttributeName, value); }
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

        public string useMap
        {
            get { return GetAttribute(HtmlAttributeNames.UseMapAttributeName); }
            set { SetAttribute(HtmlAttributeNames.UseMapAttributeName, value); }
        }

        public string type
        {
            get { return GetAttribute(HtmlAttributeNames.TypeAttributeName); }
            set { SetAttribute(HtmlAttributeNames.TypeAttributeName, value); }
        }

        public int tabIndex
        {
            get { return int.Parse(GetAttribute(HtmlAttributeNames.TabIndexAttributeName)); }
            set { SetAttribute(HtmlAttributeNames.TabIndexAttributeName, value.ToString()); }
        }

        public bool declare
        {
            get { return HasAttribute(HtmlAttributeNames.DeclareAttributeName); }
            set { SetBooleanAttribute(HtmlAttributeNames.DeclareAttributeName, value); }
        }

        public IDocument contentDocument
        {
            get { return null; }
        }
        //readonly attribute HTMLFormElement form;
        //         attribute DOMString       code;
        //         attribute DOMString       align;
        //         attribute DOMString       archive;
        //         attribute DOMString       border;
        //         attribute DOMString       codeBase;
        //         attribute DOMString       codeType;
        //         attribute DOMString       data;
        //         attribute boolean         declare;
        //         attribute DOMString       height;
        //         attribute long            hspace;
        //         attribute DOMString       name;
        //         attribute DOMString       standby;
        //         attribute long            tabIndex;
        //         attribute DOMString       type;
        //         attribute DOMString       useMap;
        //         attribute long            vspace;
        //         attribute DOMString       width;
        //// Introduced in DOM Level 2:
        //readonly attribute Document        contentDocument;

        #region IFormChild Members

        public void SetForm(HtmlFormElement form)
        {
            _form = form;
        }

        #endregion
    }
}
