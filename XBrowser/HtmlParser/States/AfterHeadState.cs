using XBrowserProject.BrowserModel.Internal;
using XBrowserProject.HtmlDom;
using XBrowserProject.HtmlParser.HtmlTokenizer;

namespace XBrowserProject.HtmlParser.States
{
    /// <summary>
    /// Represents the insertion mode of "after head"
    /// </summary>
    /// <remarks>
    /// When the insertion mode is "after head", tokens must be handled as follows:
    /// <list type="bullet">
    /// <item>
    /// A character token that is one of U+0009 CHARACTER TABULATION, U+000A LINE FEED (LF), U+000C FORM FEED (FF), or U+0020 SPACE
    /// <para>
    /// Insert the character into the current node.
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
    /// A start tag whose tag name is "body"
    /// <para>
    /// Insert an HTML element for the token.
    /// </para>
    /// <para>
    /// Set the frameset-ok flag to "not ok".
    /// </para>
    /// <para>
    /// Switch the insertion mode to "in body".
    /// </para>
    /// </item>
    /// <item>
    /// A start tag whose tag name is "frameset"
    /// <para>
    /// Insert an HTML element for the token.
    /// </para>
    /// <para>
    /// Switch the insertion mode to "in frameset".
    /// </para>
    /// </item>
    /// <item>
    /// A start tag token whose tag name is one of: "base", "link", "meta", "noframes", "script", "style", "title"
    /// <para>
    /// Parse error.
    /// </para>
    /// <para>
    /// Push the node pointed to by the head element pointer onto the stack of open elements.
    /// </para>
    /// <para>
    /// Process the token using the rules for the "in head" insertion mode.
    /// </para>
    /// <para>
    /// Remove the node pointed to by the head element pointer from the stack of open elements.
    /// </para>
    /// <para>
    /// Note: The head element pointer cannot be null at this point.
    /// </para>
    /// </item>
    /// <item>
    /// An end tag whose tag name is one of: "body", "html", "br"
    /// <para>
    /// Act as described in the "anything else" entry below.
    /// </para>
    /// </item>
    /// <item>
    /// A start tag whose tag name is "head"
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
    /// Act as if a start tag token with the tag name "body" and no attributes had been seen, then set the frameset-ok flag back to "ok", and then reprocess the current token.
    /// </para>
    /// </item>
    /// </list>
    /// </remarks>
    internal class AfterHeadState : ParserState
    {
        public override string Description
        {
            get { return "after head"; }
        }

        protected override bool ProcessStartTagToken(TagToken tag, Parser parser)
        {
            bool tokenProcessed = false;
            if (tag.Name == HtmlElementFactory.HtmlElementTagName)
            {
                // A start tag whose tag name is "html"
                // Process the token using the rules for the "in body" insertion mode.
                InBodyState temporaryState = new InBodyState(Description);
                temporaryState.ParseToken(parser);
                tokenProcessed = true;
            }
            else if (tag.Name == HtmlElementFactory.BodyElementTagName)
            {
                // A start tag whose tag name is "body"
                // Insert an HTML element for the token.
                // Set the frameset-ok flag to "not ok".
                // Switch the insertion mode to "in body".
                ProcessBodyStartTag(tag, parser, false);
                tokenProcessed = true;
            }
            else if (tag.Name == HtmlElementFactory.FramesetElementTagName)
            {
                // A start tag whose tag name is "frameset"
                // Insert an HTML element for the token.
                // Switch the insertion mode to "in frameset".
                parser.InsertElement(tag);
                parser.IsFramesetOK = true;
                parser.AdvanceState(new InFramesetState());
                tokenProcessed = true;
            }
            else if (tag.Name == HtmlElementFactory.BaseElementTagName ||
                tag.Name == HtmlElementFactory.LinkElementTagName ||
                tag.Name == HtmlElementFactory.MetaElementTagName ||
                tag.Name == HtmlElementFactory.NoFramesElementTagName ||
                tag.Name == HtmlElementFactory.ScriptElementTagName ||
                tag.Name == HtmlElementFactory.StyleElementTagName ||
                tag.Name == HtmlElementFactory.TitleElementTagName)
            {
                // A start tag token whose tag name is one of: "base", "link", "meta", "noframes", "script", "style", "title"
                // Parse error.
                // Push the node pointed to by the head element pointer onto the stack of open elements.
                // Process the token using the rules for the "in head" insertion mode.
                // Remove the node pointed to by the head element pointer from the stack of open elements.
                // Note: The head element pointer cannot be null at this point.
                parser.LogParseError("Cannot have " + tag.Name + " start tag in 'after head' state", "pushing head tag onto stack, processing token, then popping element off stack");
                parser.PushElementToStack(parser.HeadElement);
                InHeadState temporaryState = new InHeadState();
                temporaryState.ParseToken(parser);
                parser.OpenElementStack.Remove(parser.HeadElement);
                tokenProcessed = true;
            }
            else if (tag.Name == HtmlElementFactory.HeadElementTagName)
            {
                // A start tag whose tag name is "head"
                // Parse error. Ignore the token.
                parser.LogParseError("Cannot have 'head' start tag in '" + Description + "' state", "ignoring token");
            }

            return tokenProcessed;
        }

        protected override bool ProcessEndTagToken(TagToken tag, Parser parser)
        {
            bool tokenProcessed = false;
            if (tag.Name != HtmlElementFactory.BodyElementTagName &&
                tag.Name != HtmlElementFactory.HtmlElementTagName &&
                tag.Name != HtmlElementFactory.BRElementTagName)
            {
                // An end tag whose tag name is one of: "body", "html", "br"
                // Act as described in the "anything else" entry below.
                // Any other end tag
                // Parse error. Ignore the token.
                parser.LogParseError("Cannot have " + tag.Name + " end tag in '" + Description + "' state", "ignoring token");
                tokenProcessed = true;
            }

            return tokenProcessed;
        }

        protected override bool ProcessUnprocessedToken(Parser parser)
        {
            // Anything else
            // Act as if a start tag token with the tag name "body" and no attributes had been seen,
            // then set the frameset-ok flag back to "ok", and then reprocess the current token.
            return ProcessBodyStartTag(new TagToken(TokenType.StartTag, HtmlElementFactory.BodyElementTagName), parser, true);
        }

        private bool ProcessBodyStartTag(TagToken tag, Parser parser, bool reprocessTokenInNextState)
        {
            bool tokenProcessed = false;
            parser.InsertElement(tag);
            parser.IsFramesetOK = reprocessTokenInNextState;
            parser.AdvanceState(new InBodyState());
            if (reprocessTokenInNextState)
            {
                tokenProcessed = parser.State.ParseToken(parser);
            }

            return tokenProcessed;
        }
    }
}
