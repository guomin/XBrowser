using XBrowserProject.BrowserModel.Internal;
using XBrowserProject.HtmlDom;
using XBrowserProject.HtmlParser.HtmlTokenizer;

namespace XBrowserProject.HtmlParser.States
{
    /// <summary>
    /// Represents the insertion mode of "in frameset"
    /// </summary>
    /// <remarks>
    /// When the insertion mode is "in frameset", tokens must be handled as follows:
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
    /// A start tag whose tag name is "frameset"
    /// <para>
    /// Insert an HTML element for the token.
    /// </para>
    /// </item>
    /// <item>
    /// An end tag whose tag name is "frameset"
    /// <para>
    /// If the current node is the root html element, then this is a parse error; ignore the token. (fragment case)
    /// </para>
    /// <para>
    /// Otherwise, pop the current node from the stack of open elements.
    /// </para>
    /// <para>
    /// If the parser was not originally created as part of the HTML fragment parsing algorithm (fragment case), and the current node is no longer a frameset element, then switch the insertion mode to "after frameset".
    /// </para>
    /// </item>
    /// <item>
    /// A start tag whose tag name is "frame"
    /// <para>
    /// Insert an HTML element for the token. Immediately pop the current node off the stack of open elements.
    /// </para>
    /// <para>
    /// Acknowledge the token's self-closing flag, if it is set.
    /// </para>
    /// </item>
    /// <item>
    /// A start tag whose tag name is "noframes"
    /// <para>
    /// Process the token using the rules for the "in head" insertion mode.
    /// </para>
    /// </item>
    /// <item>
    /// An end-of-file token
    /// <para>
    /// If the current node is not the root html element, then this is a parse error.
    /// </para>
    /// <para>
    /// Note: It can only be the current node in the fragment case.
    /// </para>
    /// <para>
    /// Stop parsing.
    /// </para>
    /// </item>
    /// <item>
    /// Anything else
    /// <para>
    /// Parse error. Ignore the token.
    /// </para>
    /// </item>
    /// </list>
    /// </remarks>
    internal class InFramesetState : ParserState
    {
        public override string Description
        {
            get { return "in frameset"; }
        }

        protected override bool ProcessEndOfFileToken(Parser parser)
        {
            // An end-of-file token
            // If the current node is not the root html element, then this is a parse error.
            // Note: It can only be the current node in the fragment case.
            // Stop parsing.
            if (parser.CurrentNode.Name != HtmlElementFactory.HtmlElementTagName)
            {
                parser.LogParseError("end of file token encountered in '" + Description + "' state", "none, parsing ending");
            }

            return base.ProcessEndOfFileToken(parser);
        }

        protected override bool ProcessStartTagToken(TagToken tag, Parser parser)
        {
            bool tokenProcessed = false;
            switch (tag.Name)
            {
                case HtmlElementFactory.HtmlElementTagName:
                    // A start tag whose tag name is "html"
                    // Process the token using the rules for the "in body" insertion mode.
                    InBodyState temporaryBodyState = new InBodyState(Description);
                    temporaryBodyState.ParseToken(parser);
                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.FramesetElementTagName:
                    // A start tag whose tag name is "frameset"
                    // Insert an HTML element for the token.
                    parser.InsertElement(tag);
                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.FrameElementTagName:
                    // A start tag whose tag name is "frame"
                    // Insert an HTML element for the token. Immediately pop the current node 
                    // off the stack of open elements.
                    // Acknowledge the token's self-closing flag, if it is set.
                    parser.InsertElement(tag, true);
                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.NoFramesElementTagName:
                    // A start tag whose tag name is "noframes"
                    // Process the token using the rules for the "in head" insertion mode.
                    InHeadState temporaryHeadState = new InHeadState(Description);
                    temporaryHeadState.ParseToken(parser);
                    tokenProcessed = true;
                    break;
            }

            return tokenProcessed;
        }

        protected override bool ProcessEndTagToken(TagToken tag, Parser parser)
        {
            bool tokenProcessed = false;
            // An end tag whose tag name is "frameset"
            // If the current node is the root html element, then this is a parse error; 
            // ignore the token. (fragment case)
            // Otherwise, pop the current node from the stack of open elements.
            // If the parser was not originally created as part of the HTML fragment parsing 
            // algorithm (fragment case), and the current node is no longer a frameset element, 
            // then switch the insertion mode to "after frameset".
            if (tag.Name == HtmlElementFactory.FramesetElementTagName)
            {
                if (parser.CurrentNode.Name == HtmlElementFactory.HtmlElementTagName)
                {
                    parser.LogParseError("found end tag for 'frameset' while current node is root 'html' element", "ignoring token");
                }
                else
                {
                    parser.PopElementFromStack();
                    if (!parser.IsFragmentParser && parser.CurrentNode.Name != HtmlElementFactory.FramesetElementTagName)
                    {
                        parser.AdvanceState(new AfterFramesetState());
                    }
                }

                tokenProcessed = true;
            }

            return tokenProcessed;
        }

        protected override bool ProcessUnprocessedToken(Parser parser)
        {
            // Anything else
            // Parse error. Ignore the token.
            parser.LogParseError("found unhandlable token in '" + Description + "' state", "ignoring token");
            return true;
        }
    }
}
