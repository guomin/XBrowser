using System;
using System.Collections.Generic;
using System.Xml;
using XBrowserProject.Internal.Html.DOM.Parsing.HtmlTokenizer;

namespace XBrowserProject.Internal.Html.DOM.Parsing.States
{
    internal enum QuirksMode
    {
        None,
        LimitedQuirks,
        Quirks
    }

    /// <summary>
    /// Represents the insertion mode of "initial"
    /// </summary>
    /// <remarks>
    /// When the insertion mode is "initial", tokens must be handled as follows:
    /// <list type="bullet">
    /// <item>
    /// A character token that is one of U+0009 CHARACTER TABULATION, U+000A LINE FEED (LF), U+000C FORM FEED (FF), or U+0020 SPACE
    /// <para>
    /// Ignore the token.
    /// </para>
    /// </item>
    /// <item>
    /// A comment token
    /// <para>
    /// Append a Comment node to the Document object with the data attribute set to the data given in the comment token.
    /// </para>
    /// </item>
    /// <item>
    /// A DOCTYPE token
    /// <para>
    /// If the DOCTYPE token's name is not a case-sensitive match for the string "html", or the token's public identifier is not missing, or the token's system identifier is neither missing nor a case-sensitive match for the string "about:legacy-compat", and none of the sets of conditions in the following list are matched, then there is a parse error.
    /// <list type="bullet">
    /// <item>The DOCTYPE token's name is a case-sensitive match for the string "html", the token's public identifier is the case-sensitive string "-///W3C///DTD HTML 4.0///EN", and the token's system identifier is either missing or the case-sensitive string "http://www.w3.org/TR/REC-html40/strict.dtd".</item>
    /// <item>The DOCTYPE token's name is a case-sensitive match for the string "html", the token's public identifier is the case-sensitive string "-///W3C///DTD HTML 4.01///EN", and the token's system identifier is either missing or the case-sensitive string "http://www.w3.org/TR/html4/strict.dtd".</item>
    /// <item>The DOCTYPE token's name is a case-sensitive match for the string "html", the token's public identifier is the case-sensitive string "-///W3C///DTD XHTML 1.0 Strict///EN", and the token's system identifier is the case-sensitive string "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd".</item>
    /// <item>The DOCTYPE token's name is a case-sensitive match for the string "html", the token's public identifier is the case-sensitive string "-///W3C///DTD XHTML 1.1///EN", and the token's system identifier is the case-sensitive string "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd".</item>
    /// </list>
    /// </para>
    /// <para>
    /// Append a DocumentType node to the Document node, with the name attribute set to the name given in the DOCTYPE token, or the empty string if the name was missing; the publicId attribute set to the public identifier given in the DOCTYPE token, or the empty string if the public identifier was missing; the systemId attribute set to the system identifier given in the DOCTYPE token, or the empty string if the system identifier was missing; and the other attributes specific to DocumentType objects set to null and empty lists as appropriate. Associate the DocumentType node with the Document object so that it is returned as the value of the doctype attribute of the Document object.
    /// </para>
    /// <para>
    /// Then, if the DOCTYPE token matches one of the conditions in the following list, then set the Document to quirks mode:
    /// <list type="bullet">
    /// <item>The force-quirks flag is set to on.</item>
    /// <item>The name is set to anything other than "html" (compared case-sensitively).</item>
    /// <item>The public identifier starts with: "+//Silmaril//dtd html Pro v0r11 19970101//"</item>
    /// <item>The public identifier starts with: "-//AdvaSoft Ltd//DTD HTML 3.0 asWedit + extensions//"</item>
    /// <item>The public identifier starts with: "-//AS//DTD HTML 3.0 asWedit + extensions///"</item>
    /// <item>The public identifier starts with: "-//IETF//DTD HTML 2.0 Level 1///"</item>
    /// <item>The public identifier starts with: "-//IETF//DTD HTML 2.0 Level 2///"</item>
    /// <item>The public identifier starts with: "-//IETF//DTD HTML 2.0 Strict Level 1///"</item>
    /// <item>The public identifier starts with: "-//IETF//DTD HTML 2.0 Strict Level 2///"</item>
    /// <item>The public identifier starts with: "-//IETF//DTD HTML 2.0 Strict///"</item>
    /// <item>The public identifier starts with: "-//IETF//DTD HTML 2.0///"</item>
    /// <item>The public identifier starts with: "-//IETF//DTD HTML 2.1E///"</item>
    /// <item>The public identifier starts with: "-//IETF//DTD HTML 3.0///"</item>
    /// <item>The public identifier starts with: "-//IETF//DTD HTML 3.2 Final///"</item>
    /// <item>The public identifier starts with: "-//IETF//DTD HTML 3.2///"</item>
    /// <item>The public identifier starts with: "-//IETF//DTD HTML 3///"</item>
    /// <item>The public identifier starts with: "-//IETF//DTD HTML Level 0///"</item>
    /// <item>The public identifier starts with: "-//IETF//DTD HTML Level 1///"</item>
    /// <item>The public identifier starts with: "-//IETF//DTD HTML Level 2///"</item>
    /// <item>The public identifier starts with: "-//IETF//DTD HTML Level 3///"</item>
    /// <item>The public identifier starts with: "-//IETF//DTD HTML Strict Level 0///"</item>
    /// <item>The public identifier starts with: "-//IETF//DTD HTML Strict Level 1///"</item>
    /// <item>The public identifier starts with: "-//IETF//DTD HTML Strict Level 2///"</item>
    /// <item>The public identifier starts with: "-//IETF//DTD HTML Strict Level 3///"</item>
    /// <item>The public identifier starts with: "-//IETF//DTD HTML Strict///"</item>
    /// <item>The public identifier starts with: "-//IETF//DTD HTML///"</item>
    /// <item>The public identifier starts with: "-//Metrius//DTD Metrius Presentational///"</item>
    /// <item>The public identifier starts with: "-//Microsoft//DTD Internet Explorer 2.0 HTML Strict///"</item>
    /// <item>The public identifier starts with: "-//Microsoft//DTD Internet Explorer 2.0 HTML///"</item>
    /// <item>The public identifier starts with: "-//Microsoft//DTD Internet Explorer 2.0 Tables///"</item>
    /// <item>The public identifier starts with: "-//Microsoft//DTD Internet Explorer 3.0 HTML Strict///"</item>
    /// <item>The public identifier starts with: "-//Microsoft//DTD Internet Explorer 3.0 HTML///"</item>
    /// <item>The public identifier starts with: "-//Microsoft//DTD Internet Explorer 3.0 Tables///"</item>
    /// <item>The public identifier starts with: "-//Netscape Comm. Corp.//DTD HTML///"</item>
    /// <item>The public identifier starts with: "-//Netscape Comm. Corp.//DTD Strict HTML///"</item>
    /// <item>The public identifier starts with: "-//O'Reilly and Associates//DTD HTML 2.0///"</item>
    /// <item>The public identifier starts with: "-//O'Reilly and Associates//DTD HTML Extended 1.0///"</item>
    /// <item>The public identifier starts with: "-//O'Reilly and Associates//DTD HTML Extended Relaxed 1.0///"</item>
    /// <item>The public identifier starts with: "-//SoftQuad Software//DTD HoTMetaL PRO 6.0::19990601::extensions to HTML 4.0///"</item>
    /// <item>The public identifier starts with: "-//SoftQuad//DTD HoTMetaL PRO 4.0::19971010::extensions to HTML 4.0///"</item>
    /// <item>The public identifier starts with: "-//Spyglass//DTD HTML 2.0 Extended///"</item>
    /// <item>The public identifier starts with: "-//SQ//DTD HTML 2.0 HoTMetaL + extensions///"</item>
    /// <item>The public identifier starts with: "-//Sun Microsystems Corp.//DTD HotJava HTML///"</item>
    /// <item>The public identifier starts with: "-//Sun Microsystems Corp.//DTD HotJava Strict HTML///"</item>
    /// <item>The public identifier starts with: "-//W3C//DTD HTML 3 1995-03-24///"</item>
    /// <item>The public identifier starts with: "-//W3C//DTD HTML 3.2 Draft///"</item>
    /// <item>The public identifier starts with: "-//W3C//DTD HTML 3.2 Final///"</item>
    /// <item>The public identifier starts with: "-//W3C//DTD HTML 3.2///"</item>
    /// <item>The public identifier starts with: "-//W3C//DTD HTML 3.2S Draft///"</item>
    /// <item>The public identifier starts with: "-//W3C//DTD HTML 4.0 Frameset///"</item>
    /// <item>The public identifier starts with: "-//W3C//DTD HTML 4.0 Transitional///"</item>
    /// <item>The public identifier starts with: "-//W3C//DTD HTML Experimental 19960712///"</item>
    /// <item>The public identifier starts with: "-//W3C//DTD HTML Experimental 970421///"</item>
    /// <item>The public identifier starts with: "-//W3C//DTD W3 HTML///"</item>
    /// <item>The public identifier starts with: "-//W3O//DTD W3 HTML 3.0///"</item>
    /// <item>The public identifier is set to: "-//W3O//DTD W3 HTML Strict 3.0///EN///"</item>
    /// <item>The public identifier starts with: "-//WebTechs//DTD Mozilla HTML 2.0///"</item>
    /// <item>The public identifier starts with: "-//WebTechs//DTD Mozilla HTML///"</item>
    /// <item>The public identifier is set to: "-/W3C/DTD HTML 4.0 Transitional/EN"</item>
    /// <item>The public identifier is set to: "HTML"</item>
    /// <item>The system identifier is set to: "http:///www.ibm.com/data/dtd/v11/ibmxhtml1-transitional.dtd"</item>
    /// <item>The system identifier is missing and the public identifier starts with: "-//W3C//DTD HTML 4.01 Frameset///"</item>
    /// <item>The system identifier is missing and the public identifier starts with: "-//W3C//DTD HTML 4.01 Transitional///"</item>
    /// </list>
    /// </para>
    /// <para>
    /// Otherwise, if the DOCTYPE token matches one of the conditions in the following list, then set the Document to limited quirks mode:
    /// <list type="bullet">
    /// <item>The public identifier starts with: "-//W3C//DTD XHTML 1.0 Frameset///"</item>
    /// <item>The public identifier starts with: "-//W3C//DTD XHTML 1.0 Transitional///"</item>
    /// <item>The system identifier is not missing and the public identifier starts with: "-//W3C//DTD HTML 4.01 Frameset///"</item>
    /// <item>The system identifier is not missing and the public identifier starts with: "-//W3C//DTD HTML 4.01 Transitional///"</item>
    /// </list>
    /// </para>
    /// <para>
    /// The system identifier and public identifier strings must be compared to the values given in the lists above in an ASCII case-insensitive manner. A system identifier whose value is the empty string is not considered missing for the purposes of the conditions above.
    /// </para>
    /// <para>
    /// Then, switch the insertion mode to "before html".
    /// </para>
    /// <para>
    /// Then, switch the insertion mode to "before html".
    /// </para>
    /// </item>
    /// <item>
    /// Anything else
    /// <para>
    /// If the document is not an iframe srcdoc document, then this is a parse error; set the Document to quirks mode.
    /// </para>
    /// <para>
    /// In any case, switch the insertion mode to "before html", then reprocess the current token.
    /// </para>
    /// </item>
    /// </list>
    /// </remarks>
    internal class InitialState : ParserState
    {
        private static List<string> quirksPublicIdentifiers;

