using System;
using System.Collections.Generic;
using XBrowserProject.BrowserModel.Internal;
using XBrowserProject.HtmlDom;
using XBrowserProject.HtmlParser.HtmlTokenizer;

namespace XBrowserProject.HtmlParser.States
{
    /// <summary>
    /// Represents the insertion mode of "in table"
    /// </summary>
    /// <remarks>
    /// When the insertion mode is "in table", tokens must be handled as follows:
    /// <list type="bullet">
    /// <item>
    /// A character token
    /// <para>
    /// Let the pending table character tokens be an empty list of tokens.
    /// </para>
    /// <para>
    /// Let the original insertion mode be the current insertion mode.
    /// </para>
    /// <para>
    /// Switch the insertion mode to "in table text" and reprocess the token.
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
    /// A start tag whose tag name is "caption"
    /// <para>
    /// </para>
    /// Clear the stack back to a table context. (See below.)
    /// <para>
    /// Insert a marker at the end of the list of active formatting elements.
    /// </para>
    /// <para>
    /// Insert an HTML element for the token, then switch the insertion mode to "in caption".
    /// </para>
    /// </item>
    /// <item>
    /// A start tag whose tag name is "colgroup"
    /// <para>
    /// Clear the stack back to a table context. (See below.)
    /// </para>
    /// <para>
    /// Insert an HTML element for the token, then switch the insertion mode to "in column group".
    /// </para>
    /// </item>
    /// <item>
    /// A start tag whose tag name is "col"
    /// <para>
    /// Act as if a start tag token with the tag name "colgroup" had been seen, then reprocess the current token.
    /// </para>
    /// </item>
    /// <item>
    /// A start tag whose tag name is one of: "tbody", "tfoot", "thead"
    /// <para>
    /// Clear the stack back to a table context. (See below.)
    /// </para>
    /// <para>
    /// Insert an HTML element for the token, then switch the insertion mode to "in table body".
    /// </para>
    /// </item>
    /// <item>
    /// A start tag whose tag name is one of: "td", "th", "tr"
    /// <para>
    /// Act as if a start tag token with the tag name "tbody" had been seen, then reprocess the current token.
    /// </para>
    /// </item>
    /// <item>
    /// A start tag whose tag name is "table"
    /// <para>
    /// Parse error. Act as if an end tag token with the tag name "table" had been seen, then, if that token wasn't ignored, reprocess the current token.
    /// </para>
    /// <para>
    /// Note: The fake end tag token here can only be ignored in the fragment case.
    /// </para>
    /// </item>
    /// <item>
    /// An end tag whose tag name is "table"
    /// <para>
    /// If the stack of open elements does not have an element in table scope with the same tag name as the token, this is a parse error. Ignore the token. (fragment case)
    /// </para>
    /// <para>
    /// Otherwise:
    /// </para>
    /// <para>
    /// Pop elements from this stack until a table element has been popped from the stack.
    /// </para>
    /// <para>
    /// Reset the insertion mode appropriately.
    /// </para>
    /// </item>
    /// <item>
    /// An end tag whose tag name is one of: "body", "caption", "col", "colgroup", "html", "tbody", "td", "tfoot", "th", "thead", "tr"
    /// <para>
    /// Parse error. Ignore the token.
    /// </para>
    /// </item>
    /// <item>
    /// A start tag whose tag name is one of: "style", "script"
    /// <para>
    /// Process the token using the rules for the "in head" insertion mode.
    /// </para>
    /// </item>
    /// <item>
    /// A start tag whose tag name is "input"
    /// <para>
    /// If the token does not have an attribute with the name "type", or if it does, but that attribute's value is not an ASCII case-insensitive match for the string "hidden", then: act as described in the "anything else" entry below.
    /// </para>
    /// Otherwise:
    /// <para>
    /// Parse error.
    /// </para>
    /// <para>
    /// Insert an HTML element for the token.
    /// </para>
    /// <para>
    /// Pop that input element off the stack of open elements.
    /// </para>
    /// </item>
    /// <item>
    /// A start tag whose tag name is "form"
    /// <para>
    /// Parse error.
    /// </para>
    /// <para>
    /// If the form element pointer is not null, ignore the token.
    /// </para>
    /// <para>
    /// Otherwise:
    /// </para>
    /// <para>
    /// Insert an HTML element for the token.
    /// </para>
    /// <para>
    /// Pop that form element off the stack of open elements.
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
    /// Parse error. Process the token using the rules for the "in body" insertion mode, except that if the current node is a table, tbody, tfoot, thead, or tr element, then, whenever a node would be inserted into the current node, it must instead be foster parented.
    /// </para>
    /// <para>
    /// When the steps above require the UA to clear the stack back to a table context, it means that the UA must, while the current node is not a table element or an html element, pop elements from the stack of open elements.
    /// </para>
    /// <para>
    /// Note: The current node being an html element after this process is a fragment case.
    /// </para>
    /// </item>
    /// </list>
    /// </remarks>
    internal class InTableState : ParserState
    {
        private string description = "in table";

