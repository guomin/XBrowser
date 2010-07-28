using XBrowserProject.BrowserModel.Internal;
using XBrowserProject.HtmlDom;
using XBrowserProject.HtmlParser.HtmlTokenizer;

namespace XBrowserProject.HtmlParser.States
{
    /// <summary>
    /// Represents the insertion mode of "in cell"
    /// </summary>
    /// <remarks>
    /// When the insertion mode is "in cell", tokens must be handled as follows:
    /// <list type="bullet">
    /// <item>
    /// An end tag whose tag name is one of: "td", "th"
    /// <para>
    /// If the stack of open elements does not have an element in table scope with the same tag name as that of the token, then this is a parse error and the token must be ignored.
    /// </para>
    /// <para>
    /// Otherwise:
    /// </para>
    /// <para>
    /// Generate implied end tags.
    /// </para>
    /// <para>
    /// Now, if the current node is not an element with the same tag name as the token, then this is a parse error.
    /// </para>
    /// <para>
    /// Pop elements from the stack of open elements stack until an element with the same tag name as the token has been popped from the stack.
    /// </para>
    /// <para>
    /// Clear the list of active formatting elements up to the last marker.
    /// </para>
    /// <para>
    /// Switch the insertion mode to "in row". (The current node will be a tr element at this point.)
    /// </para>
    /// </item>
    /// <item>
    /// A start tag whose tag name is one of: "caption", "col", "colgroup", "tbody", "td", "tfoot", "th", "thead", "tr"
    /// <para>
    /// If the stack of open elements does not have a td or th element in table scope, then this is a parse error; ignore the token. (fragment case)
    /// </para>
    /// <para>
    /// Otherwise, close the cell (see below) and reprocess the current token.
    /// </para>
    /// </item>
    /// <item>
    /// An end tag whose tag name is one of: "body", "caption", "col", "colgroup", "html"
    /// <para>
    /// Parse error. Ignore the token.
    /// </para>
    /// </item>
    /// <item>
    /// An end tag whose tag name is one of: "table", "tbody", "tfoot", "thead", "tr"
    /// <para>
    /// If the stack of open elements does not have an element in table scope with the same tag name as that of the token (which can only happen for "tbody", "tfoot" and "thead", or, in the fragment case), then this is a parse error and the token must be ignored.
    /// </para>
    /// <para>
    /// Otherwise, close the cell (see below) and reprocess the current token.
    /// </para>
    /// </item>
    /// <item>
    /// Anything else
    /// <para>
    /// Process the token using the rules for the "in body" insertion mode.
    /// </para>
    /// <para>
    /// Where the steps above say to close the cell, they mean to run the following algorithm:
    /// </para>
    /// <para>
    /// 1.If the stack of open elements has a td element in table scope, then act as if an end tag token with the tag name "td" had been seen.
    /// </para>
    /// <para>
    /// 2.Otherwise, the stack of open elements will have a th element in table scope; act as if an end tag token with the tag name "th" had been seen.
    /// </para>
    /// </item>
    /// </list>
    /// </remarks>
    public class InCellState : ParserState
    {
        public override string Description
        {
            get { return "in cell"; }
        }


        protected override bool ProcessStartTagToken(TagToken tag, Parser parser)
        {
            bool tokenProcessed = false;
            switch (tag.Name)
            {
                case HtmlElementFactory.CaptionElementTagName:
                case HtmlElementFactory.ColElementTagName:
                case HtmlElementFactory.ColGroupElementTagName:
                case HtmlElementFactory.TBodyElementTagName:
                case HtmlElementFactory.TFootElementTagName:
                case HtmlElementFactory.THeadElementTagName:
                case HtmlElementFactory.TDElementTagName:
                case HtmlElementFactory.THElementTagName:
                case HtmlElementFactory.TRElementTagName:
                    // A start tag whose tag name is one of: "caption", "col", "colgroup", "tbody", "td", 
                    // "tfoot", "th", "thead", "tr"
                    // If the stack of open elements does not have a td or th element in table scope, 
                    // then this is a parse error; ignore the token. (fragment case)
                    // Otherwise, close the cell (see below) and reprocess the current token.
                    if (!parser.OpenElementStack.HasElementOfTypeInScope(HtmlElementFactory.TDElementTagName, ScopeType.Table) &&
                        !parser.OpenElementStack.HasElementOfTypeInScope(HtmlElementFactory.THElementTagName, ScopeType.Table))
                    {
                        parser.LogParseError("Open element stack does not have 'td' or 'th' in table scope", "ignoring token");
                    }
                    else
                    {
                        CloseCell(parser, true);
                    }

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
                case HtmlElementFactory.TDElementTagName:
                case HtmlElementFactory.THElementTagName:
                    // An end tag whose tag name is one of: "td", "th"
                    // If the stack of open elements does not have an element in table scope with the same 
                    // tag name as that of the token, then this is a parse error and the token must be ignored.
                    // Otherwise:
                    // Generate implied end tags.
                    // Now, if the current node is not an element with the same tag name as the token, 
                    // then this is a parse error.
                    // Pop elements from the stack of open elements stack until an element with the same 
                    // tag name as the token has been popped from the stack.
                    // Clear the list of active formatting elements up to the last marker.
                    // Switch the insertion mode to "in row". (The current node will be a tr element at this point.)
                    ProcessDataCellEndTag(tag, parser, false);
                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.BodyElementTagName:
                case HtmlElementFactory.CaptionElementTagName:
                case HtmlElementFactory.ColElementTagName:
                case HtmlElementFactory.ColGroupElementTagName:
                case HtmlElementFactory.HtmlElementTagName:
                    // An end tag whose tag name is one of: "body", "caption", "col", "colgroup", "html"
                    // Parse error. Ignore the token.
                    parser.LogParseError("Found '" + tag.Name + "' end tag token in '" + Description + "'", "ignoring token");
                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.TableElementTagName:
                case HtmlElementFactory.TBodyElementTagName:
                case HtmlElementFactory.TFootElementTagName:
                case HtmlElementFactory.THeadElementTagName:
                case HtmlElementFactory.TRElementTagName:
                    // An end tag whose tag name is one of: "table", "tbody", "tfoot", "thead", "tr"
                    // If the stack of open elements does not have an element in table scope with the 
                    // same tag name as that of the token (which can only happen for "tbody", "tfoot" 
                    // and "thead", or, in the fragment case), then this is a parse error and the token 
                    // must be ignored.
                    // Otherwise, close the cell (see below) and reprocess the current token.
                    if (!parser.OpenElementStack.HasElementOfTypeInScope(tag.Name, ScopeType.Table))
                    {
                        parser.LogParseError("Did not have '" + tag.Name + "' element in table scope", "ignoring token");
                        tokenProcessed = true;
                    }
                    else
                    {
                        tokenProcessed = CloseCell(parser, true);
                    }

                    break;
            }

            return tokenProcessed;
        }