        public override string Description
        {
            get { return "initial"; }
        }

        private static List<string> QuirksPublicIdentifiers
        {
            get 
            {
                if (quirksPublicIdentifiers == null)
                {
                    quirksPublicIdentifiers = new List<string>();
                    quirksPublicIdentifiers.Add("+//Silmaril//dtd html Pro v0r11 19970101//*");
                    quirksPublicIdentifiers.Add("-//AdvaSoft Ltd//DTD HTML 3.0 asWedit + extensions//*");
                    quirksPublicIdentifiers.Add("-//AS//DTD HTML 3.0 asWedit + extensions///*");
                    quirksPublicIdentifiers.Add("-//IETF//DTD HTML 2.0 Level 1///*");
                    quirksPublicIdentifiers.Add("-//IETF//DTD HTML 2.0 Level 2///*");
                    quirksPublicIdentifiers.Add("-//IETF//DTD HTML 2.0 Strict Level 1///*");
                    quirksPublicIdentifiers.Add("-//IETF//DTD HTML 2.0 Strict Level 2///*");
                    quirksPublicIdentifiers.Add("-//IETF//DTD HTML 2.0 Strict///*");
                    quirksPublicIdentifiers.Add("-//IETF//DTD HTML 2.0///*");
                    quirksPublicIdentifiers.Add("-//IETF//DTD HTML 2.1E///*");
                    quirksPublicIdentifiers.Add("-//IETF//DTD HTML 3.0///*");
                    quirksPublicIdentifiers.Add("-//IETF//DTD HTML 3.2 Final///*");
                    quirksPublicIdentifiers.Add("-//IETF//DTD HTML 3.2///*");
                    quirksPublicIdentifiers.Add("-//IETF//DTD HTML 3///*");
                    quirksPublicIdentifiers.Add("-//IETF//DTD HTML Level 0///*");
                    quirksPublicIdentifiers.Add("-//IETF//DTD HTML Level 1///*");
                    quirksPublicIdentifiers.Add("-//IETF//DTD HTML Level 2///*");
                    quirksPublicIdentifiers.Add("-//IETF//DTD HTML Level 3///*");
                    quirksPublicIdentifiers.Add("-//IETF//DTD HTML Strict Level 0///*");
                    quirksPublicIdentifiers.Add("-//IETF//DTD HTML Strict Level 1///*");
                    quirksPublicIdentifiers.Add("-//IETF//DTD HTML Strict Level 2///*");
                    quirksPublicIdentifiers.Add("-//IETF//DTD HTML Strict Level 3///*");
                    quirksPublicIdentifiers.Add("-//IETF//DTD HTML Strict///*");
                    quirksPublicIdentifiers.Add("-//IETF//DTD HTML///*");
                    quirksPublicIdentifiers.Add("-//Metrius//DTD Metrius Presentational///*");
                    quirksPublicIdentifiers.Add("-//Microsoft//DTD Internet Explorer 2.0 HTML Strict///*");
                    quirksPublicIdentifiers.Add("-//Microsoft//DTD Internet Explorer 2.0 HTML///*");
                    quirksPublicIdentifiers.Add("-//Microsoft//DTD Internet Explorer 2.0 Tables///*");
                    quirksPublicIdentifiers.Add("-//Microsoft//DTD Internet Explorer 3.0 HTML Strict///*");
                    quirksPublicIdentifiers.Add("-//Microsoft//DTD Internet Explorer 3.0 HTML///*");
                    quirksPublicIdentifiers.Add("-//Microsoft//DTD Internet Explorer 3.0 Tables///*");
                    quirksPublicIdentifiers.Add("-//Netscape Comm. Corp.//DTD HTML///*");
                    quirksPublicIdentifiers.Add("-//Netscape Comm. Corp.//DTD Strict HTML///*");
                    quirksPublicIdentifiers.Add("-//O'Reilly and Associates//DTD HTML 2.0///*");
                    quirksPublicIdentifiers.Add("-//O'Reilly and Associates//DTD HTML Extended 1.0///*");
                    quirksPublicIdentifiers.Add("-//O'Reilly and Associates//DTD HTML Extended Relaxed 1.0///*");
                    quirksPublicIdentifiers.Add("-//SoftQuad Software//DTD HoTMetaL PRO 6.0::19990601::extensions to HTML 4.0///*");
                    quirksPublicIdentifiers.Add("-//SoftQuad//DTD HoTMetaL PRO 4.0::19971010::extensions to HTML 4.0///*");
                    quirksPublicIdentifiers.Add("-//Spyglass//DTD HTML 2.0 Extended///*");
                    quirksPublicIdentifiers.Add("-//SQ//DTD HTML 2.0 HoTMetaL + extensions///*");
                    quirksPublicIdentifiers.Add("-//Sun Microsystems Corp.//DTD HotJava HTML///*");
                    quirksPublicIdentifiers.Add("-//Sun Microsystems Corp.//DTD HotJava Strict HTML///*");
                    quirksPublicIdentifiers.Add("-//W3C//DTD HTML 3 1995-03-24///*");
                    quirksPublicIdentifiers.Add("-//W3C//DTD HTML 3.2 Draft///*");
                    quirksPublicIdentifiers.Add("-//W3C//DTD HTML 3.2 Final///*");
                    quirksPublicIdentifiers.Add("-//W3C//DTD HTML 3.2///*");
                    quirksPublicIdentifiers.Add("-//W3C//DTD HTML 3.2S Draft///*");
                    quirksPublicIdentifiers.Add("-//W3C//DTD HTML 4.0 Frameset///*");
                    quirksPublicIdentifiers.Add("-//W3C//DTD HTML 4.0 Transitional///*");
                    quirksPublicIdentifiers.Add("-//W3C//DTD HTML Experimental 19960712///*");
                    quirksPublicIdentifiers.Add("-//W3C//DTD HTML Experimental 970421///*");
                    quirksPublicIdentifiers.Add("-//W3C//DTD W3 HTML///*");
                    quirksPublicIdentifiers.Add("-//W3O//DTD W3 HTML 3.0///*");
                    quirksPublicIdentifiers.Add("-//W3O//DTD W3 HTML Strict 3.0///EN///");
                    quirksPublicIdentifiers.Add("-//WebTechs//DTD Mozilla HTML 2.0///*");
                    quirksPublicIdentifiers.Add("-//WebTechs//DTD Mozilla HTML///*");
                    quirksPublicIdentifiers.Add("-/W3C/DTD HTML 4.0 Transitional/EN");
                    quirksPublicIdentifiers.Add("HTML");

                }
                return InitialState.quirksPublicIdentifiers; 
            }
        }

