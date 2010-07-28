using XBrowserProject.BrowserModel.Internal;
using XBrowserProject.HtmlDom;
using XBrowserProject.HtmlParser.HtmlTokenizer;

namespace XBrowserProject.HtmlParser.States
{
    /// <summary>
    /// Represents the insertion mode of "in select"
    /// </summary>
    /// <remarks>
    /// When the insertion mode is "in select", tokens must be handled as follows:
    /// <list type="bullet">
    /// <item>
    /// A character token
    /// <para>
    /// Insert the token's character into the current node.
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
    /// A start tag whose tag name is "option"
    /// <para>
    /// If the current node is an option element, act as if an end tag with the tag name "option" had been seen.
    /// </para>
    /// <para>
    /// Insert an HTML element for the token.
    /// </para>
    /// </item>
    /// <item>
    /// A start tag whose tag name is "optgroup"
    /// <para>
    /// If the current node is an option element, act as if an end tag with the tag name "option" had been seen.
    /// </para>
    /// <para>
    /// If the current node is an optgroup element, act as if an end tag with the tag name "optgroup" had been seen.
    /// </para>
    /// <para>
    /// Insert an HTML element for the token.
    /// </para>
    /// </item>
    /// <item>
    /// An end tag whose tag name is "optgroup"
    /// <para>
    /// First, if the current node is an option element, and the node immediately before it in the stack of open elements is an optgroup element, then act as if an end tag with the tag name "option" had been seen.
    /// </para>
    /// <para>
    /// If the current node is an optgroup element, then pop that node from the stack of open elements. Otherwise, this is a parse error; ignore the token.
    /// </para>
    /// </item>
    /// <item>
    /// An end tag whose tag name is "option"
    /// <para>
    /// If the current node is an option element, then pop that node from the stack of open elements. Otherwise, this is a parse error; ignore the token.
    /// </para>
    /// </item>
    /// <item>
    /// An end tag whose tag name is "select"
    /// <para>
    /// If the stack of open elements does not have an element in table scope with the same tag name as the token, this is a parse error. Ignore the token. (fragment case)
    /// </para>
    /// <para>
    /// Otherwise:
    /// </para>
    /// <para>
    /// Pop elements from the stack of open elements until a select element has been popped from the stack.
    /// </para>
    /// <para>
    /// Reset the insertion mode appropriately.
    /// </para>
    /// </item>
    /// <item>
    /// A start tag whose tag name is "select"
    /// <para>
    /// Parse error. Act as if the token had been an end tag with the tag name "select" instead.
    /// </para>
    /// </item>
    /// <item>
    /// A start tag whose tag name is one of: "input", "keygen", "textarea"
    /// <para>
    /// Parse error.
    /// </para>
    /// <para>
    /// If the stack of open elements does not have a select element in table scope, ignore the token. (fragment case)
    /// </para>
    /// <para>
    /// Otherwise, act as if an end tag with the tag name "select" had been seen, and reprocess the token.
    /// </para>
    /// </item>
    /// <item>
    /// A start tag token whose tag name is "script"
    /// <para>
    /// Process the token using the rules for the "in head" insertion mode.
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
    /// Parse error. Ignore the token.
    /// </para>
    /// </item>
    /// </list>
    /// </remarks>
    public class InSelectState : ParserState
    {
        private string description = "in select";

        public InSelectState()
            : this(string.Empty)
        {
        }

        public InSelectState(string actualDescription)
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

        protected override bool ProcessCharacterToken(CharacterToken character, Parser parser)
        {
            // A character token
            // Insert the token's character into the current node.
            parser.InsertCharacterIntoNode(character.Data);
            return true;
        }

