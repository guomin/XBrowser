using XBrowserProject.Internal.Html.DOM.Parsing.HtmlTokenizer;

namespace XBrowserProject.Internal.Html.DOM.Parsing.States
{
    /// <summary>
    /// Represents the insertion mode of "in caption"
    /// </summary>
    /// <remarks>
    /// When the insertion mode is "in caption", tokens must be handled as follows:
    /// <list type="bullet">
    /// <item>
    /// An end tag whose tag name is "caption"
    /// <para>
    /// If the stack of open elements does not have an element in table scope with the same tag name as the token, this is a parse error. Ignore the token. (fragment case)
    /// </para>
    /// <para>
    /// Otherwise:
    /// </para>
    /// <para>
    /// Generate implied end tags.
    /// </para>
    /// <para>
    /// Now, if the current node is not a caption element, then this is a parse error.
    /// </para>
    /// <para>
    /// Pop elements from this stack until a caption element has been popped from the stack.
    /// </para>
    /// <para>
    /// Clear the list of active formatting elements up to the last marker.
    /// </para>
    /// <para>
    /// Switch the insertion mode to "in table".
    /// </para>
    /// </item>
    /// <item>
    /// A start tag whose tag name is one of: "caption", "col", "colgroup", "tbody", "td", "tfoot", "th", "thead", "tr"
    /// </item>
    /// <item>
    /// An end tag whose tag name is "table"
    /// <para>
    /// Parse error. Act as if an end tag with the tag name "caption" had been seen, then, if that token wasn't ignored, reprocess the current token.
    /// </para>
    /// <para>
    /// Note: The fake end tag token here can only be ignored in the fragment case.
    /// </para>
    /// </item>
    /// <item>
    /// An end tag whose tag name is one of: "body", "col", "colgroup", "html", "tbody", "td", "tfoot", "th", "thead", "tr"
    /// <para>
    /// Parse error. Ignore the token.
    /// </para>
    /// </item>
    /// <item>
    /// Anything else
    /// <para>
    /// Process the token using the rules for the "in body" insertion mode.
    /// </para>
    /// </item>
    /// </list>
    /// </remarks>
    internal class InCaptionState : ParserState
    {
        public override string Description
        {
            get { return "in caption"; }
        }

        protected override bool ProcessStartTagToken(TagToken tag, Parser parser)
        {
            bool tokenProcessed = false;

            // A start tag whose tag name is one of: "caption", "col", "colgroup", 
            // "tbody", "td", "tfoot", "th", "thead", "tr"
            // Parse error. Act as if an end tag with the tag name "caption" had been seen, 
            // then, if that token wasn't ignored, reprocess the current token.
            switch (tag.Name)
            {
                case HtmlElementFactory.CaptionElementTagName:
                case HtmlElementFactory.ColElementTagName:
                case HtmlElementFactory.ColGroupElementTagName:
                case HtmlElementFactory.TBodyElementTagName:
                case HtmlElementFactory.TDElementTagName:
                case HtmlElementFactory.TFootElementTagName:
                case HtmlElementFactory.THeadElementTagName:
                case HtmlElementFactory.THElementTagName:
                case HtmlElementFactory.TRElementTagName:
                    parser.LogParseError("Found start tag for '" + tag.Name + "' before close of caption", "adding end tag for caption, then reprocessing");
                    tokenProcessed = ProcessCaptionEndTag(new TagToken(TokenType.EndTag, HtmlElementFactory.CaptionElementTagName), parser, true);
                    break;
            }

            return tokenProcessed;
        }