        protected override bool ProcessCharacterToken(CharacterToken character, Parser parser)
        {
            bool tokenProcessed = false;
            if (HtmlCharacterUtilities.IsWhiteSpace(character.Data))
            {
                tokenProcessed = true;
            }

            return tokenProcessed;
        }

        protected override bool ProcessCommentToken(CommentToken comment, Parser parser)
        {
            HtmlCommentNode commentNode = parser.Document.createComment(comment.Data) as HtmlCommentNode;
            parser.Document.AppendChild(commentNode);
            return true;
        }

        protected override bool ProcessStartTagToken(TagToken tag, Parser parser)
        {
            return false;
        }

        protected override bool ProcessEndTagToken(TagToken tag, Parser parser)
        {
            return false;
        }

        protected override bool ProcessDocTypeToken(DocTypeToken docType, Parser parser)
        {
            string docTypeName = Parser.EmptyDocTypeName;
            if (!string.IsNullOrEmpty(docType.Name))
            {
                docTypeName = XmlConvert.EncodeName(docType.Name); ;
            }

            if (!CheckForParseError(docType))
            {
                parser.LogParseError("Invalid DOCTYPE", "none");
            }

            // XmlDocumentType docTypeNode = parser.Document.CreateDocumentType(docTypeName, docType.PublicId, docType.SystemId, null);
            XmlDocumentType docTypeNode = parser.Document.CreateDocumentType(docTypeName, docType.PublicId, null, null);
            parser.Document.AppendChild(docTypeNode);
            parser.DocumentQuirksMode = SetQuirksMode(docType);
            parser.AdvanceState(new BeforeHtmlState());
            return true;
        }

