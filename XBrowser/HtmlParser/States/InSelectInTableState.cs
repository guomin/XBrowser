using XBrowserProject.BrowserModel.Internal;
using XBrowserProject.HtmlDom;
using XBrowserProject.HtmlParser.HtmlTokenizer;

namespace XBrowserProject.HtmlParser.States
{
    /// <summary>
    /// Represents the insertion mode of "in select in table"
    /// </summary>
    /// <remarks>
    /// When the insertion mode is "in select in table", tokens must be handled as follows:
    /// <list type="bullet">
    /// <item>
    /// A start tag whose tag name is one of: "caption", "table", "tbody", "tfoot", "thead", "tr", "td", "th"
    /// <para>
    /// Parse error. Act as if an end tag with the tag name "select" had been seen, and reprocess the token.
    /// </para>
    /// </item>
    /// <item>
    /// An end tag whose tag name is one of: "caption", "table", "tbody", "tfoot", "thead", "tr", "td", "th"
    /// <para>
    /// Parse error.
    /// </para>
    /// <para>
    /// If the stack of open elements has an element in table scope with the same tag name as that of the token, then act as if an end tag with the tag name "select" had been seen, and reprocess the token. Otherwise, ignore the token.
    /// </para>
    /// </item>
    /// <item>
    /// Anything else
    /// <para>
    /// Process the token using the rules for the "in select" insertion mode.
    /// </para>
    /// </item>
    /// </list>
    /// </remarks>
    public class InSelectInTableState : ParserState
    {
        public override string Description
        {
            get { return "in select in table"; }
        }
        protected override bool ProcessStartTagToken(TagToken tag, Parser parser)
        {
            bool tokenProcessed = false;
            switch (tag.Name)
            {
                case HtmlElementFactory.CaptionElementTagName:
                case HtmlElementFactory.TableElementTagName:
                case HtmlElementFactory.TBodyElementTagName:
                case HtmlElementFactory.TFootElementTagName:
                case HtmlElementFactory.THeadElementTagName:
                case HtmlElementFactory.TRElementTagName:
                case HtmlElementFactory.TDElementTagName:
                case HtmlElementFactory.THElementTagName:
                    // A start tag whose tag name is one of: "caption", "table", "tbody", "tfoot", 
                    // "thead", "tr", "td", "th"
                    // Parse error. Act as if an end tag with the tag name "select" had been seen, 
                    // and reprocess the token.
                    parser.LogParseError("Found start tag for '" + tag.Name + "' in '" + Description + "' state", "adding 'select' end tag and reprocessing");
                    tokenProcessed = ProcessEndSelectToken(tag, parser);
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
                case HtmlElementFactory.TableElementTagName:
                case HtmlElementFactory.TBodyElementTagName:
                case HtmlElementFactory.TFootElementTagName:
                case HtmlElementFactory.THeadElementTagName:
                case HtmlElementFactory.TRElementTagName:
                case HtmlElementFactory.TDElementTagName:
                case HtmlElementFactory.THElementTagName:
                    // An end tag whose tag name is one of: "caption", "table", "tbody", "tfoot", 
                    // "thead", "tr", "td", "th"
                    // Parse error.
                    // If the stack of open elements has an element in table scope with the same 
                    // tag name as that of the token, then act as if an end tag with the tag name 
                    // "select" had been seen, and reprocess the token. Otherwise, ignore the token.
                    string action = "adding 'select' end tag and reprocessing";
                    if (parser.OpenElementStack.HasElementOfTypeInScope(tag.Name, ScopeType.Table))
                    {
                        tokenProcessed = ProcessEndSelectToken(tag, parser);
                    }
                    else
                    {
                        action = "ignoring token";
                        tokenProcessed = true;
                    }
                    parser.LogParseError("Found start tag for '" + tag.Name + "' in '" + Description + "' state", action);
                    break;
            }

            return tokenProcessed;
        }

        protected override bool ProcessUnprocessedToken(Parser parser)
        {
            // Anything else
            // Process the token using the rules for the "in select" insertion mode.
            InSelectState temporaryState = new InSelectState(Description);
            bool tokenProcessed = temporaryState.ParseToken(parser);
            return tokenProcessed;
        }

        private bool ProcessEndSelectToken(TagToken tag, Parser parser)
        {
            // Set current token to be an end tag for select, then process it as if it were
            // an unprocessed token (that is, but the "in select" rules).
            parser.CurrentToken = new TagToken(TokenType.EndTag, HtmlElementFactory.SelectElementTagName);
            ProcessUnprocessedToken(parser);

            // Reset the current token to the original one, then reprocess it.
            parser.CurrentToken = tag;
            return parser.State.ParseToken(parser);
        }
    }
}