        protected override bool ProcessUnprocessedToken(Parser parser)
        {
            // Anything else
            // Process the token using the rules for the "in body" insertion mode.
            InBodyState temporaryState = new InBodyState(Description, false, true);
            bool tokenProcessed = temporaryState.ParseToken(parser);
            return tokenProcessed;
        }

        protected override bool ProcessCharacterToken(CharacterToken character, Parser parser)
        {
            // Character tokens do not process the default behavior in "in body" state, so
            // we have to return false to fall into the "anything else" bucket to be processed
            // by the "in body" character token handler.
            return false;
        }

        private bool ProcessDataCellEndTag(TagToken tag, Parser parser, bool reprocessTokenInNextState)
        {
            bool tokenProcessed = false;

            // If the stack of open elements does not have an element in table scope 
            // with the same tag name as that of the token, then this is a parse error 
            // and the token must be ignored.
            if (!parser.OpenElementStack.HasElementOfTypeInScope(tag.Name, ScopeType.Table))
            {
                parser.LogParseError("Open element stack does not have '" + tag.Name + "' in table scope", "ignoring token");
            }
            else
            {
                // Otherwise:
                // Generate implied end tags.
                parser.GenerateImpliedEndTags(string.Empty);

                // Now, if the current node is not an element with the same tag name
                // as the token, then this is a parse error.
                if (parser.CurrentNode.Name != tag.Name)
                {
                    parser.LogParseError("Current node is not '" + tag.Name + "' after generating implied end tags", "none");
                }

                // Pop elements from the stack of open elements stack until an element with the same 
                // tag name as the token has been popped from the stack.
                while (parser.CurrentNode.Name != tag.Name)
                {
                    parser.PopElementFromStack();
                }

                parser.PopElementFromStack();

                // Clear the list of active formatting elements up to the last marker.
                parser.ActiveFormattingElementList.ClearToLastMarker();

                // Switch the insertion mode to "in row". (The current node will be a tr 
                // element at this point.)
                parser.AdvanceState(new InRowState());
                if (reprocessTokenInNextState)
                {
                    tokenProcessed = parser.State.ParseToken(parser);
                }
            }

            return tokenProcessed;
        }

        private bool CloseCell(Parser parser, bool reprocessTokenInNextState)
        {
            bool tokenProcessed = false;

            // Where the steps above say to close the cell, they mean to run the following algorithm:
            // 1.If the stack of open elements has a td element in table scope, then act as if an end 
            // tag token with the tag name "td" had been seen.
            // 2.Otherwise, the stack of open elements will have a th element in table scope; act as 
            // if an end tag token with the tag name "th" had been seen.
            string elementName = string.Empty;
            if (parser.OpenElementStack.HasElementOfTypeInScope(HtmlElementFactory.TDElementTagName, ScopeType.Table))
            {
                elementName = HtmlElementFactory.TDElementTagName;
            }
            else
            {
                elementName = HtmlElementFactory.THElementTagName;
            }

            tokenProcessed = ProcessDataCellEndTag(new TagToken(TokenType.EndTag, elementName), parser, reprocessTokenInNextState);
            return tokenProcessed;
        }
    }
}
