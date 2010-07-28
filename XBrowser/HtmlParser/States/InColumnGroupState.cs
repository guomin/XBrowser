using XBrowserProject.BrowserModel.Internal;
using XBrowserProject.HtmlDom;
using XBrowserProject.HtmlParser.HtmlTokenizer;

namespace XBrowserProject.HtmlParser.States
{
    /// <summary>
    /// Represents the insertion mode of "in column group"
    /// </summary>
    /// <remarks>
    /// When the insertion mode is "in column group", tokens must be handled as follows:
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
    /// A start tag whose tag name is "col"
    /// <para>
    /// Insert an HTML element for the token. Immediately pop the current node off the stack of open elements.
    /// </para>
    /// <para>
    /// Acknowledge the token's self-closing flag, if it is set.
    /// </para>
    /// </item>
    /// <item>
    /// An end tag whose tag name is "colgroup"
    /// <para>
    /// If the current node is the root html element, then this is a parse error; ignore the token. (fragment case)
    /// </para>
    /// <para>
    /// Otherwise, pop the current node (which will be a colgroup element) from the stack of open elements. Switch the insertion mode to "in table".
    /// </para>
    /// </item>
    /// <item>
    /// An end tag whose tag name is "col"
    /// <para>
    /// Parse error. Ignore the token.
    /// </para>
    /// </item>
    /// <item>
    /// An end-of-file token
    /// <para>
    /// If the current node is the root html element, then stop parsing. (fragment case)
    /// </para>
    /// <para>
    /// Otherwise, act as described in the "anything else" entry below.
    /// </para>
    /// </item>
    /// <item>
    /// Anything else
    /// <para>
    /// Act as if an end tag with the tag name "colgroup" had been seen, and then, if that token wasn't ignored, reprocess the current token.
    /// </para>
    /// <para>
    /// Note: The fake end tag token here can only be ignored in the fragment case.
    /// </para>
    /// </item>
    /// </list>
    /// </remarks>
    public class InColumnGroupState : ParserState
    {
        public override string Description
        {
            get { return "in column group"; }
        }
        // An end-of-file token
        // If the current node is the root html element, then stop parsing. (fragment case)

        protected override bool ProcessStartTagToken(TagToken tag, Parser parser)
        {
            bool tokenProcessed = false;

            switch (tag.Name)
            {
                case HtmlElementFactory.HtmlElementTagName:
                    // A start tag whose tag name is "html"
                    // Process the token using the rules for the "in body" insertion mode.
                    InBodyState temporaryState = new InBodyState(Description);
                    temporaryState.ParseToken(parser);
                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.ColElementTagName:
                    // A start tag whose tag name is "col"
                    // Insert an HTML element for the token. Immediately pop the current node 
                    // off the stack of open elements.
                    // Acknowledge the token's self-closing flag, if it is set.
                    parser.InsertElement(tag, true);
                    tokenProcessed = true;
                    break;
            }

            return tokenProcessed;
        }

        protected override bool ProcessEndTagToken(TagToken tag, Parser parser)
        {
            bool tokenProcessed = false;

            switch (tag.Name)
            {
                case HtmlElementFactory.ColGroupElementTagName:
                    // An end tag whose tag name is "colgroup"
                    // If the current node is the root html element, then this is a parse
                    // error; ignore the token. (fragment case)
                    // Otherwise, pop the current node (which will be a colgroup element) from
                    // the stack of open elements. Switch the insertion mode to "in table".
                    ProcessColumnGroupEndTag(tag, parser, false);
                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.ColElementTagName:
                    // An end tag whose tag name is "col"
                    // Parse error. Ignore the token.
                    parser.LogParseError("Found '" + tag.Name + "' end tag token in '" + Description + "' state", "ignoring token");
                    tokenProcessed = true;
                    break;  
            }

            return tokenProcessed;
        }

        protected override bool ProcessUnprocessedToken(Parser parser)
        {
            // Anything else
            // Act as if an end tag with the tag name "colgroup" had been seen, and then, 
            // if that token wasn't ignored, reprocess the current token.
            // Note: The fake end tag token here can only be ignored in the fragment case.
            bool tokenProcessed = ProcessColumnGroupEndTag(new TagToken(TokenType.EndTag, HtmlElementFactory.ColGroupElementTagName), parser, true);
            return tokenProcessed;
        }

        private bool ProcessColumnGroupEndTag(TagToken tag, Parser parser, bool reprocessTokenInNextState)
        {
            bool tokenProcessed = false;
            // If the current node is the root html element, then this is a parse
            // error; ignore the token. (fragment case)
            // Otherwise, pop the current node (which will be a colgroup element) from
            // the stack of open elements. Switch the insertion mode to "in table".
            if (parser.CurrentNode.Name == HtmlElementFactory.HtmlElementTagName)
            {
                parser.LogParseError("'html' is at top of stack", "ignoring token");
                tokenProcessed = true;
            }
            else
            {
                parser.PopElementFromStack();
                parser.AdvanceState(new InTableState());
            }

            return tokenProcessed;
        }
    }
}