        protected override bool ProcessUnprocessedToken(Parser parser)
        {
            bool tokenProcessed = false;
            if (!parser.IsIFrameSourceDoc)
            {
                parser.LogParseError("Document is not an iframe source doc", "none. advancing to 'before html' state");
            }

            parser.DocumentQuirksMode = QuirksMode.Quirks;
            parser.AdvanceState(new BeforeHtmlState());
            return tokenProcessed;
        }

        private QuirksMode SetQuirksMode(DocTypeToken docType)
        {
            QuirksMode doctypeQuirksMode = QuirksMode.None;
            if (docType.QuirksMode ||
                docType.Name != "html" ||
                PublicIdentifierImpliesQuirksMode(docType.PublicId) ||
                (docType.SystemId != null && string.Compare(docType.SystemId, "http:///www.ibm.com/data/dtd/v11/ibmxhtml1-transitional.dtd", StringComparison.OrdinalIgnoreCase) == 0) ||
                docType.SystemId == null && docType.PublicId != null && docType.PublicId.StartsWith("-//W3C//DTD HTML 4.01 Frameset///", StringComparison.OrdinalIgnoreCase) ||
                docType.SystemId == null && docType.PublicId != null && docType.PublicId.StartsWith("-//W3C//DTD HTML 4.01 Transitional///", StringComparison.OrdinalIgnoreCase))
            {
                doctypeQuirksMode = QuirksMode.Quirks;
            }
            else
            {
                if (docType.PublicId != null && docType.PublicId.StartsWith("-//W3C//DTD XHTML 1.0 Frameset///", StringComparison.OrdinalIgnoreCase) ||
                    docType.PublicId != null && docType.PublicId.StartsWith("-//W3C//DTD XHTML 1.0 Transitional///", StringComparison.OrdinalIgnoreCase) ||
                    (docType.SystemId != null && docType.PublicId != null && docType.PublicId.StartsWith("-//W3C//DTD HTML 4.01 Frameset///", StringComparison.OrdinalIgnoreCase)) ||
                    (docType.SystemId != null && docType.PublicId != null && docType.PublicId.StartsWith("-//W3C//DTD HTML 4.01 Transitional///", StringComparison.OrdinalIgnoreCase)))
                {
                    doctypeQuirksMode = QuirksMode.LimitedQuirks;
                }
            }

            return doctypeQuirksMode;
        }

