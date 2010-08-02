using XBrowserProject.BrowserModel.Internal;
using XBrowserProject.HtmlDom;
using XBrowserProject.HtmlParser.HtmlTokenizer;

namespace XBrowserProject.HtmlParser.States
{
    /// <summary>
    /// Represents the insertion mode of "before html"
    /// </summary>
    /// <remarks>
    /// When the insertion mode is "before html", tokens must be handled as follows:
    /// <list type="bullet">
    /// <item>
    /// A DOCTYPE token
    /// <para>
    /// Parse error. Ignore the token.
    /// </para>
    /// </item>
    /// <item>
    /// A comment token
    /// <para>
    /// Append a Comment node to the Document object with the data attribute set to the data given in the comment token.
    /// </para>
    /// </item>
    /// <item>
    /// A character token that is one of U+0009 CHARACTER TABULATION, U+000A LINE FEED (LF), U+000C FORM FEED (FF), or U+0020 SPACE
    /// <para>
    /// Ignore the token.
    /// </para>
    /// </item>
    /// <item>
    /// A start tag whose tag name is "html"
    /// <para>
    /// Create an element for the token in the HTML namespace. Append it to the Document object. Put this element in the stack of open elements.
    /// </para>
    /// <para>
    /// If the Document is being loaded as part of navigation of a browsing context, then: if the newly created element has a manifest attribute whose value is not the empty string, then resolve the value of that attribute to an absolute URL, relative to the newly created element, and if that is successful, run the application cache selection algorithm with the resulting absolute URL with any <fragment> component removed; otherwise, if there is no such attribute, or its value is the empty string, or resolving its value fails, run the application cache selection algorithm with no manifest. The algorithm must be passed the Document object.
    /// </para>
    /// <para>
    /// Switch the insertion mode to "before head".
    /// </para>
    /// </item>
    /// <item>
    /// An end tag whose tag name is one of: "head", "body", "html", "br"
    /// <para>
    /// Act as described in the "anything else" entry below.
    /// </para>
    /// </item>
    /// <item>
    /// Any other end tag
    /// <para>
    /// Parse error. Ignore the token.
    /// </para>
    /// </item>
    /// <item>
    /// Anything else
    /// <para>
    /// Create an html element. Append it to the Document object. Put this element in the stack of open elements.
    /// </para>
    /// <para>
    /// If the Document is being loaded as part of navigation of a browsing context, then: run the application cache selection algorithm with no manifest, passing it the Document object.
    /// </para>
    /// <para>
    /// Switch the insertion mode to "before head", then reprocess the current token.
    /// </para>
    /// </item>
    /// </list>
    /// <para>
    /// The root element can end up being removed from the Document object, e.g. by scripts; nothing in particular happens in such cases, content continues being appended to the nodes as described in the next section.
    /// </para>
    /// </remarks>
    internal class BeforeHtmlState : ParserState
    {
        public override string Description
        {
            get { return "before html"; }
        }

        protected override bool ProcessCharacterToken(CharacterToken character, Parser parser)
        {
            // A character token that is one of U+0009 CHARACTER TABULATION, 
            // U+000A LINE FEED (LF), U+000C FORM FEED (FF), or U+0020 SPACE
            // Ignore the token.
            bool tokenProcessed = false;
            if (HtmlCharacterUtilities.IsWhiteSpace(character.Data))
            {
                tokenProcessed = true;
            }

            return tokenProcessed;
        }

        protected override bool ProcessStartTagToken(TagToken tag, Parser parser)
        {
            // A start tag whose tag name is "html"
            // Create an element for the token in the HTML namespace. Append it to the Document object. 
            // Put this element in the stack of open elements.
            //
            // If the Document is being loaded as part of navigation of a browsing context,
            // then: if the newly created element has a manifest attribute whose value is
            // not the empty string, then resolve the value of that attribute to an absolute URL,
            // relative to the newly created element, and if that is successful, run the application 
            // cache selection algorithm with the resulting absolute URL with any <fragment> component
            // removed; otherwise, if there is no such attribute, or its value is the empty string,
            // or resolving its value fails, run the application cache selection algorithm with no 
            // manifest. The algorithm must be passed the Document object.
            //
            // Switch the insertion mode to "before head".
            bool tokenProcessed = false;
            if (tag.Name == HtmlElementFactory.HtmlElementTagName)
            {
                // TODO: Use application caching algorithm as defined by spec.
                parser.InsertElement(tag);
                tokenProcessed = true;
                parser.AdvanceState(new BeforeHeadState());
            }

            return tokenProcessed;
        }

        protected override bool ProcessEndTagToken(TagToken tag, Parser parser)
        {
            bool tokenProcessed = false;

            // An end tag whose tag name is one of: "head", "body", "html", "br"
            // Act as described in the "anything else" entry below.
            //
            // Any other end tag
            // Parse error. Ignore the token.
            if (tag.Name != HtmlElementFactory.HtmlElementTagName &&
                tag.Name != HtmlElementFactory.BodyElementTagName &&
                tag.Name != HtmlElementFactory.HeadElementTagName &&
                tag.Name != HtmlElementFactory.HrElementTagName)
            {
                parser.LogParseError("Cannot have close tag for " + tag.Name + " in '" + Description + "' state", "ignoring token");
                tokenProcessed = true;
            }

            return tokenProcessed;
        }

        protected override bool ProcessUnprocessedToken(Parser parser)
        {
            // Anything else
            // Create an html element. Append it to the Document object. Put this element 
            // in the stack of open elements.
            //
            // If the Document is being loaded as part of navigation of a browsing context, 
            // then: run the application cache selection algorithm with no manifest, passing 
            // it the Document object.
            //
            // Switch the insertion mode to "before head", then reprocess the current token.
            // TODO: Use application caching algorithm as defined by spec.
            parser.InsertElement(new TagToken(TokenType.StartTag, HtmlElementFactory.HtmlElementTagName), false);
            parser.AdvanceState(new BeforeHeadState());
            return false;
        }
    }
}