        public InTableState()
            : this(string.Empty)
        {
        }

        public InTableState(string actualDescription)
        {
            if (!string.IsNullOrEmpty(actualDescription))
            {
                description = actualDescription;
            }
        }

        public override string Description
        {
            get { return description; }
        }

        protected override bool ProcessStartTagToken(TagToken tag, Parser parser)
        {
            bool tokenProcessed = false;

            switch (tag.Name)
            {
                case HtmlElementFactory.CaptionElementTagName:
                    // A start tag whose tag name is "caption"
                    // Clear the stack back to a table context. (See below.)
                    // Insert a marker at the end of the list of active formatting elements.
                    // Insert an HTML element for the token, then switch the insertion mode to "in caption".
                    ClearStackBackToTableContext(parser);
                    parser.InsertElement(tag, false, false, ActiveFormattingElementListState.AddAsMarker);
                    parser.AdvanceState(new InCaptionState());
                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.ColGroupElementTagName:
                    // A start tag whose tag name is "colgroup"
                    // Clear the stack back to a table context. (See below.)
                    // Insert an HTML element for the token, then switch the insertion mode to "in column group".
                    ProcessColGroupStartTag(tag, parser, false);
                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.ColElementTagName:
                    // A start tag whose tag name is "col"
                    // Act as if a start tag token with the tag name "colgroup" had been seen, then reprocess the current token.
                    tokenProcessed = ProcessColGroupStartTag(new TagToken(TokenType.StartTag, HtmlElementFactory.ColGroupElementTagName), parser, true);
                    break;

                case HtmlElementFactory.TBodyElementTagName:
                case HtmlElementFactory.TFootElementTagName:
                case HtmlElementFactory.THeadElementTagName:
                    // A start tag whose tag name is one of: "tbody", "tfoot", "thead"
                    // Clear the stack back to a table context. (See below.)
                    // Insert an HTML element for the token, then switch the insertion mode to "in table body".
                    ProcessTBodyStartTag(tag, parser, false);
                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.TDElementTagName:
                case HtmlElementFactory.THElementTagName:
                case HtmlElementFactory.TRElementTagName:
                    // A start tag whose tag name is one of: "td", "th", "tr"
                    // Act as if a start tag token with the tag name "tbody" had been seen, then reprocess the current token.
                    tokenProcessed = ProcessTBodyStartTag(new TagToken(TokenType.StartTag, HtmlElementFactory.TBodyElementTagName), parser, true);
                    break;

                case HtmlElementFactory.TableElementTagName:
                    // A start tag whose tag name is "table"
                    // Parse error. Act as if an end tag token with the tag name "table" had been
                    // seen, then, if that token wasn't ignored, reprocess the current token.
                    // Note: The fake end tag token here can only be ignored in the fragment case.
                    ProcessTableEndTag(new TagToken(TokenType.EndTag, HtmlElementFactory.TableElementTagName), parser, true);
                    break;

                case HtmlElementFactory.StyleElementTagName:
                case HtmlElementFactory.ScriptElementTagName:
                    // A start tag whose tag name is one of: "style", "script"
                    // Process the token using the rules for the "in head" insertion mode.
                    InHeadState temporaryState = new InHeadState(Description);
                    temporaryState.ParseToken(parser);
                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.InputElementTagName:
                    // A start tag whose tag name is "input"
                    // If the token does not have an attribute with the name "type", or if it does, 
                    // but that attribute's value is not an ASCII case-insensitive match for the string 
                    // "hidden", then: act as described in the "anything else" entry below.
                    // Otherwise:
                    // Parse error.
                    // Insert an HTML element for the token.
                    // Pop that input element off the stack of open elements.
                    if (tag.Attributes.ContainsKey("type") && string.Compare(tag.Attributes["type"].Value, "hidden", StringComparison.OrdinalIgnoreCase) == 0) 
                    {
                        parser.LogParseError("Found 'input' start tag in '" + Description + "' insertion mode", "inserting element");
                        parser.InsertElement(tag, true);
                        tokenProcessed = true;
                    }
                    break;

                case HtmlElementFactory.FormElementTagName:
                    // A start tag whose tag name is "form"
                    // Parse error.
                    // If the form element pointer is not null, ignore the token.
                    // Otherwise:
                    // Insert an HTML element for the token.
                    // Pop that form element off the stack of open elements.
                    string action = "ignoring token";
                    if (parser.FormElement == null)
                    {
                        action = "inserting element and popping off stack";
                        parser.InsertElement(tag, true);
                    }

                    parser.LogParseError("Found 'form' start tag in '" + Description + "' insertion mode", action);
                    tokenProcessed = true;
                    break;
            }

            return tokenProcessed;
        }
        // An end-of-file token
        // If the current node is not the root html element, then this is a parse error.
        // Note: It can only be the current node in the fragment case.
        // Stop parsing.

