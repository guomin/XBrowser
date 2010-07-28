using XBrowserProject.BrowserModel.Internal;
using XBrowserProject.HtmlDom;
using XBrowserProject.HtmlParser.HtmlTokenizer;

namespace XBrowserProject.HtmlParser.States
{
    /// <summary>
    /// Represents the insertion mode of "in table text"
    /// </summary>
    /// <remarks>
    /// When the insertion mode is "in table text", tokens must be handled as follows:
    /// <list type="bullet">
    /// <item>
    /// A character token
    /// <para>
    /// Append the character token to the pending table character tokens list.
    /// </para>
    /// </item>
    /// <item>
    /// Anything else
    /// <para>
    /// If any of the tokens in the pending table character tokens list are character tokens that are not one of U+0009 CHARACTER TABULATION, U+000A LINE FEED (LF), U+000C FORM FEED (FF), or U+0020 SPACE, then reprocess those character tokens using the rules given in the "anything else" entry in the in table" insertion mode.
    /// </para>
    /// <para>
    /// Otherwise, insert the characters given by the pending table character tokens list into the current node.
    /// </para>
    /// <para>
    /// Switch the insertion mode to the original insertion mode and reprocess the token.
    /// </para>
    /// </item>
    /// </list>
    /// </remarks>
    public class InTableTextState : ParserState
    {
        private ParserState returnState;
        private bool isFosterParentingRequired;

        public InTableTextState(ParserState returnState)
            : this(returnState, false)
        {
        }

        public InTableTextState(ParserState returnState, bool fosterParentingIsRequired)
        {
            isFosterParentingRequired = fosterParentingIsRequired;
            this.returnState = returnState;
        }

        public override string Description
        {
            get { return "in table text"; }
        }

        protected override bool ProcessStartTagToken(TagToken tag, Parser parser)
        {
            return false;
        }

        protected override bool ProcessEndTagToken(TagToken tag, Parser parser)
        {
            return false;
        }

        protected override bool ProcessUnprocessedToken(Parser parser)
        {
            Token currentToken = parser.CurrentToken;
            // Anything else
            // If any of the tokens in the pending table character tokens list are character 
            // tokens that are not one of U+0009 CHARACTER TABULATION, U+000A LINE FEED (LF), 
            // U+000C FORM FEED (FF), or U+0020 SPACE, then reprocess those character tokens 
            // using the rules given in the "anything else" entry in the "in table" insertion mode.
            // Otherwise, insert the characters given by the pending table character tokens list into the current node.
            // Switch the insertion mode to the original insertion mode and reprocess the token.
            bool pendingTokenListContainsNonWhitespace = false;
            foreach (CharacterToken token in parser.PendingTableCharacterTokens)
            {
                if (!HtmlCharacterUtilities.IsWhiteSpace(token.Data))
                {
                    pendingTokenListContainsNonWhitespace = true;
                    break;
                }
            }

            if (pendingTokenListContainsNonWhitespace)
            {
                parser.LogParseError("Unhandled token '" + parser.CurrentToken.TokenType.ToString() + "' in '" + Description + "' mode", "processing using 'in body' rules");
                foreach (CharacterToken token in parser.PendingTableCharacterTokens)
                {
                    bool fosterParentingRequired = parser.CurrentNode.Name == HtmlElementFactory.TableElementTagName ||
                        parser.CurrentNode.Name == HtmlElementFactory.TBodyElementTagName ||
                        parser.CurrentNode.Name == HtmlElementFactory.TFootElementTagName ||
                        parser.CurrentNode.Name == HtmlElementFactory.THeadElementTagName ||
                        parser.CurrentNode.Name == HtmlElementFactory.TRElementTagName;

                    parser.CurrentToken = token;
                    InBodyState temporaryState = new InBodyState(Description, fosterParentingRequired, true);
                    temporaryState.ParseToken(parser);
                }
            }
            else
            {
                foreach (CharacterToken token in parser.PendingTableCharacterTokens)
                {
                    parser.InsertCharacterIntoNode(token.Data);
                }
            }

            parser.CurrentToken = currentToken;
            parser.AdvanceState(returnState);
            return parser.State.ParseToken(parser);
        }

        protected override bool ProcessCharacterToken(CharacterToken character, Parser parser)
        {
            /// A character token
            /// Append the character token to the pending table character tokens list.
            parser.PendingTableCharacterTokens.Add(character);
            return true;
        }
    }
}
