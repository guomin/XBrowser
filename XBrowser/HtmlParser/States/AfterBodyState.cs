using XBrowserProject.BrowserModel.Internal;
using XBrowserProject.BrowserModel.Internal.HtmlDom;
using XBrowserProject.HtmlDom;
using XBrowserProject.HtmlParser.HtmlTokenizer;

namespace XBrowserProject.HtmlParser.States
{
    /// <summary>
    /// Represents the insertion mode of "after body"
    /// </summary>
    /// <remarks>
    /// When the insertion mode is "after body", tokens must be handled as follows:
    /// <list type="bullet">
    /// <item>
    /// A character token that is one of U+0009 CHARACTER TABULATION, U+000A LINE FEED (LF), U+000C FORM FEED (FF), or U+0020 SPACE
    /// <para>
    /// Process the token using the rules for the "in body" insertion mode.
    /// </para>
    /// </item>
    /// <item>
    /// A comment token
    /// <para>
    /// Append a Comment node to the first element in the stack of open elements (the html element), with the data attribute set to the data given in the comment token.
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
    /// If the parser was originally created as part of the HTML fragment parsing algorithm, this is a parse error; ignore the token. (fragment case)
    /// </para>
    /// <para>
    /// Otherwise, switch the insertion mode to "after after body".
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
    /// Parse error. Switch the insertion mode to "in body" and reprocess the token.
    /// </para>
    /// </list>
    /// </remarks>
    internal class AfterBodyState : ParserState
    {
        public override string Description
        {
            get { return "after body"; }
        }

        protected override bool ProcessCommentToken(CommentToken comment, Parser parser)
        {
            HtmlElement rootElement = parser.OpenElementStack[parser.OpenElementStack.Count - 1];
            parser.CreateComment(comment.Data, rootElement);
            return true;
        }

        protected override bool ProcessCharacterToken(CharacterToken character, Parser parser)
        {
            return false;
        }

        protected override bool ProcessStartTagToken(TagToken tag, Parser parser)
        {
            bool tokenProcessed = false;

            // A start tag whose tag name is "html"
            // Process the token using the rules for the "in body" insertion mode.
            if (tag.Name == HtmlElementFactory.HtmlElementTagName)
            {
                InBodyState temporaryState = new InBodyState(Description);
                temporaryState.ParseToken(parser);
                tokenProcessed = true;
            }

            return tokenProcessed;
        }

        protected override bool ProcessEndTagToken(TagToken tag, Parser parser)
        {
            bool tokenProcessed = false;

            // An end tag whose tag name is "html"
            // If the parser was originally created as part of the HTML fragment parsing algorithm, 
            // this is a parse error; ignore the token. (fragment case)
            // Otherwise, switch the insertion mode to "after after body".
            if (tag.Name == HtmlElementFactory.HtmlElementTagName)
            {
                if (parser.IsFragmentParser)
                {
                    parser.LogParseError("found end tag for 'html' in '" + Description + "' state", "ignoring token");
                }
                else
                {
                    parser.AdvanceState(new AfterAfterBodyState());
                }

                tokenProcessed = true;
            }

            return tokenProcessed;
        }

        protected override bool ProcessUnprocessedToken(Parser parser)
        {
            // Anything else
            // Parse error. Switch the insertion mode to "in body" and reprocess the token.
            parser.LogParseError("Found unexpected token in '" + Description + "' state", "Switching to 'in body' state and reprocessing");
            parser.AdvanceState(new InBodyState());
            return false;
        }
    }
}
