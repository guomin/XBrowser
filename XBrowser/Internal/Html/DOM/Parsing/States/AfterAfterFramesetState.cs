using XBrowserProject.Internal.Html.DOM.Parsing.HtmlTokenizer;

namespace XBrowserProject.Internal.Html.DOM.Parsing.States
{
    internal class AfterAfterFramesetState : ParserState
    {
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
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
        /// A start tag whose tag name is "noframes"
        /// <para>
        /// Process the token using the rules for the "in head" insertion mode.
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
            get { return "after after frameset"; }
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
            if (tag.Name == HtmlElementFactory.HtmlElementTagName)
            {
                // A start tag whose tag name is "html"
                // Process the token using the rules for the "in body" insertion mode.
                InBodyState temporaryState = new InBodyState(Description);
                temporaryState.ParseToken(parser);
                tokenProcessed = true;
            }
            else if (tag.Name == HtmlElementFactory.NoFramesElementTagName)
            {
                // A start tag whose tag name is "noframes"
                // Process the token using the rules for the "in head" insertion mode.
                InHeadState temporaryHeadState = new InHeadState(Description);
                temporaryHeadState.ParseToken(parser);
                tokenProcessed = true;
            }

            return tokenProcessed;
        }

        protected override bool ProcessEndTagToken(TagToken tag, Parser parser)
        {
            return false;
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
