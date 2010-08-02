using XBrowserProject.BrowserModel.Internal;
using XBrowserProject.HtmlDom;
using XBrowserProject.HtmlParser.HtmlTokenizer;

namespace XBrowserProject.HtmlParser.States
{
    /// <summary>
    /// Represents the insertion mode of "in table body"
    /// </summary>
    /// <remarks>
    /// When the insertion mode is "in table body", tokens must be handled as follows:
    /// <list type="bullet">
    /// <item>
    /// A start tag whose tag name is "tr"
    /// <para>
    /// Clear the stack back to a table body context. (See below.)
    /// </para>
    /// <para>
    /// Insert an HTML element for the token, then switch the insertion mode to "in row".
    /// </para>
    /// </item>
    /// <item>
    /// A start tag whose tag name is one of: "th", "td"
    /// <para>
    /// Parse error. Act as if a start tag with the tag name "tr" had been seen, then reprocess the current token.
    /// </para>
    /// </item>
    /// <item>
    /// An end tag whose tag name is one of: "tbody", "tfoot", "thead"
    /// <para>
    /// If the stack of open elements does not have an element in table scope with the same tag name as the token, this is a parse error. Ignore the token.
    /// </para>
    /// <para>
    /// Otherwise:
    /// </para>
    /// <para>
    /// Clear the stack back to a table body context. (See below.)
    /// </para>
    /// <para>
    /// Pop the current node from the stack of open elements. Switch the insertion mode to "in table".
    /// </para>
    /// </item>
    /// <item>
    /// A start tag whose tag name is one of: "caption", "col", "colgroup", "tbody", "tfoot", "thead"
    /// </item>
    /// <item>
    /// An end tag whose tag name is "table"
    /// <para>
    /// If the stack of open elements does not have a tbody, thead, or tfoot element in table scope, this is a parse error. Ignore the token. (fragment case)
    /// </para>
    /// <para>
    /// Otherwise:
    /// </para>
    /// <para>
    /// Clear the stack back to a table body context. (See below.)
    /// </para>
    /// <para>
    /// Act as if an end tag with the same tag name as the current node ("tbody", "tfoot", or "thead") had been seen, then reprocess the current token.
    /// </para>
    /// </item>
    /// <item>
    /// An end tag whose tag name is one of: "body", "caption", "col", "colgroup", "html", "td", "th", "tr"
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
    /// When the steps above require the UA to clear the stack back to a table body context, it means that the UA must, while the current node is not a tbody, tfoot, thead, or html element, pop elements from the stack of open elements.
    /// </para>
    /// <para>
    /// Note: The current node being an html element after this process is a fragment case.
    /// </para>
    /// </item>
    /// </list>
    ///</remarks>
    internal class InTableBodyState : ParserState
    {
        public override string Description
        {
            get { return "in table body"; }
        }

        protected override bool ProcessStartTagToken(TagToken tag, Parser parser)
        {
            bool tokenProcessed = false;

            switch (tag.Name)
            {
                case HtmlElementFactory.TRElementTagName:
                    // A start tag whose tag name is "tr"
                    // Clear the stack back to a table body context. (See below.)
                    // Insert an HTML element for the token, then switch the insertion mode to "in row".
                    ProcessTableRowStartTag(tag, parser, false);
                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.TDElementTagName:
                case HtmlElementFactory.THElementTagName:
                    // A start tag whose tag name is one of: "th", "td"
                    // Parse error. Act as if a start tag with the tag name "tr" had been seen, then 
                    // reprocess the current token.
                    parser.LogParseError("Found '" + tag.Name + "' start tag in '" + Description + "' state", "inserting tr start token and reprocessing");
                    tokenProcessed = ProcessTableRowStartTag(new TagToken(TokenType.StartTag, HtmlElementFactory.TRElementTagName), parser, true);
                    break;

                case HtmlElementFactory.CaptionElementTagName:
                case HtmlElementFactory.ColElementTagName:
                case HtmlElementFactory.ColGroupElementTagName:
                case HtmlElementFactory.TBodyElementTagName:
                case HtmlElementFactory.THeadElementTagName:
                case HtmlElementFactory.TFootElementTagName:
                    // A start tag whose tag name is one of: "caption", "col", "colgroup", "tbody", "tfoot", "thead"
                    // If the stack of open elements does not have a tbody, thead, or tfoot element 
                    // in table scope, this is a parse error. Ignore the token. (fragment case)
                    // Otherwise:
                    // Clear the stack back to a table body context. (See below.)
                    // Act as if an end tag with the same tag name as the current node ("tbody", 
                    // "tfoot", or "thead") had been seen, then reprocess the current token.
                    if (!parser.OpenElementStack.HasElementOfTypeInScope(HtmlElementFactory.THeadElementTagName, ScopeType.Table) &&
                        !parser.OpenElementStack.HasElementOfTypeInScope(HtmlElementFactory.TBodyElementTagName, ScopeType.Table) &&
                        !parser.OpenElementStack.HasElementOfTypeInScope(HtmlElementFactory.TFootElementTagName, ScopeType.Table))
                    {
                        parser.LogParseError("Open element stack does not have 'tbody', 'thead', or 'tfoot' in table scope", "ignoring token");
                        tokenProcessed = true;
                    }
                    else
                    {
                        ClearStackBackToTableBodyContext(parser);
                        tokenProcessed = ProcessTableBodyEndTag(new TagToken(TokenType.EndTag, parser.CurrentNode.Name), parser, true);
                    }

                    break;
            }


            return tokenProcessed;
        }

