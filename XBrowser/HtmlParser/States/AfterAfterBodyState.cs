using XBrowserProject.BrowserModel.Internal;
using XBrowserProject.HtmlDom;
using XBrowserProject.HtmlParser.HtmlTokenizer;

namespace XBrowserProject.HtmlParser.States
{
    public class AfterAfterBodyState : ParserState
    {
        /// <summary>
        /// Represents the insertion mode of "after after body"
        /// </summary>
        /// <remarks>
        /// When the insertion mode is "after after body", tokens must be handled as follows:
        /// <list type="bullet">
        /// <item>
        /// A comment token
        /// </item>
        /// <para>
        /// Append a Comment node to the Document object with the data attribute set to the data given in the comment token.
        /// </para>
        /// <item>
        /// A DOCTYPE token
        /// </item>
        /// <item>
        /// A character token that is one of U+0009 CHARACTER TABULATION, U+000A LINE FEED (LF), U+000C FORM FEED (FF), or U+0020 SPACE
        /// </item>
        /// <item>
        /// A start tag whose tag name is "html"
        /// <para>
        /// Process the token using the rules for the "in body" insertion mode.
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
        /// </item>
        /// </list>
        /// </remarks>
        public override string Description
        {
            get { return "after after body"; }
        }

        protected override bool ProcessCommentToken(CommentToken comment, Parser parser)
        {
            // A comment token
            // Append a Comment node to the Document object with the data attribute set to the data given in the comment token.
            parser.CreateComment(comment.Data, null);
            return true;
        }

        protected override bool ProcessCharacterToken(CharacterToken character, Parser parser)
        {
            // A character token that is one of U+0009 CHARACTER TABULATION, U+000A LINE FEED (LF),
            // U+000C FORM FEED (FF), or U+0020 SPACE
            // Process the token using the rules for the "in body" insertion mode.
            bool tokenProcessed = false;
            if (HtmlCharacterUtilities.IsWhiteSpace(character.Data))
            {
                InBodyState temporaryState = new InBodyState(Description);
                tokenProcessed = temporaryState.ParseToken(parser);
            }

            return tokenProcessed;
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
            return false;
        }

        protected override bool ProcessEndOfFileToken(Parser parser)
        {
            // An end-of-file token
            // Stop parsing.
            return true;
        }

        protected override bool ProcessUnprocessedToken(Parser parser)
        {
            // Anything else
            // Parse error. Switch the insertion mode to "in body" and reprocess the token.
            parser.LogParseError("found unhandlable token in '" + Description + "' state", "handling using 'in body; rules");
            parser.AdvanceState(new InBodyState());
            return false;
        }
    }
}
