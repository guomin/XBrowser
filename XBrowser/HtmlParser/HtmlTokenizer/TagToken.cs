using System.Collections.Generic;

namespace XBrowserProject.HtmlParser.HtmlTokenizer
{
    internal class TagToken : Token
    {
        private TokenType tokenTypeValue;
        private string tokenName = string.Empty;
        private bool tokenIsSelfClosing = false;
        private string currentAttributeName = string.Empty;
        private string currentAttributeValue = string.Empty;
        private Dictionary<string, TagTokenAttribute> tokenAttributeDictionary = new Dictionary<string, TagTokenAttribute>();
        private bool dropAttributeValue;

        public TagToken(TokenType type, string name)
        {
            tokenName = name;
            tokenTypeValue = type;
        }

        public string Name
        {
            get { return tokenName; }
            set { tokenName = value; }
        }

        public override TokenType TokenType
        {
            get { return tokenTypeValue; }
        }

        public bool IsSelfClosing
        {
            get { return tokenIsSelfClosing; }
            set { tokenIsSelfClosing = value; }
        }

        public Dictionary<string, TagTokenAttribute> Attributes
        {
            get { return tokenAttributeDictionary; }
        }

        public string CurrentAttributeName
        {
            get { return currentAttributeName; }
            set { currentAttributeName = value; }
        }

        public string CurrentAttributeValue
        {
            get { return currentAttributeValue; }
            set { currentAttributeValue = value; }
        }

        public bool CommitAttributeName()
        {
            bool attributeNameAlreadyExists = tokenAttributeDictionary.ContainsKey(currentAttributeName);
            if (attributeNameAlreadyExists)
            {
                dropAttributeValue = true;
            }
            else
            {
                tokenAttributeDictionary.Add(currentAttributeName, new TagTokenAttribute(currentAttributeName, string.Empty));
                currentAttributeValue = string.Empty;
                dropAttributeValue = false;
            }

            return !attributeNameAlreadyExists;
        }

        public void CommitAttributeValue()
        {
            if (!dropAttributeValue)
            {
                tokenAttributeDictionary[currentAttributeName].Value = currentAttributeValue;
            }
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", TokenType, Name);
        }
    }
}