        protected override bool ProcessEndTagToken(TagToken tag, Parser parser)
        {
            bool tokenProcessed = false;
            switch (tag.Name)
            {
                case HtmlElementFactory.TBodyElementTagName:
                case HtmlElementFactory.THeadElementTagName:
                case HtmlElementFactory.TFootElementTagName:
                    // An end tag whose tag name is one of: "tbody", "tfoot", "thead"
                    // If the stack of open elements does not have an element in table scope with the 
                    // same tag name as the token, this is a parse error. Ignore the token.
                    // Otherwise:
                    // Clear the stack back to a table body context. (See below.)
                    // Pop the current node from the stack of open elements. Switch the insertion 
                    // mode to "in table".
                    if (!parser.OpenElementStack.HasElementOfTypeInScope(HtmlElementFactory.THeadElementTagName, ScopeType.Table) &&
                        !parser.OpenElementStack.HasElementOfTypeInScope(HtmlElementFactory.TBodyElementTagName, ScopeType.Table) &&
                        !parser.OpenElementStack.HasElementOfTypeInScope(HtmlElementFactory.TFootElementTagName, ScopeType.Table))
                    {
                        parser.LogParseError("Open element stack does not have 'tbody', 'thead', or 'tfoot' in table scope", "ignoring token");
                    }
                    else
                    {
                        ClearStackBackToTableBodyContext(parser);
                        ProcessTableBodyEndTag(tag, parser, false);
                    }

                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.TableElementTagName:
                    // An end tag whose tag name is "table"
                    // If the stack of open elements does not have a tbody, thead, or tfoot element 
                    // in table scope, this is a parse error. Ignore the token. (fragment case)
                    // Otherwise:
                    // Clear the stack back to a table body context. (See below.)
                    // Act as if an end tag with the same tag name as the current node ("tbody",
                    // "tfoot", or "thead") had been seen, then reprocess the current token.
                    if (!parser.OpenElementStack.HasElementOfTypeInScope(HtmlElementFactory.THeadElementTagName, ScopeType.Table) &&
                        !parser.OpenElementStack.HasElementOfTypeInScope(HtmlElementFactory.TBodyElementTagName, ScopeType.Table) &&
                        !parser.OpenElementStack.HasElementOfTypeInScope(HtmlElementFactory.TFootElementTagName, ScopeType.Table))
                    {
                        parser.LogParseError("Open element stack does not have 'tbody', 'thead', or 'tfoot' in table scope", "ignoring token");
                        tokenProcessed = true;
                    }
                    else
                    {
                        ClearStackBackToTableBodyContext(parser);
                        tokenProcessed = ProcessTableBodyEndTag(new TagToken(TokenType.EndTag, parser.CurrentNode.Name), parser, true);
                    }

                    break;

                case HtmlElementFactory.BodyElementTagName:
                case HtmlElementFactory.CaptionElementTagName:
                case HtmlElementFactory.ColElementTagName:
                case HtmlElementFactory.ColGroupElementTagName:
                case HtmlElementFactory.HtmlElementTagName:
                case HtmlElementFactory.TDElementTagName:
                case HtmlElementFactory.THElementTagName:
                case HtmlElementFactory.TRElementTagName:
                    // An end tag whose tag name is one of: "body", "caption", "col", "colgroup", 
                    // "html", "td", "th", "tr"
                    // Parse error. Ignore the token.
                    parser.LogParseError("Found '" + tag.Name + "' end tag token in '" + Description + "' state", "ignoring token");
                    break;
            }

            return tokenProcessed;
        }

        protected override bool ProcessCharacterToken(CharacterToken character, Parser parser)
        {
            // Character tokens do not process the default behavior in "in table" state, so
            // we have to return false to fall into the "anything else" bucket to be processed
            // by the "in table" character token handler.
            return false;
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

        private bool ProcessTableRowStartTag(TagToken tag, Parser parser, bool reprocessCurrentTokenInNextState)
        {
            bool tokenProcessed = true;
            ClearStackBackToTableBodyContext(parser);
            parser.InsertElement(tag);
            parser.AdvanceState(new InRowState());
            if (reprocessCurrentTokenInNextState)
            {
                parser.State.ParseToken(parser);
            }

            return tokenProcessed;
        }

        private bool ProcessTableBodyEndTag(TagToken tag, Parser parser, bool reprocessCurrentTokenInNextState)
        {
            bool tokenProcessed = true;
            parser.PopElementFromStack();
            parser.AdvanceState(new InTableState());
            if (reprocessCurrentTokenInNextState)
            {
                tokenProcessed = parser.State.ParseToken(parser);
            }

            return tokenProcessed;
        }

        private void ClearStackBackToTableBodyContext(Parser parser)
        {
            // When the steps [...] require the UA to clear the stack back to a table
            // body context, it means that the UA must, while the current node is not a 
            // tbody, tfoot, thead, or html element, pop elements from the stack of open 
            // elements.
            while (parser.CurrentNode.Name != HtmlElementFactory.TBodyElementTagName &&
                parser.CurrentNode.Name != HtmlElementFactory.TFootElementTagName &&
                parser.CurrentNode.Name != HtmlElementFactory.THeadElementTagName &&
                parser.CurrentNode.Name != HtmlElementFactory.HtmlElementTagName)
            {
                parser.PopElementFromStack();
            }
        }
    }
}
