using XBrowserProject.Internal.Html.DOM.Parsing.HtmlTokenizer;

namespace XBrowserProject.Internal.Html.DOM.Parsing.States
{
    /// <summary>
    /// Represents the insertion mode of "in head"
    /// </summary>
    /// <remarks>
    /// When the insertion mode is "in head", tokens must be handled as follows:
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
    /// A start tag whose tag name is one of: "base", "command", "link"
    /// <para>
    /// Insert an HTML element for the token. Immediately pop the current node off the stack of open elements.
    /// </para>
    /// <para>
    /// Acknowledge the token's self-closing flag, if it is set.
    /// </para>
    /// </item>
    /// <item>
    /// A start tag whose tag name is "meta"
    /// <para>
    /// Insert an HTML element for the token. Immediately pop the current node off the stack of open elements.
    /// </para>
    /// <para>
    /// Acknowledge the token's self-closing flag, if it is set.
    /// </para>
    /// <para>
    /// If the element has a charset attribute, and its value is a supported encoding, and the confidence is currently tentative, then change the encoding to the encoding given by the value of the charset attribute.
    /// </para>
    /// <para>
    /// Otherwise, if the element has a content attribute, and applying the algorithm for extracting an encoding from a Content-Type to its value returns a supported encoding encoding, and the confidence is currently tentative, then change the encoding to the encoding encoding.
    /// </para>
    /// </item>
    /// <item>
    /// A start tag whose tag name is "title"
    /// <para>
    /// Follow the generic RCDATA element parsing algorithm.
    /// </para>
    /// </item>
    /// <item>
    /// A start tag whose tag name is "noscript", if the scripting flag is enabled
    /// </item>
    /// <item>
    /// A start tag whose tag name is one of: "noframes", "style"
    /// <para>
    /// Follow the generic raw text element parsing algorithm.
    /// </para>
    /// </item>
    /// <item>
    /// A start tag whose tag name is "noscript", if the scripting flag is disabled
    /// <para>
    /// Insert an HTML element for the token.
    ///</para>
    /// Switch the insertion mode to "in head noscript".
    /// </item>
    /// <item>
    /// A start tag whose tag name is "script"
    /// <para>
    /// Run these steps:
    /// <list type="number">
    /// </list>
    /// </para>
    /// <item>
    /// 1.Create an element for the token in the HTML namespace.
    /// </item>
    /// <item>
    /// 2.Mark the element as being "parser-inserted".
    /// <para>
    /// Note: This ensures that, if the script is external, any document.write() calls in the script will execute in-line, instead of blowing the document away, as would happen in most other cases. It also prevents the script from executing until the end tag is seen.
    /// </para>
    /// </item>
    /// <item>
    /// 3.If the parser was originally created for the HTML fragment parsing algorithm, then mark the script element as "already started". (fragment case)
    /// </item>
    /// <item>
    /// 4.Append the new element to the current node and push it onto the stack of open elements.
    /// </item>
    /// <item>
    /// 5.Switch the tokenizer to the script data state.
    /// </item>
    /// <item>
    /// 6.Let the original insertion mode be the current insertion mode.
    /// </item>
    /// <item>
    /// 7.Switch the insertion mode to "text".
    /// </item>
    /// </item>
    /// <item>
    /// An end tag whose tag name is "head"
    /// <para>
    /// Pop the current node (which will be the head element) off the stack of open elements.
    /// </para>
    /// <para>
    /// Switch the insertion mode to "after head".
    /// </para>
    /// </item>
    /// <item>
    /// An end tag whose tag name is one of: "body", "html", "br"
    /// <para>
    /// Act as described in the "anything else" entry below.
    /// </para>
    /// </item>
    /// <item>
    /// A start tag whose tag name is "head"
    /// </item>
    /// <item>
    /// Any other end tag
    /// <para>
    /// Parse error. Ignore the token.
    /// </para>
    /// </item>
    /// <item>
    /// Anything else
    /// <para>
    /// Act as if an end tag token with the tag name "head" had been seen, and reprocess the current token.
    /// </para>
    /// </item>
    /// </list>
    /// </remarks>
    internal class InHeadState : ParserState
    {
        private string description = "in head";
        private bool isFosterParentingRequired = false;
        private bool isInForeignContent = false;

        public InHeadState()
            : this(string.Empty)
        {
        }

        public InHeadState(string actualDescription)
            : this(actualDescription, false)
        {
        }

