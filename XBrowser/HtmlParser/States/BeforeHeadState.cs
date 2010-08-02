using XBrowserProject.BrowserModel.Internal;
using XBrowserProject.HtmlDom;
using XBrowserProject.HtmlParser.HtmlTokenizer;

namespace XBrowserProject.HtmlParser.States
{
    /// <summary>
    /// Represents the insertion mode of "before head"
    /// </summary>
    /// <remarks>
    /// When the insertion mode is "before head", tokens must be handled as follows:
    /// <list type="bullet">
    /// <item>
    /// A character token that is one of U+0009 CHARACTER TABULATION, U+000A LINE FEED (LF), U+000C FORM FEED (FF), or U+0020 SPACE
    /// <para>
    /// Ignore the token.
    /// </para>
    /// </item>
    /// <item>
    /// A comment token
    /// <para>
    /// Append a Comment node to the current node with the data attribute set to the data given in the comment token.
    /// </para>
    /// </item>
    /// <item>
    /// A DOCTYPE token
    /// <para>
    /// Parse error. Ignore the token.
    /// </para>
    /// </item>
    /// <item>
    /// A start tag whose tag name is "html"
    /// <para>
    /// Process the token using the rules for the "in body" insertion mode.
    /// </para>
    /// </item>
    /// <item>
    /// A start tag whose tag name is "head"
    /// <para>
    /// Insert an HTML element for the token.
    /// </para>
    /// <para>
    /// Set the head element pointer to the newly created head element.
    /// </para>
    /// <para>
    /// Switch the insertion mode to "in head".
    /// </para>
    /// </item>
    /// <item>
    /// An end tag whose tag name is one of: "head", "body", "html", "br"
    /// <para>
    /// Act as if a start tag token with the tag name "head" and no attributes had been seen, then reprocess the current token.
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
    /// Act as if a start tag token with the tag name "head" and no attributes had been seen, then reprocess the current token.
    /// </para>
    /// </item>
    /// </list>
    /// </remarks>
    internal class BeforeHeadState : ParserState
    {
        public override string Description
        {
            get { return "before head"; }
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
            bool tokenProcessed = false;

            // A start tag whose tag name is "head"
            // Insert an HTML element for the token.
            // Set the head element pointer to the newly created head element.
            // Switch the insertion mode to "in head".
            if (tag.Name == HtmlElementFactory.HeadElementTagName)
            {
                ProcessHeadStartTag(tag, parser, false);
                tokenProcessed = true;
            }
            else if (tag.Name == HtmlElementFactory.HtmlElementTagName)
            {
                // A start tag whose tag name is "html"
                // Process the token using the rules for the "in body" insertion mode.
                InBodyState temporaryState = new InBodyState(Description);
                temporaryState.ParseToken(parser);
                tokenProcessed = true;
            }

            return tokenProcessed;
        }

        protected override bool ProcessEndTagToken(TagToken tag, Parser parser)
        {
            bool tokenProcessed = false;

            if (tag.Name == HtmlElementFactory.HtmlElementTagName ||
                tag.Name == HtmlElementFactory.BodyElementTagName ||
                tag.Name == HtmlElementFactory.HeadElementTagName ||
                tag.Name == HtmlElementFactory.BRElementTagName)
            {
                // An end tag whose tag name is one of: "head", "body", "html", "br"
                // Act as if a start tag token with the tag name "head" and no attributes had been seen,
                // then reprocess the current token.
                tokenProcessed = ProcessHeadStartTag(new TagToken(TokenType.StartTag, HtmlElementFactory.HeadElementTagName), parser, true);
            }
            else
            {
                // Any other end tag
                // Parse error. Ignore the token.
                parser.LogParseError("Cannot have close tag for " + tag.Name + " in '" + Description + "' state", "ignoring token");
                tokenProcessed = true;
            }

            return tokenProcessed;
        }

        protected override bool ProcessUnprocessedToken(Parser parser)
        {
            // Anything else
            // Act as if a start tag token with the tag name "head" and no attributes had been seen, 
            // then reprocess the current token.
            bool tokenProcessed = ProcessHeadStartTag(new TagToken(TokenType.StartTag, HtmlElementFactory.HeadElementTagName), parser, true);
            //parser.AdvanceState(new BeforeHeadState());
            return tokenProcessed;
        }

        private bool ProcessHeadStartTag(TagToken tag, Parser parser, bool reprocessTokenInNextState)
        {
            bool tokenProcessed = false;
            parser.InsertElement(tag);
            parser.HeadElement = parser.CurrentNode;
            parser.AdvanceState(new InHeadState());
            if (reprocessTokenInNextState)
            {
                tokenProcessed = parser.State.ParseToken(parser);
            }

            return tokenProcessed;
        }
    }
}
