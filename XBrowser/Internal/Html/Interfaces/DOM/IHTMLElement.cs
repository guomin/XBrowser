namespace XBrowserProject.Internal.Html.Interfaces.DOM
{
    internal interface IHTMLElement : IElement
    {
        /// <summary>
        /// The element'coreRule identifier. See the id attribute definition  in HTML 4.0.
        /// </summary>
        string id
        {
            get;
            set;
        }

        /// <summary>
        /// The element'coreRule advisory title. See the title attribute definition  in HTML 4.0.
        /// </summary>
        string title
        {
            get;
            set;
        }
        
        /// <summary>
        /// Language code defined in RFC 1766. See the lang attribute definition  in HTML 4.0.
        /// </summary>
        string lang
        {
            get;
            set;
        }

        /// <summary>
        /// Specifies the base direction of directionally neutral text and the directionality of tables. See the dir attribute definition  in HTML 4.0.
        /// </summary>
        string dir
        {
            get;
            set;
        }

        /// <summary>
        /// The class attribute of the element. This attribute has been renamed due to conflicts with the "class" keyword exposed by many languages. See the class attribute definition  in HTML 4.0.
        /// </summary>
        string className
        {
            get;
            set;
        }

    }
}
