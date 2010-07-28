using XBrowserProject.HtmlParser.HtmlTokenizer;

namespace XBrowserProject.HtmlParser.States
{
    public abstract class ParserState
    {
        public abstract string Description { get; }

        public bool ParseToken(Parser parser)
        {
            bool tokenProcessed = false;
            switch (parser.CurrentToken.TokenType)
            {
                case TokenType.DocType:
                    DocTypeToken doctype = parser.CurrentToken as DocTypeToken;
                    tokenProcessed = ProcessDocTypeToken(doctype, parser);
                    break;

                case TokenType.Comment:
                    CommentToken comment = parser.CurrentToken as CommentToken;
                    tokenProcessed = ProcessCommentToken(comment, parser);
                    break;

                case TokenType.Character:
                    CharacterToken character = parser.CurrentToken as CharacterToken;
                    tokenProcessed = ProcessCharacterToken(character, parser);
                    break;

                case TokenType.StartTag:
                    TagToken startTag = parser.CurrentToken as TagToken;
                    tokenProcessed = ProcessStartTagToken(startTag, parser);
                    break;

                case TokenType.EndTag:
                    TagToken endTag = parser.CurrentToken as TagToken;
                    tokenProcessed = ProcessEndTagToken(endTag, parser);
                    break;

                default:
                    tokenProcessed = ProcessEndOfFileToken(parser);
                    break;
            }

            if (!tokenProcessed)
            {
                tokenProcessed = ProcessUnprocessedToken(parser);
            }

            return tokenProcessed;
        }

        protected abstract bool ProcessStartTagToken(TagToken tag, Parser parser);

        protected abstract bool ProcessEndTagToken(TagToken tag, Parser parser);

        protected abstract bool ProcessUnprocessedToken(Parser parser);

        protected virtual bool ProcessCharacterToken(CharacterToken character, Parser parser)
        {
            bool tokenProcessed = false;

            // A character token that is one of U+0009 CHARACTER TABULATION, 
            // U+000A LINE FEED (LF), U+000C FORM FEED (FF), or U+0020 SPACE
            // Insert the character into the current node.
            if (HtmlCharacterUtilities.IsWhiteSpace(character.Data))
            {
                parser.InsertCharacterIntoNode(character.Data);
                tokenProcessed = true;
            }

            return tokenProcessed;
        }

        protected virtual bool ProcessCommentToken(CommentToken comment, Parser parser)
        {
            // A comment token
            // Append a Comment node to the current node with the data attribute 
            // set to the data given in the comment token.
            parser.CreateComment(comment.Data);
            return true;
        }

        protected virtual bool ProcessDocTypeToken(DocTypeToken docType, Parser parser)
        {
            // A DOCTYPE token
            // Parse error. Ignore the token.
            parser.LogParseError("Cannot have DOCTYPE in '" + Description + "' mode", "ignoring token");
            return true;
        }

        protected virtual bool ProcessEndOfFileToken(Parser parser)
        {
            return false;
        }
    }
}