        protected override bool ProcessEndOfFileToken(Parser parser)
        {
            // An end-of-file token
            // If the current node is not the root html element, then this is a parse error.
            // Note: It can only be the current node in the fragment case.
            // Stop parsing.
            if (parser.CurrentNode.Name != HtmlElementFactory.HtmlElementTagName)
            {
                parser.LogParseError("End of file token seen with current element not 'html'", "none, stopping parsing");
            }

            return true;
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

                case HtmlElementFactory.OptionElementTagName:
                    // A start tag whose tag name is "option"
                    // If the current node is an option element, act as if an end tag with the 
                    // tag name "option" had been seen.
                    // Insert an HTML element for the token.
                    if (parser.CurrentNode.Name == HtmlElementFactory.OptionElementTagName)
                    {
                        ProcessEndTagToken(new TagToken(TokenType.EndTag, HtmlElementFactory.OptionElementTagName), parser);
                    }

                    parser.InsertElement(tag);
                    break;

                case HtmlElementFactory.OptGroupElementTagName:
                    // A start tag whose tag name is "optgroup"
                    // If the current node is an option element, act as if an end tag with the tag 
                    // name "option" had been seen.
                    // If the current node is an optgroup element, act as if an end tag with the tag 
                    // name "optgroup" had been seen.
                    // Insert an HTML element for the token.
                    if (parser.CurrentNode.Name == HtmlElementFactory.OptionElementTagName)
                    {
                        ProcessEndTagToken(new TagToken(TokenType.EndTag, HtmlElementFactory.OptionElementTagName), parser);
                    }

                    if (parser.CurrentNode.Name == HtmlElementFactory.OptGroupElementTagName)
                    {
                        ProcessEndTagToken(new TagToken(TokenType.EndTag, HtmlElementFactory.OptGroupElementTagName), parser);
                    }

                    parser.InsertElement(tag);
                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.SelectElementTagName:
                    // A start tag whose tag name is "select"
                    // Parse error. Act as if the token had been an end tag with the tag name "select" instead.
                    parser.LogParseError("Open tag for '" + tag.Name + "' found in '" + Description + "' state", "treating as close tag");
                    ProcessEndTagToken(new TagToken(TokenType.EndTag, HtmlElementFactory.SelectElementTagName), parser);
                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.InputElementTagName:
                case HtmlElementFactory.KeyGenElementTagName:
                case HtmlElementFactory.TextAreaElementTagName:
                    // A start tag whose tag name is one of: "input", "keygen", "textarea"
                    // Parse error.
                    // If the stack of open elements does not have a select element in table scope, 
                    // ignore the token. (fragment case)
                    // Otherwise, act as if an end tag with the tag name "select" had been seen, 
                    // and reprocess the token.
                    string action = "processing as if end tag";
                    if (!parser.OpenElementStack.HasElementOfTypeInScope(HtmlElementFactory.SelectElementTagName, ScopeType.Table))
                    {
                        action = "ignoring token";
                        tokenProcessed = true;
                    }
                    else
                    {
                        tokenProcessed = ProcessEndSelectTag(new TagToken(TokenType.EndTag, HtmlElementFactory.SelectElementTagName), parser, true);
                    }

                    parser.LogParseError("Open tag for '" + tag.Name + "' found in '" + Description + "' state", action);
                    break;

                case HtmlElementFactory.ScriptElementTagName:
                    // A start tag token whose tag name is "script"
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
            switch (tag.Name)
            {
                case HtmlElementFactory.OptGroupElementTagName:
                    // An end tag whose tag name is "optgroup"
                    // First, if the current node is an option element, and the node immediately 
                    // before it in the stack of open elements is an optgroup element, then act 
                    // as if an end tag with the tag name "option" had been seen.
                    // If the current node is an optgroup element, then pop that node from the 
                    // stack of open elements. Otherwise, this is a parse error; ignore the token.
                    if (parser.CurrentNode.Name == HtmlElementFactory.OptionElementTagName &&
                        parser.OpenElementStack[1].Name == HtmlElementFactory.OptGroupElementTagName)
                    {
                        ProcessEndTagToken(new TagToken(TokenType.EndTag, HtmlElementFactory.OptionElementTagName), parser);
                    }

                    if (parser.CurrentNode.Name == HtmlElementFactory.OptGroupElementTagName)
                    {
                        parser.PopElementFromStack();
                    }
                    else
                    {
                        parser.LogParseError("Current node is not 'optgroup'", "ignoring token");
                    }

                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.OptionElementTagName:
                    // An end tag whose tag name is "option"
                    // If the current node is an option element, then pop that node from the 
                    // stack of open elements. Otherwise, this is a parse error; ignore the token.
                    if (parser.CurrentNode.Name == HtmlElementFactory.OptionElementTagName)
                    {
                        parser.PopElementFromStack();
                    }
                    else
                    {
                        parser.LogParseError("Current node is not 'optgroup'", "ignoring token");
                    }

                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.SelectElementTagName:
                    // An end tag whose tag name is "select"
                    // If the stack of open elements does not have an element in table scope 
                    // with the same tag name as the token, this is a parse error. Ignore the token. (fragment case)
                    // Otherwise:
                    // Pop elements from the stack of open elements until a select element has been 
                    // popped from the stack.
                    // Reset the insertion mode appropriately.
                    ProcessEndSelectTag(tag, parser, false);
                    tokenProcessed = true;
                    break;
            }

            return tokenProcessed;

        }

        protected override bool ProcessUnprocessedToken(Parser parser)
        {
            // Anything else
            // Parse error. Ignore the token.
            parser.LogParseError("Invalid token in '" + Description + "' state", "ignoring token");
            return true;
        }

        private bool ProcessEndSelectTag(TagToken tag, Parser parser, bool reprocessTokenInNextState)
        {
            bool tokenProcessed = false;
            if (!parser.OpenElementStack.HasElementOfTypeInScope(tag.Name, ScopeType.Table))
            {
                parser.LogParseError("Did not have '" + tag.Name + "' element in table scope", "ignoring token");
            }
            else
            {
                while (parser.CurrentNode.Name != tag.Name)
                {
                    parser.PopElementFromStack();
                }

                parser.PopElementFromStack();
                parser.ResetInsertionMode();
            }

            if (reprocessTokenInNextState)
            {
                tokenProcessed = parser.State.ParseToken(parser);
            }

            return tokenProcessed;
        }
    }
}
