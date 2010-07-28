using XBrowserProject.BrowserModel.Internal;
using XBrowserProject.HtmlDom;
using XBrowserProject.HtmlParser.HtmlTokenizer;

namespace XBrowserProject.HtmlParser.States
{
    /// <summary>
    /// Represents the insertion mode of "after frameset"
    /// </summary>
    /// <remarks>
    /// When the insertion mode is "after frameset", tokens must be handled as follows:
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
    /// An end tag whose tag name is "html"
    /// <para>
    /// Switch the insertion mode to "after after frameset".
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
    public class AfterFramesetState : ParserState
    {
        public override string Description
        {
            get { return "after frameset"; }
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
            if (tag.Name == HtmlElementFactory.HtmlElementTagName)
            {
                // An end tag whose tag name is "html"
                // Switch the insertion mode to "after after frameset".
                parser.AdvanceState(new AfterAfterFramesetState());
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