        private bool PublicIdentifierImpliesQuirksMode(string publicIdentifier)
        {
            bool idImpliesQuirksMode = false;
            if (!string.IsNullOrEmpty(publicIdentifier))
            {
                foreach (string quirksModePublicId in QuirksPublicIdentifiers)
                {
                    if (quirksModePublicId.EndsWith("*"))
                    {
                        idImpliesQuirksMode = publicIdentifier.StartsWith(quirksModePublicId.Substring(0, quirksModePublicId.Length - 1));
                    }
                    else
                    {
                        idImpliesQuirksMode = publicIdentifier == quirksModePublicId;
                    }
                }
            }
            return idImpliesQuirksMode;
        }

        private bool CheckForParseError(DocTypeToken token)
        {
            bool tokenIsValid = false;

            if (token.Name != "html" || token.PublicId != null || (token.SystemId != null && token.SystemId != "about:legacy-compat"))
            {
                if (token.Name == "html" && token.PublicId == "-//W3C//DTD HTML 4.0//EN" && (token.SystemId == null || token.SystemId == "http://www.w3.org/TR/REC-html40/strict.dtd"))
                {
                    tokenIsValid = true;
                }
                else if (token.Name == "html" && token.PublicId == "-//W3C//DTD HTML 4.01//EN" && (token.SystemId == null || token.SystemId == "http://www.w3.org/TR/REC-html40/strict.dtd"))
                {
                    tokenIsValid = true;
                }
                else if (token.Name == "html" && token.PublicId == "-//W3C//DTD XHTML 1.0 Strict//EN" && token.SystemId == "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd")
                {
                    tokenIsValid = true;
                }
                else if (token.Name == "html" && token.PublicId == "-//W3C//DTD XHTML 1.1//EN" && token.SystemId == "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd")
                {
                    tokenIsValid = true;
                }
            }
            else
            {
                tokenIsValid = true;
            }

            return tokenIsValid;
        }
    }
}