        protected override bool ProcessEndTagToken(TagToken tag, Parser parser)
        {
            bool tokenProcessed = false;

            switch (tag.Name)
            {
                case HtmlElementFactory.CaptionElementTagName:
                    // An end tag whose tag name is "caption"
                    // If the stack of open elements does not have an element in table scope with the same tag 
                    // name as the token, this is a parse error. Ignore the token. (fragment case)
                    // Otherwise:
                    // Generate implied end tags.
                    // Now, if the current node is not a caption element, then this is a parse error.
                    // Pop elements from this stack until a caption element has been popped from the stack.
                    // Clear the list of active formatting elements up to the last marker.
                    // Switch the insertion mode to "in table".
                    ProcessCaptionEndTag(new TagToken(TokenType.EndTag, HtmlElementFactory.CaptionElementTagName), parser, false);
                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.TableElementTagName:
                    // An end tag whose tag name is "table"
                    // Parse error. Act as if an end tag with the tag name "caption" had been seen, then, 
                    // if that token wasn't ignored, reprocess the current token.
                    // Note: The fake end tag token here can only be ignored in the fragment case.
                    parser.LogParseError("Found end tag for '" + tag.Name + "' before close of caption", "adding end tag for caption, then reprocessing");
                    tokenProcessed = ProcessCaptionEndTag(new TagToken(TokenType.EndTag, HtmlElementFactory.CaptionElementTagName), parser, true);
                    break;

                case HtmlElementFactory.BodyElementTagName:
                case HtmlElementFactory.ColGroupElementTagName:
                case HtmlElementFactory.ColElementTagName:
                case HtmlElementFactory.HtmlElementTagName:
                case HtmlElementFactory.TBodyElementTagName:
                case HtmlElementFactory.TDElementTagName:
                case HtmlElementFactory.TFootElementTagName:
                case HtmlElementFactory.THElementTagName:
                case HtmlElementFactory.THeadElementTagName:
                case HtmlElementFactory.TRElementTagName:
                    // An end tag whose tag name is one of: "body", "col", "colgroup", "html", 
                    // "tbody", "td", "tfoot", "th", "thead", "tr"
                    // Parse error. Ignore the token.
                    parser.LogParseError("Found end tag for '" + tag.Name + "' before close of caption", "ignoring token");
                    tokenProcessed = true;
                    break;
            }

            return tokenProcessed;
        }

        protected override bool ProcessCharacterToken(CharacterToken character, Parser parser)
        {
            // Character tokens do not process the default behavior in "in body" state, so
            // we have to return false to fall into the "anything else" bucket to be processed
            // by the "in body" character token handler.
            return false;
        }

        protected override bool ProcessUnprocessedToken(Parser parser)
        {
            /// Anything else
            /// Process the token using the rules for the "in body" insertion mode.
            InBodyState temporaryState = new InBodyState(Description, false, true);
            bool tokenProcessed = temporaryState.ParseToken(parser);
            return tokenProcessed;
        }

        private bool ProcessCaptionEndTag(TagToken tag, Parser parser, bool reprocessTokenInNextState)
        {
            bool tokenProcessed = false;

            // An end tag whose tag name is "caption"
            // If the stack of open elements does not have an element in table scope with the same tag 
            // name as the token, this is a parse error. Ignore the token. (fragment case)
            // Otherwise:
            // Generate implied end tags.
            // Now, if the current node is not a caption element, then this is a parse error.
            // Pop elements from this stack until a caption element has been popped from the stack.
            // Clear the list of active formatting elements up to the last marker.
            // Switch the insertion mode to "in table".
            if (!parser.OpenElementStack.HasElementOfTypeInScope(tag.Name, ScopeType.Table))
            {
                parser.LogParseError("Open element stack does not have element in table scope with tag name '" + tag.Name + "'", "ignoring token");
            }
            else
            {
                parser.GenerateImpliedEndTags(string.Empty);
                if (parser.CurrentNode.Name != HtmlElementFactory.CaptionElementTagName)
                {
                    parser.LogParseError("Current element was not a '" + tag.Name + "' element", "none");
                }

                while (parser.CurrentNode.Name != HtmlElementFactory.CaptionElementTagName)
                {
                    parser.PopElementFromStack();
                }

                // Caption element should be current element on stack. Pop it in compliance with spec.
                parser.PopElementFromStack();
                parser.ActiveFormattingElementList.ClearToLastMarker();
                parser.AdvanceState(new InTableState());
                if (reprocessTokenInNextState)
                {
                    tokenProcessed = parser.State.ParseToken(parser);
                }
            }

            return tokenProcessed;
        }
    }
}