        public InHeadState(string actualDescription, bool fosterParentChildren)
        {
            isFosterParentingRequired = fosterParentChildren;
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
            if (tag.Name == HtmlElementFactory.BaseElementTagName ||
                tag.Name == HtmlElementFactory.CommandElementTagName ||
                tag.Name == HtmlElementFactory.LinkElementTagName ||
                tag.Name == HtmlElementFactory.MetaElementTagName)
            {
                // A start tag whose tag name is one of: "base", "command", "link"
                // Insert an HTML element for the token. Immediately pop the current 
                // node off the stack of open elements.
                // Acknowledge the token's self-closing flag, if it is set.
                parser.InsertElement(tag, true, isFosterParentingRequired);

                // A start tag whose tag name is "meta"
                // Insert an HTML element for the token. Immediately pop the current node 
                // off the stack of open elements.
                // Acknowledge the token's self-closing flag, if it is set.
                //
                // If the element has a charset attribute, and its value is a supported encoding, 
                // and the confidence is currently tentative, then change the encoding to 
                // the encoding given by the value of the charset attribute.
                //
                // Otherwise, if the element has a content attribute, and applying the algorithm 
                // for extracting an encoding from a Content-Type to its value returns a supported 
                // encoding encoding, and the confidence is currently tentative, then change the 
                // encoding to the encoding encoding.
                if (tag.Name == HtmlElementFactory.MetaElementTagName)
                {
                    // TODO: update character set encoding in accordance with above spec guidance.
                }

                tokenProcessed = true;
            }
            else if (tag.Name == HtmlElementFactory.HtmlElementTagName)
            {
                // A start tag whose tag name is "html"
                // Process the token using the rules for the "in body" insertion mode.
                parser.LogParseError("Cannot have html tag in '" + Description + "' state", "adding attributes to existing element");
                InBodyState temporaryState = new InBodyState(Description);
                temporaryState.ParseToken(parser);
                tokenProcessed = true;
            }
            else if (tag.Name == HtmlElementFactory.TitleElementTagName)
            {
                // A start tag whose tag name is "title"
                // Follow the generic RCDATA element parsing algorithm.
                parser.ActivateRCDataState(tag, isFosterParentingRequired);
                tokenProcessed = true;
            }
            else if (tag.Name == HtmlElementFactory.NoFramesElementTagName ||
                tag.Name == HtmlElementFactory.StyleElementTagName)
            {
                // A start tag whose tag name is one of: "noframes", "style"
                // Follow the generic raw text element parsing algorithm.
                parser.ActivateRawTextState(tag, isFosterParentingRequired);
                tokenProcessed = true;
            }
            else if (tag.Name == HtmlElementFactory.NoScriptElementTagName)
            {
                if (parser.IsScriptingEnabled)
                {
                    // A start tag whose tag name is "noscript", if the scripting flag is enabled
                    // Follow the generic raw text element parsing algorithm.
                    parser.ActivateRawTextState(tag, isFosterParentingRequired);
                }
                else
                {
                    // A start tag whose tag name is "noscript", if the scripting flag is disabled
                    // Insert an HTML element for the token.
                    // Switch the insertion mode to "in head noscript".
                    parser.InsertElement(tag);
                    parser.AdvanceState(new InHeadNoScriptState());
                }

                tokenProcessed = true;
            }
            else if (tag.Name == HtmlElementFactory.HeadElementTagName)
            {
                // A start tag whose tag name is "head"
                // Parse error. Ignore the token.
                parser.LogParseError("Cannot have head start tag in '" + Description + "' state", "ignoring token");
                tokenProcessed = true;
            }
            else if (tag.Name == HtmlElementFactory.ScriptElementTagName)
            {
                // A start tag whose tag name is "script"
                // Run these steps:

                // 1.Create an element for the token in the HTML namespace.

                // 2.Mark the element as being "parser-inserted".

                // Note: This ensures that, if the script is external, any document.write() calls 
                // in the script will execute in-line, instead of blowing the document away, as 
                // would happen in most other cases. It also prevents the script from executing 
                // until the end tag is seen.

                // 3.If the parser was originally created for the HTML fragment parsing algorithm, 
                // then mark the script element as "already started". (fragment case)

                // 4.Append the new element to the current node and push it onto the stack of 
                // open elements.

                // 5.Switch the tokenizer to the script data state.

                // 6.Let the original insertion mode be the current insertion mode.

                // 7.Switch the insertion mode to "text".
                parser.ActivateScriptState(tag);
                tokenProcessed = true;
            }

            return tokenProcessed;
        }

        protected override bool ProcessEndTagToken(TagToken tag, Parser parser)
        {
            bool tokenProcessed = false;
            if (tag.Name == HtmlElementFactory.HeadElementTagName)
            {
                ProcessEndHeadTag(tag, parser, false);
                tokenProcessed = true;
            }
            else if (tag.Name != HtmlElementFactory.BodyElementTagName &&
                tag.Name != HtmlElementFactory.HtmlElementTagName &&
                tag.Name != HtmlElementFactory.BRElementTagName)
            {
                // An end tag whose tag name is one of: "body", "html", "br"
                // Act as described in the "anything else" entry below.
                //
                // Any other end tag
                // Parse error. Ignore the token.
                parser.LogParseError("Cannot have " + tag.Name + " end tag in '" + Description + "' state", "ignoring token");
                tokenProcessed = true;
            }

            return tokenProcessed;
        }

        protected override bool ProcessUnprocessedToken(Parser parser)
        {
            // Anything else
            // Act as if an end tag token with the tag name "head" had been seen, and reprocess 
            // the current token.
            return ProcessEndHeadTag(new TagToken(TokenType.EndTag, HtmlElementFactory.HeadElementTagName), parser, true);
        }

        private bool ProcessEndHeadTag(TagToken tag, Parser parser, bool reprocessTokenInNextState)
        {
            bool tokenProcessed = false;

            // An end tag whose tag name is "head"
            // Pop the current node (which will be the head element) off the stack of open elements.
            // Switch the insertion mode to "after head".
            parser.PopElementFromStack();
            parser.AdvanceState(new AfterHeadState());
            if (reprocessTokenInNextState)
            {
                tokenProcessed = parser.State.ParseToken(parser);
            }

            return tokenProcessed;
        }
    }
}
