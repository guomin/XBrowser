using System.Collections.Generic;
using XBrowserProject.Internal.Html.DOM.Parsing.HtmlTokenizer;

namespace XBrowserProject.Internal.Html.DOM.Parsing.States
{
    /// <summary>
    /// Represents the insertion mode of "in row"
    /// </summary>
    /// <remarks>
    /// When the insertion mode is "in row", tokens must be handled as follows:
    /// <list type="bullet">
    /// <item>
    /// A start tag whose tag name is one of: "th", "td"
    /// <para>
    /// Clear the stack back to a table row context. (See below.)
    /// </para>
    /// <para>
    /// Insert an HTML element for the token, then switch the insertion mode to "in cell".
    /// </para>
    /// <para>
    /// Insert a marker at the end of the list of active formatting elements.
    /// </para>
    /// </item>
    /// <item>
    /// An end tag whose tag name is "tr"
    /// <para>
    /// If the stack of open elements does not have an element in table scope with the same tag name as the token, this is a parse error. Ignore the token. (fragment case)
    /// </para>
    /// <para>
    /// Otherwise:
    /// </para>
    /// <para>
    /// Clear the stack back to a table row context. (See below.)
    /// </para>
    /// <para>
    /// Pop the current node (which will be a tr element) from the stack of open elements. Switch the insertion mode to "in table body".
    /// </para>
    /// </item>
    /// <item>
    /// A start tag whose tag name is one of: "caption", "col", "colgroup", "tbody", "tfoot", "thead", "tr"
    /// </item>
    /// <item>
    /// An end tag whose tag name is "table"
    /// <para>
    /// Act as if an end tag with the tag name "tr" had been seen, then, if that token wasn't ignored, reprocess the current token.
    /// </para>
    /// <para>
    /// Note: The fake end tag token here can only be ignored in the fragment case.
    /// </para>
    /// </item>
    /// <item>
    /// An end tag whose tag name is one of: "tbody", "tfoot", "thead"
    /// <para>
    /// If the stack of open elements does not have an element in table scope with the same tag name as the token, this is a parse error. Ignore the token.
    /// </para>
    /// <para>
    /// Otherwise, act as if an end tag with the tag name "tr" had been seen, then reprocess the current token.
    /// </para>
    /// </item>
    /// <item>
    /// An end tag whose tag name is one of: "body", "caption", "col", "colgroup", "html", "td", "th"
    /// <para>
    /// Parse error. Ignore the token.
    /// </para>
    /// </item>
    /// <item>
    /// Anything else
    /// <para>
    /// Process the token using the rules for the "in table" insertion mode.
    /// </para>
    /// <para>
    /// When the steps above require the UA to clear the stack back to a table row context, it means that the UA must, while the current node is not a tr element or an html element, pop elements from the stack of open elements.
    /// </para>
    /// <para>
    /// Note: The current node being an html element after this process is a fragment case.
    /// </para>
    /// </item>
    /// </list>
    /// </remarks>
    internal class InRowState : ParserState
    {
        public override string Description
        {
            get { return "in row"; }
        }

        protected override bool ProcessStartTagToken(TagToken tag, Parser parser)
        {
            bool tokenProcessed = false;
            switch (tag.Name)
            {
                case HtmlElementFactory.TDElementTagName:
                case HtmlElementFactory.THElementTagName:
                    // A start tag whose tag name is one of: "th", "td"
                    // Clear the stack back to a table row context. (See below.)
                    // Insert an HTML element for the token, then switch the insertion mode to "in cell".
                    // Insert a marker at the end of the list of active formatting elements.
                    ClearStackBackToTableRowContext(parser);
                    parser.InsertElement(tag, false, false, ActiveFormattingElementListState.AddAsMarker);
                    parser.AdvanceState(new InCellState());
                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.CaptionElementTagName:
                case HtmlElementFactory.ColGroupElementTagName:
                case HtmlElementFactory.ColElementTagName:
                case HtmlElementFactory.TBodyElementTagName:
                case HtmlElementFactory.TFootElementTagName:
                case HtmlElementFactory.THeadElementTagName:
                case HtmlElementFactory.TRElementTagName:
                    // A start tag whose tag name is one of: "caption", "col", "colgroup", 
                    // "tbody", "tfoot", "thead", "tr"
                    // Act as if an end tag with the tag name "tr" had been seen, then, if 
                    // that token wasn't ignored, reprocess the current token.
                    // Note: The fake end tag token here can only be ignored in the fragment case.
                    tokenProcessed = ProcessTableRowEndTag(new TagToken(TokenType.EndTag, HtmlElementFactory.TRElementTagName), parser, true);
                    break;
            }

            return tokenProcessed;
        }

