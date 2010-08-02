using XBrowserProject.BrowserModel.Internal;
using XBrowserProject.BrowserModel.Internal.HtmlDom;
using XBrowserProject.HtmlDom;
using XBrowserProject.HtmlParser.HtmlTokenizer;

namespace XBrowserProject.HtmlParser.States
{
    internal class TextState : ParserState
    {
        private ParserState returnState;

        public TextState(ParserState returnState)
        {
            this.returnState = returnState;
        }

        public override string Description
        {
            get { return "text"; }
        }

        protected override bool ProcessCharacterToken(CharacterToken character, Parser parser)
        {
            // A character token
            // Insert the token's character into the current node
            parser.InsertCharacterIntoNode(character.Data);
            return true;
        }

        protected override bool ProcessStartTagToken(TagToken tag, Parser parser)
        {
            return false;
        }

        protected override bool ProcessEndTagToken(TagToken tag, Parser parser)
        {
            if (tag.Name == HtmlElementFactory.ScriptElementTagName)
            {
                HtmlScriptElement script = parser.CurrentNode as HtmlScriptElement;

                parser.PopElementFromStack();
                parser.AdvanceState(returnState);
                RunScript(parser, script);
            }
            else
            {
                // Any other end tag
                // Pop the current node off the stack of open elements.
                // Switch the insertion mode to the original insertion mode.
                parser.PopElementFromStack();
                parser.AdvanceState(returnState);
            }

            return true;
        }

        protected override bool ProcessEndOfFileToken(Parser parser)
        {
            parser.LogParseError("Unexpected EOF in text state", "none");
            if (parser.CurrentNode.Name == HtmlElementFactory.ScriptElementTagName)
            {
                HtmlScriptElement element = parser.CurrentNode as HtmlScriptElement;
                element.AlreadyStarted = true;
            }

            parser.PopElementFromStack();
            parser.AdvanceState(returnState);
            return parser.State.ParseToken(parser);
        }

        private void RunScript(Parser parser, HtmlScriptElement script)
        {
            if (parser.Document.Window != null)
            {
                //parser.Document.Window.RunScript(script.InnerText);
            }
        }

        protected override bool ProcessUnprocessedToken(Parser parser)
        {
            return true;
        }
    }
}
