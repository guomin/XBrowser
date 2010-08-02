namespace XBrowserProject.HtmlParser.HtmlTokenizer
{
    internal class TagTokenAttribute
    {
        private string attributeName = string.Empty;
        private string attributeValue = string.Empty;
        private string attributeNamespace = string.Empty;

        public TagTokenAttribute(string name, string value)
        {
            attributeName = name;
            attributeValue = value;
        }

        public string Name
        {
            get { return attributeName; }
            set { attributeName = value; }
        }

        public string Value
        {
            get { return attributeValue; }
            set { attributeValue = value; }
        }

        public string Namespace
        {
            get { return attributeNamespace; }
            set { attributeNamespace = value; }
        }
    }
}