        protected override bool ProcessEndTagToken(TagToken tag, Parser parser)
        {
            bool tokenProcessed = false;
            switch (tag.Name)
            {
                case HtmlElementFactory.TRElementTagName:
                    // An end tag whose tag name is "tr"
                    // If the stack of open elements does not have an element in table scope
                    // with the same tag name as the token, this is a parse error. Ignore the 
                    // token. (fragment case)
                    // Otherwise:
                    // Clear the stack back to a table row context. (See below.)
                    // Pop the current node (which will be a tr element) from the stack of 
                    // open elements. Switch the insertion mode to "in table body".
                    ProcessTableRowEndTag(tag, parser, false);
                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.TableElementTagName:
                    // An end tag whose tag name is "table"
                    // Act as if an end tag with the tag name "tr" had been seen, then, 
                    // if that token wasn't ignored, reprocess the current token.
                    // Note: The fake end tag token here can only be ignored in the fragment case.
                    tokenProcessed = ProcessTableRowEndTag(new TagToken(TokenType.EndTag, HtmlElementFactory.TRElementTagName), parser, true);
                    break;

                case HtmlElementFactory.TBodyElementTagName:
                case HtmlElementFactory.THeadElementTagName:
                case HtmlElementFactory.TFootElementTagName:
                    // An end tag whose tag name is one of: "tbody", "tfoot", "thead"
                    // If the stack of open elements does not have an element in table scope 
                    // with the same tag name as the token, this is a parse error. Ignore the token.
                    // Otherwise, act as if an end tag with the tag name "tr" had been seen, 
                    // then reprocess the current token.
                    if (!parser.OpenElementStack.HasElementOfTypeInScope(HtmlElementFactory.TBodyElementTagName, ScopeType.Table) &&
                        !parser.OpenElementStack.HasElementOfTypeInScope(HtmlElementFactory.THeadElementTagName, ScopeType.Table) &&
                        !parser.OpenElementStack.HasElementOfTypeInScope(HtmlElementFactory.TFootElementTagName, ScopeType.Table))
                    {
                        parser.LogParseError("Open element stack does not have 'tbody', 'thead', or 'tfoot' in table scope", "ignoring token");
                        tokenProcessed = true;
                    }
                    else
                    {
                        tokenProcessed = ProcessTableRowEndTag(new TagToken(TokenType.EndTag, HtmlElementFactory.TRElementTagName), parser, true);
                    }
                    break;

                case HtmlElementFactory.BodyElementTagName:
                case HtmlElementFactory.CaptionElementTagName:
                case HtmlElementFactory.ColGroupElementTagName:
                case HtmlElementFactory.ColElementTagName:
                case HtmlElementFactory.HtmlElementTagName:
                case HtmlElementFactory.THElementTagName:
                case HtmlElementFactory.TDElementTagName:
                    // An end tag whose tag name is one of: "body", "caption", "col", "colgroup",
                    // "html", "td", "th"
                    // Parse error. Ignore the token.
                    parser.LogParseError("Found '" + tag.Name + "' end tag token in '" + Description + "'", "ignoring token");
                    tokenProcessed = true;
                    break;
            }

            return tokenProcessed;


        }

        protected override bool ProcessUnprocessedToken(Parser parser)
        {
            // Anything else
            // Process the token using the rules for the "in table" insertion mode.
            // Note: The current node being an html element after this process is a fragment case.
            InTableState temporaryState = new InTableState(Description);
            bool tokenProcessed = temporaryState.ParseToken(parser);
            return tokenProcessed;
        }

        protected override bool ProcessCharacterToken(CharacterToken character, Parser parser)
        {
            // Anything else
            // Process the token using the rules for the "in table" insertion mode.
            // These are the rules for "in table" character tokens.
            // A character token
            // Let the pending table character tokens be an empty list of tokens.
            // Let the original insertion mode be the current insertion mode.
            // Switch the insertion mode to "in table text" and reprocess the token.
            parser.PendingTableCharacterTokens = new List<CharacterToken>();
            parser.AdvanceState(new InTableTextState(this));
            return parser.State.ParseToken(parser);
        }

        private bool ProcessTableRowEndTag(TagToken tag, Parser parser, bool reprocessTokenInNextState)
        {
            bool tokenProcessed = false;
            if (!parser.OpenElementStack.HasElementOfTypeInScope(tag.Name, ScopeType.Table))
            {
                parser.LogParseError("Open element stack does not have a '" + tag.Name + "' element in table scope", "ignoring token");
                tokenProcessed = true;
            }
            else
            {
                ClearStackBackToTableRowContext(parser);
                parser.PopElementFromStack();
                parser.AdvanceState(new InTableBodyState());
                if (reprocessTokenInNextState)
                {
                    tokenProcessed = parser.State.ParseToken(parser);
                }
            }

            return tokenProcessed;
        }

        private void ClearStackBackToTableRowContext(Parser parser)
        {
            // When the steps above require the UA to clear the stack back to a table 
            // row context, it means that the UA must, while the current node is not a 
            // tr element or an html element, pop elements from the stack of open elements.
            while (parser.CurrentNode.Name != HtmlElementFactory.TRElementTagName &&
                parser.CurrentNode.Name != HtmlElementFactory.HtmlElementTagName)
            {
                parser.PopElementFromStack();
            }
        }
    }
}