        protected override bool ProcessEndTagToken(TagToken tag, Parser parser)
        {
            bool tokenProcessed = false;

            switch (tag.Name)
            {
                case HtmlElementFactory.BodyElementTagName:
                case HtmlElementFactory.CaptionElementTagName:
                case HtmlElementFactory.ColElementTagName:
                case HtmlElementFactory.ColGroupElementTagName:
                case HtmlElementFactory.HtmlElementTagName:
                case HtmlElementFactory.TBodyElementTagName:
                case HtmlElementFactory.TDElementTagName:
                case HtmlElementFactory.TFootElementTagName:
                case HtmlElementFactory.THElementTagName:
                case HtmlElementFactory.THeadElementTagName:
                case HtmlElementFactory.TRElementTagName:
                    // An end tag whose tag name is one of: "body", "caption", "col", "colgroup", 
                    // "html", "tbody", "td", "tfoot", "th", "thead", "tr"
                    // Parse error. Ignore the token.
                    parser.LogParseError("Found '" + tag.Name + "' end tag in '" + Description + "' insertion mode", "ignoring token");
                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.TableElementTagName:
                    // An end tag whose tag name is "table"
                    // If the stack of open elements does not have an element in table scope with the same 
                    // tag name as the token, this is a parse error. Ignore the token. (fragment case)
                    // Otherwise:
                    // Pop elements from this stack until a table element has been popped from the stack.
                    // Reset the insertion mode appropriately.
                    ProcessTableEndTag(tag, parser, false);
                    tokenProcessed = true;
                    break;
            }


            return tokenProcessed;
        }

        protected override bool ProcessUnprocessedToken(Parser parser)
        {
            bool tokenProcessed = false;
            // Anything else
            // Parse error. Process the token using the rules for the "in body" insertion mode, 
            // except that if the current node is a table, tbody, tfoot, thead, or tr element, 
            // then, whenever a node would be inserted into the current node, it must instead 
            // be foster parented.
            parser.LogParseError("Unhandled token '" + parser.CurrentToken.TokenType.ToString() + "' in '" + Description + "' mode", "processing using 'in body' rules");
            bool fosterParentingRequired = parser.CurrentNode.Name == HtmlElementFactory.TableElementTagName ||
                parser.CurrentNode.Name == HtmlElementFactory.TBodyElementTagName ||
                parser.CurrentNode.Name == HtmlElementFactory.TFootElementTagName ||
                parser.CurrentNode.Name == HtmlElementFactory.THeadElementTagName ||
                parser.CurrentNode.Name == HtmlElementFactory.TRElementTagName;

            InBodyState temporaryState = new InBodyState(Description, fosterParentingRequired, true);
            tokenProcessed = temporaryState.ParseToken(parser);
            return tokenProcessed;
        }

        protected override bool ProcessCharacterToken(CharacterToken character, Parser parser)
        {
            // A character token
            // Let the pending table character tokens be an empty list of tokens.
            // Let the original insertion mode be the current insertion mode.
            // Switch the insertion mode to "in table text" and reprocess the token.
            parser.PendingTableCharacterTokens = new List<CharacterToken>();
            parser.AdvanceState(new InTableTextState(this));
            return parser.State.ParseToken(parser);
        }

        private bool ProcessTBodyStartTag(TagToken tag, Parser parser, bool reprocessTokenInNextState)
        {
            bool tokenProcessed = false;
            ClearStackBackToTableContext(parser);
            parser.InsertElement(tag);
            parser.AdvanceState(new InTableBodyState());
            if (reprocessTokenInNextState)
            {
                tokenProcessed = parser.State.ParseToken(parser);
            }

            return tokenProcessed;
        }

        private bool ProcessColGroupStartTag(TagToken tag, Parser parser, bool reprocessTokenInNextState)
        {
            bool tokenProcessed = false;
            ClearStackBackToTableContext(parser);
            parser.InsertElement(tag);
            parser.AdvanceState(new InColumnGroupState());
            if (reprocessTokenInNextState)
            {
                tokenProcessed = parser.State.ParseToken(parser);
            }

            return tokenProcessed;
        }

        private void ProcessTableEndTag(TagToken tag, Parser parser, bool reprocessTokenInNextState)
        {
            if (!parser.OpenElementStack.HasElementOfTypeInScope(tag.Name, ScopeType.Table))
            {
                parser.LogParseError("No element with tag name '" + tag.Name + "' is in table scope", "ignoring token");
            }
            else
            {
                ClearStackBackToTableContext(parser);

                // Table element should be at top of stack, so pop it in accordance with spec.
                parser.PopElementFromStack();
                parser.ResetInsertionMode();
            }
        }

        private void ClearStackBackToTableContext(Parser parser)
        {
            // When the steps [...] require the UA to clear the stack back to a table context, 
            // it means that the UA must, while the current node is not a table element or an 
            // html element, pop elements from the stack of open elements.
            while (parser.CurrentNode.Name != HtmlElementFactory.TableElementTagName &&
                parser.CurrentNode.Name != HtmlElementFactory.HtmlElementTagName)
            {
                parser.PopElementFromStack();
            }
        }
    }
}
