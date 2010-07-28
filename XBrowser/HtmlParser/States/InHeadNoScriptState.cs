using XBrowserProject.BrowserModel.Internal;
using XBrowserProject.HtmlDom;
using XBrowserProject.HtmlParser.HtmlTokenizer;

namespace XBrowserProject.HtmlParser.States
{
    /// <summary>
    /// Represents the insertion mode of "in head noscript"
    /// </summary>
    /// <remarks>
    /// When the insertion mode is "in head noscript", tokens must be handled as follows:
    /// <list type="bullet">
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
    /// An end tag whose tag name is "noscript"
    /// <para>
    /// Pop the current node (which will be a noscript element) from the stack of open elements; the new current node will be a head element.
    /// </para>
    /// <para>
    /// Switch the insertion mode to "in head".
    /// </para>
    /// </item>
    /// <item>
    /// A character token that is one of U+0009 CHARACTER TABULATION, U+000A LINE FEED (LF), U+000C FORM FEED (FF), or U+0020 SPACE
    /// </item>
    /// <item>
    /// A comment token
    /// </item>
    /// <item>
    /// A start tag whose tag name is one of: "link", "meta", "noframes", "style"
    /// <para>
    /// Process the token using the rules for the "in head" insertion mode.
    /// </para>
    /// </item>
    /// <item>
    /// An end tag whose tag name is "br"
    /// <para>
    /// Act as described in the "anything else" entry below.
    /// </para>
    /// </item>
    /// <item>
    /// A start tag whose tag name is one of: "head", "noscript"
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
    /// Parse error. Act as if an end tag with the tag name "noscript" had been seen and reprocess the current token.
    /// </para>
    /// </item>
    /// </list>
    /// </remarks>
    public class InHeadNoScriptState : ParserState
    {
        public override string Description
        {
            get { return "in head noscript"; }
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
            else if (tag.Name == HtmlElementFactory.LinkElementTagName ||
                tag.Name == HtmlElementFactory.MetaElementTagName ||
                tag.Name == HtmlElementFactory.NoFramesElementTagName ||
                tag.Name == HtmlElementFactory.StyleElementTagName)
            {
                // A start tag whose tag name is one of: "link", "meta", "noframes", "style"
                // Process the token using the rules for the "in head" insertion mode.
                InHeadState temporaryState = new InHeadState();
                parser.AdvanceState(this);
            }
            else if (tag.Name == HtmlElementFactory.NoScriptElementTagName ||
                tag.Name == HtmlElementFactory.HeadElementTagName)
            {
                // A start tag whose tag name is one of: "head", "noscript"
                // Parse error. Ignore the token.
                parser.LogParseError("Cannot have " + tag.Name + " start tag in 'in head noscript' state", "ignoring token");
                tokenProcessed = true;
            }

            return tokenProcessed;
        }

        protected override bool ProcessEndTagToken(TagToken tag, Parser parser)
        {
            bool tokenProcessed = false;
            if (tag.Name == HtmlElementFactory.NoScriptElementTagName)
            {
                ProcessNoScriptEndTag(tag, parser, false);
                tokenProcessed = true;
            }
            else if (tag.Name != HtmlElementFactory.BRElementTagName)
            {
                // An end tag whose tag name is "br"
                // Act as described in the "anything else" entry below.
                //
                // Any other end tag
                // Parse error. Ignore the token.
                parser.LogParseError("Cannot have " + tag.Name + " end tag in 'in head noscript' state", "ignoring token");
                tokenProcessed = true;
            }

            return tokenProcessed;
        }

        protected override bool ProcessUnprocessedToken(Parser parser)
        {
            // Anything else
            // Parse error. Act as if an end tag with the tag name "noscript" had been seen and 
            // reprocess the current token.
            ProcessNoScriptEndTag(new TagToken(TokenType.EndTag, HtmlElementFactory.NoScriptElementTagName), parser, true);
            return false;
        }

        private void ProcessNoScriptEndTag(TagToken tag, Parser parser, bool reprocessTokenInNextState)
        {
            // An end tag whose tag name is "noscript"
            // Pop the current node (which will be a noscript element) from the stack of open 
            // elements; the new current node will be a head element.
            // Switch the insertion mode to "in head".
            parser.PopElementFromStack();
            parser.AdvanceState(new InHeadState());
        }
    }
}
