using System.Collections.Generic;
using XBrowserProject.BrowserModel.Internal;
using XBrowserProject.BrowserModel.Internal.HtmlDom;
using XBrowserProject.HtmlDom;
using XBrowserProject.HtmlParser.HtmlTokenizer;

namespace XBrowserProject.HtmlParser.States
{
    /// <summary>
    /// Represents the insertion mode of "in body"
    /// </summary>
    public class InBodyState : ParserState
    {
        private string description = "in body";
        private bool isInTable = false;
        private bool isFosterParentingRequired = false;

        public InBodyState()
        {
        }

        public InBodyState(string actualDescription)
            : this(actualDescription, false, false)
        {
        }

        public InBodyState(string actualDescription, bool fosterParentingRequired, bool isTableState)
        {
            description = actualDescription;
            isInTable = isTableState;
            isFosterParentingRequired = fosterParentingRequired;
        }

        public override string Description
        {
            get { return description; }
        }

        protected override bool ProcessCharacterToken(CharacterToken character, Parser parser)
        {
            bool reconstructedFormattingElements = parser.ReconstructActiveFormattingElements(isFosterParentingRequired);
            parser.InsertCharacterIntoNode(character.Data, isFosterParentingRequired && !reconstructedFormattingElements);
            if (HtmlCharacterUtilities.IsWhiteSpace(character.Data))
            {
                parser.IsFramesetOK = false;
            }

            return true;
        }

        protected override bool ProcessStartTagToken(TagToken tag, Parser parser)
        {
            bool tokenProcessed = false;

            switch (tag.Name)
            {
                case HtmlElementFactory.HtmlElementTagName:
                    // Parse error. For each attribute on the token, check to see if the attribute 
                    // is already present on the top element of the stack of open elements. If it 
                    // is not, add the attribute and its corresponding value to that element.
                    ProcessHtmlStartTag(tag, parser);
                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.BaseElementTagName:
                case HtmlElementFactory.CommandElementTagName:
                case HtmlElementFactory.LinkElementTagName:
                case HtmlElementFactory.MetaElementTagName:
                case HtmlElementFactory.NoFramesElementTagName:
                case HtmlElementFactory.ScriptElementTagName:
                case HtmlElementFactory.StyleElementTagName:
                case HtmlElementFactory.TitleElementTagName:
                    // Process the token using the rules for the "in head" insertion mode.
                    InHeadState temporaryState = new InHeadState(description, isFosterParentingRequired);
                    tokenProcessed = temporaryState.ParseToken(parser);
                    break;

                case HtmlElementFactory.BodyElementTagName:
                    // Parse error.
                    // If the second element on the stack of open elements is not a body element,
                    // or, if the stack of open elements has only one node on it, then ignore the
                    // token. (fragment case)
                    // Otherwise, for each attribute on the token, check to see if the attribute 
                    // is already present on the body element (the second element) on the stack 
                    // of open elements. If it is not, add the attribute and its corresponding 
                    // value to that element.
                    ProcessBodyStartTag(tag, parser);
                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.FramesetElementTagName:
                    ProcessFramesetStartTag(tag, parser);
                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.AddressElementTagName:
                case HtmlElementFactory.ArticleElementTagName:
                case HtmlElementFactory.AsideElementTagName:
                case HtmlElementFactory.BlockQuoteElementTagName:
                case HtmlElementFactory.CenterElementTagName:
                case HtmlElementFactory.DetailsElementTagName:
                case HtmlElementFactory.DirElementTagName:
                case HtmlElementFactory.DivElementTagName:
                case HtmlElementFactory.DLElementTagName:
                case HtmlElementFactory.FieldsetElementTagName:
                case HtmlElementFactory.FigureElementTagName:
                case HtmlElementFactory.FooterElementTagName:
                case HtmlElementFactory.HeaderElementTagName:
                case HtmlElementFactory.HGroupElementTagName:
                case HtmlElementFactory.MenuElementTagName:
                case HtmlElementFactory.NavElementTagName:
                case HtmlElementFactory.OLElementTagName:
                case HtmlElementFactory.ParaElementTagName:
                case HtmlElementFactory.SectionElementTagName:
                case HtmlElementFactory.ULElementTagName:
                    // A start tag whose tag name is one of: "address", "article", "aside", "blockquote",
                    // "center", "details", "dir", "div", "dl", "fieldset", "figure", "footer", "header", 
                    // "hgroup", "menu", "nav", "ol", "p", "section", "ul"
                    // If the stack of open elements has a p element in scope, then act as if an end tag 
                    // with the tag name "p" had been seen.
                    // Insert an HTML element for the token.
                    if (parser.OpenElementStack.HasElementOfTypeInScope(HtmlElementFactory.ParaElementTagName))
                    {
                        ProcessParaEndTag(new TagToken(TokenType.EndTag, HtmlElementFactory.ParaElementTagName), parser);
                    }

                    parser.InsertElement(tag, false, isFosterParentingRequired);
                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.H1ElementTagName:
                case HtmlElementFactory.H2ElementTagName:
                case HtmlElementFactory.H3ElementTagName:
                case HtmlElementFactory.H4ElementTagName:
                case HtmlElementFactory.H5ElementTagName:
                case HtmlElementFactory.H6ElementTagName:
                    // If the stack of open elements has a p element in scope, then act as if an end tag 
                    // with the tag name "p" had been seen.
                    // If the current node is an element whose tag name is one of "h1", "h2", "h3", "h4", 
                    // "h5", or "h6", then this is a parse error; pop the current node off the stack of 
                    // open elements.
                    // Insert an HTML element for the token.
                    ProcessHeaderStartTag(tag, parser);
                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.PreElementTagName:
                case HtmlElementFactory.ListingElementTagName:
                    // If the stack of open elements has a p element in scope, then act as if an end tag
                    // with the tag name "p" had been seen.
                    // Insert an HTML element for the token.
                    // If the next token is a U+000A LINE FEED (LF) character token, then ignore that token
                    // and move on to the next one. (Newlines at the start of pre blocks are ignored as an 
                    // authoring convenience.)
                    // Set the frameset-ok flag to "not ok".
                    if (parser.OpenElementStack.HasElementOfTypeInScope(HtmlElementFactory.ParaElementTagName))
                    {
                        ProcessParaEndTag(new TagToken(TokenType.EndTag, HtmlElementFactory.ParaElementTagName), parser);
                    }

                    parser.IgnoreNextLineFeedToken(tag, false, isFosterParentingRequired);
                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.FormElementTagName:
                    // If the form element pointer is not null, then this is a parse error; ignore the token.
                    // Otherwise:
                    // If the stack of open elements has a p element in scope, then act as if an end tag with 
                    // the tag name "p" had been seen.
                    // Insert an HTML element for the token, and set the form element pointer to point to the
                    // element created.
                    tokenProcessed = ProcessFormStartTag(tag, parser);
                    break;

                case HtmlElementFactory.LIElementTagName:
                    // Run these steps:
                    // 1.Set the frameset-ok flag to "not ok".
                    // 2.Initialize node to be the current node (the bottommost node of the stack).
                    // 3.Loop: If node is an li element, then act as if an end tag with the
                    // tag name "li" had been seen, then jump to the last step.
                    // 4.If node is not in the formatting category, and is not in the phrasing category,
                    // and is not an address, div, or p element, then jump to the last step.
                    // 5.Otherwise, set node to the previous entry in the stack of open elements and 
                    // return to the step labeled loop.
                    // 6.This is the last step.
                    // If the stack of open elements has a p element in scope, then act as if an end 
                    // tag with the tag name "p" had been seen.
                    // Finally, insert an HTML element for the token.
                    List<string> listItemElementToAutoCloseList = new List<string>() { HtmlElementFactory.LIElementTagName };
                    ProcessListItemStartTag(tag, parser, listItemElementToAutoCloseList);
                    tokenProcessed = true;
                    break;
                case HtmlElementFactory.DDElementTagName:
                case HtmlElementFactory.DTElementTagName:
                    // Run these steps:
                    // 1.Set the frameset-ok flag to "not ok".
                    // 2.Initialize node to be the current node (the bottommost node of the stack).
                    // 3.Loop: If node is a dd or dt element, then act as if an end tag with the
                    // same tag name as node had been seen, then jump to the last step.
                    // 4.If node is not in the formatting category, and is not in the phrasing category,
                    // and is not an address, div, or p element, then jump to the last step.
                    // 5.Otherwise, set node to the previous entry in the stack of open elements and 
                    // return to the step labeled loop.
                    // 6.This is the last step.
                    // If the stack of open elements has a p element in scope, then act as if an end 
                    // tag with the tag name "p" had been seen.
                    // Finally, insert an HTML element for the token.
                    List<string> directoryElementToAutoCloseList = new List<string>() { HtmlElementFactory.DDElementTagName, HtmlElementFactory.DTElementTagName };
                    ProcessListItemStartTag(tag, parser, directoryElementToAutoCloseList);
                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.PlainTextElementTagName:
                    // If the stack of open elements has a p element in scope, then act as if an end 
                    // tag with the tag name "p" had been seen.
                    // Insert an HTML element for the token.
                    // Switch the tokenizer to the PLAINTEXT state.
                    // Note: Once a start tag with the tag name "plaintext" has been seen, that will 
                    // be the last token ever seen other than character tokens (and the end-of-file 
                    // token), because there is no way to switch out of the PLAINTEXT state.
                    if (parser.OpenElementStack.HasElementOfTypeInScope(HtmlElementFactory.ParaElementTagName))
                    {
                        ProcessParaEndTag(new TagToken(TokenType.EndTag, HtmlElementFactory.ParaElementTagName), parser);
                    }

                    parser.ActivatePlainTextState(tag, isFosterParentingRequired);
                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.AnchorElementTagName:
                    // If the list of active formatting elements contains an element whose tag name is
                    // "a" between the end of the list and the last marker on the list (or the start of 
                    // the list if there is no marker on the list), then this is a parse error; act as 
                    // if an end tag with the tag name "a" had been seen, then remove that element from 
                    // the list of active formatting elements and the stack of open elements if the end 
                    // tag didn't already remove it (it might not have if the element is not in table scope).
                    // Reconstruct the active formatting elements, if any.
                    // Insert an HTML element for the token. Add that element to the list of active 
                    // formatting elements.
                    ProcessAnchorStartTag(tag, parser);
                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.BElementTagName:
                case HtmlElementFactory.BigElementTagName:
                case HtmlElementFactory.CodeElementTagName:
                case HtmlElementFactory.EmElementTagName:
                case HtmlElementFactory.FontElementTagName:
                case HtmlElementFactory.IElementTagName:
                case HtmlElementFactory.SElementTagName:
                case HtmlElementFactory.SmallElementTagName:
                case HtmlElementFactory.StrikeElementTagName:
                case HtmlElementFactory.StrongElementTagName:
                case HtmlElementFactory.TTElementTagName:
                case HtmlElementFactory.UElementTagName:
                    // A start tag whose tag name is one of: "b", "big", "code", "em", "font", "i", 
                    // "s", "small", "strike", "strong", "tt", "u"
                    // Reconstruct the active formatting elements, if any.
                    // Insert an HTML element for the token. Add that element to the list 
                    // of active formatting elements.
                    parser.ReconstructActiveFormattingElements(isFosterParentingRequired);
                    parser.InsertElement(tag, false, isFosterParentingRequired, ActiveFormattingElementListState.Add);
                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.NoBrElementTagName:
                    // Reconstruct the active formatting elements, if any.
                    // If the stack of open elements has a nobr element in scope, then this is a parse 
                    // error; act as if an end tag with the tag name "nobr" had been seen, then once 
                    // again reconstruct the active formatting elements, if any.
                    // Insert an HTML element for the token. Add that element to the list of active 
                    // formatting elements.
                    parser.ReconstructActiveFormattingElements(isFosterParentingRequired);
                    if (parser.OpenElementStack.HasElementOfTypeInScope(HtmlElementFactory.NoBrElementTagName))
                    {
                        parser.LogParseError("Found 'nobr' start tag with 'nobr' tag already in scope", "closing tag");
                        ProcessEndTagToken(new TagToken(TokenType.EndTag, HtmlElementFactory.NoBrElementTagName), parser);
                        parser.ReconstructActiveFormattingElements(isFosterParentingRequired);
                    }

                    parser.InsertElement(tag, false, isFosterParentingRequired, ActiveFormattingElementListState.Add);
                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.ButtonElementTagName:
                    // If the stack of open elements has a button element in scope, then this is a parse error; 
                    // act as if an end tag with the tag name "button" had been seen, then reprocess the token.
                    // Otherwise:
                    // Reconstruct the active formatting elements, if any.
                    // Insert an HTML element for the token.
                    // Insert a marker at the end of the list of active formatting elements.
                    // Set the frameset-ok flag to "not ok".
                    if (parser.OpenElementStack.HasElementOfTypeInScope(HtmlElementFactory.ButtonElementTagName))
                    {
                        parser.LogParseError("Found 'button' start tag with 'button' tag already in scope", "closing tag");
                        ProcessEndTagToken(new TagToken(TokenType.EndTag, HtmlElementFactory.ButtonElementTagName), parser);
                    }

                    parser.ReconstructActiveFormattingElements(isFosterParentingRequired);
                    parser.InsertElement(tag, false, isFosterParentingRequired, ActiveFormattingElementListState.AddAsMarker);
                    parser.IsFramesetOK = false;
                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.AppletElementTagName:
                case HtmlElementFactory.MarqueeElementTagName:
                case HtmlElementFactory.ObjectElementTagName:
                    // Reconstruct the active formatting elements, if any.
                    // Insert an HTML element for the token.
                    // Insert a marker at the end of the list of active formatting elements.
                    // Set the frameset-ok flag to "not ok".
                    parser.ReconstructActiveFormattingElements(isFosterParentingRequired);
                    parser.InsertElement(tag, false, isFosterParentingRequired, ActiveFormattingElementListState.AddAsMarker);
                    parser.IsFramesetOK = false;
                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.TableElementTagName:
                    // If the Document is not set to quirks mode, and the stack of open elements 
                    // has a p element in scope, then act as if an end tag with the tag name "p"
                    // had been seen.
                    // Insert an HTML element for the token.
                    // Set the frameset-ok flag to "not ok".
                    // Switch the insertion mode to "in table".
                    if (parser.DocumentQuirksMode != QuirksMode.Quirks && parser.OpenElementStack.HasElementOfTypeInScope(HtmlElementFactory.ParaElementTagName))
                    {
                        ProcessEndTagToken(new TagToken(TokenType.EndTag, HtmlElementFactory.ParaElementTagName), parser);
                    }

                    parser.InsertElement(tag, false, isFosterParentingRequired);
                    parser.IsFramesetOK = false;
                    tokenProcessed = true;
                    parser.AdvanceState(new InTableState());
                    break;

                case HtmlElementFactory.AreaElementTagName:
                case HtmlElementFactory.BaseFontElementTagName:
                case HtmlElementFactory.BgSoundElementTagName:
                case HtmlElementFactory.BRElementTagName:
                case HtmlElementFactory.EmbedElementTagName:
                case HtmlElementFactory.ImgElementTagName:
                case HtmlElementFactory.InputElementTagName:
                case HtmlElementFactory.KeyGenElementTagName:
                case HtmlElementFactory.WbrElementTagName:
                    // Reconstruct the active formatting elements, if any.
                    // Insert an HTML element for the token. Immediately pop the current node off the 
                    // stack of open elements.
                    // Acknowledge the token's self-closing flag, if it is set.
                    // Set the frameset-ok flag to "not ok".
                    parser.ReconstructActiveFormattingElements(isFosterParentingRequired);
                    parser.InsertElement(tag, true, isFosterParentingRequired);
                    parser.IsFramesetOK = false;
                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.HrElementTagName:
                    // If the stack of open elements has a p element in scope, then act as if an end 
                    // tag with the tag name "p" had been seen.
                    // Insert an HTML element for the token. Immediately pop the current node off the 
                    // stack of open elements.
                    // Acknowledge the token's self-closing flag, if it is set.
                    // Set the frameset-ok flag to "not ok".
                    if (parser.OpenElementStack.HasElementOfTypeInScope(HtmlElementFactory.ParaElementTagName))
                    {
                        ProcessEndTagToken(new TagToken(TokenType.EndTag, HtmlElementFactory.ParaElementTagName), parser);
                    }

                    parser.InsertElement(tag, true, isFosterParentingRequired);
                    parser.IsFramesetOK = false;
                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.ImageElementTagName:
                    // Parse error. Change the token's tag name to "img" and reprocess it. (Don't ask.)
                    parser.LogParseError("Found start token for 'image', which is illegal", "processing as 'img'");
                    tag.Name = HtmlElementFactory.ImgElementTagName;
                    tokenProcessed = ProcessStartTagToken(tag, parser);
                    break;

                case HtmlElementFactory.IsIndexElementTagName:
                    // Parse error.
                    // If the form element pointer is not null, then ignore the token.
                    // Otherwise:
                    // Acknowledge the token's self-closing flag, if it is set.
                    // Act as if a start tag token with the tag name "form" had been seen.
                    // If the token has an attribute called "action", set the action attribute on the 
                    // resulting form element to the value of the "action" attribute of the token.
                    // Act as if a start tag token with the tag name "hr" had been seen.
                    // Act as if a start tag token with the tag name "label" had been seen.
                    // Act as if a stream of character tokens had been seen (see below for what they should say).
                    // Act as if a start tag token with the tag name "input" had been seen, with all 
                    // the attributes from the "isindex" token except "name", "action", and "prompt".
                    // Set the name attribute of the resulting input element to the value "isindex".
                    // Act as if a stream of character tokens had been seen (see below for what they should say).
                    // Act as if an end tag token with the tag name "label" had been seen.
                    // Act as if a start tag token with the tag name "hr" had been seen.
                    // Act as if an end tag token with the tag name "form" had been seen.
                    // If the token has an attribute with the name "prompt", then the first stream of characters
                    // must be the same string as given in that attribute, and the second stream of characters 
                    // must be empty. Otherwise, the two streams of character tokens together should, together 
                    // with the input element, express the equivalent of "This is a searchable index. Insert 
                    // your search keywords here: (input field)" in the user's preferred language.
                    ProcessIsIndexStartTag(tag, parser);
                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.TextAreaElementTagName:
                    // Run these steps:
                    // 1.Insert an HTML element for the token.
                    // 2.If the next token is a U+000A LINE FEED (LF) character token, then ignore that 
                    // token and move on to the next one. (Newlines at the start of textarea elements are 
                    // ignored as an authoring convenience.)
                    // 3.Switch the tokenizer to the RCDATA state.
                    // 4.Let the original insertion mode be the current insertion mode.
                    // 5.Set the frameset-ok flag to "not ok".
                    // 6.Switch the insertion mode to "text".
                    parser.IgnoreNextLineFeedToken(tag, true, isFosterParentingRequired);
                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.XmpElementTagName:
                    // If the stack of open elements has a p element in scope, then act as if an end 
                    // tag with the tag name "p" had been seen.
                    // Reconstruct the active formatting elements, if any.
                    // Set the frameset-ok flag to "not ok".
                    // Follow the generic raw text element parsing algorithm.
                    if (parser.OpenElementStack.HasElementOfTypeInScope(HtmlElementFactory.ParaElementTagName))
                    {
                        ProcessEndTagToken(new TagToken(TokenType.EndTag, HtmlElementFactory.ParaElementTagName), parser);
                    }

                    parser.ReconstructActiveFormattingElements(isFosterParentingRequired);
                    parser.IsFramesetOK = false;
                    parser.ActivateRawTextState(tag, isFosterParentingRequired);
                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.IFrameElementTagName:
                    // Set the frameset-ok flag to "not ok".
                    // Follow the generic raw text element parsing algorithm.
                    parser.IsFramesetOK = false;
                    parser.ActivateRawTextState(tag, isFosterParentingRequired);
                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.NoEmbedElementTagName:
                    // Follow the generic raw text element parsing algorithm.
                    parser.ActivateRawTextState(tag, isFosterParentingRequired);
                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.NoScriptElementTagName:
                    // if the scripting flag is enabled
                    // Follow the generic raw text element parsing algorithm.
                    if (parser.IsScriptingEnabled)
                    {
                        parser.ActivateRawTextState(tag, isFosterParentingRequired);
                        tokenProcessed = true;
                    }

                    break;

                case HtmlElementFactory.SelectElementTagName:
                    // Reconstruct the active formatting elements, if any.
                    // Insert an HTML element for the token.
                    // Set the frameset-ok flag to "not ok".
                    // If the insertion mode is one of "in table", "in caption", "in column group", 
                    // "in table body", "in row", or "in cell", then switch the insertion mode to 
                    // "in select in table". Otherwise, switch the insertion mode to "in select".
                    parser.ReconstructActiveFormattingElements(isFosterParentingRequired);
                    parser.InsertElement(tag, false, isFosterParentingRequired);
                    parser.IsFramesetOK = false;
                    if (isInTable)
                    {
                        parser.AdvanceState(new InSelectInTableState());
                    }
                    else
                    {
                        parser.AdvanceState(new InSelectState());
                    }

                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.OptGroupElementTagName:
                case HtmlElementFactory.OptionElementTagName:
                    // If the stack of open elements has an option element in scope, then act as if 
                    // an end tag with the tag name "option" had been seen.
                    // Reconstruct the active formatting elements, if any.
                    // Insert an HTML element for the token.
                    if (parser.OpenElementStack.HasElementOfTypeInScope(HtmlElementFactory.OptionElementTagName))
                    {
                        ProcessEndTagToken(new TagToken(TokenType.EndTag, HtmlElementFactory.OptionElementTagName), parser);
                    }

                    parser.ReconstructActiveFormattingElements(isFosterParentingRequired);
                    parser.InsertElement(tag, false, isFosterParentingRequired);
                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.RpElementTagName:
                case HtmlElementFactory.RtElementTagName:
                    // If the stack of open elements has a ruby element in scope, then generate implied 
                    // end tags. If the current node is not then a ruby element, this is a parse error; 
                    // pop all the nodes from the current node up to the node immediately before the 
                    // bottommost ruby element on the stack of open elements.
                    // Insert an HTML element for the token.
                    if (parser.OpenElementStack.HasElementOfTypeInScope(HtmlElementFactory.RubyElementTagName))
                    {
                        parser.GenerateImpliedEndTags(string.Empty);

                        if (parser.CurrentNode.Name != HtmlElementFactory.RubyElementTagName)
                        {
                            parser.LogParseError("Start tag of '" + tag.Name + "': current element must be 'ruby'", "popping nodes from stack");
                            while (parser.CurrentNode.Name != HtmlElementFactory.RubyElementTagName)
                            {
                                parser.PopElementFromStack();
                            }
                        }
                    }

                    parser.InsertElement(tag, false, isFosterParentingRequired);
                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.MathElementTagName:
                    // Reconstruct the active formatting elements, if any.
                    // Adjust MathML attributes for the token. (This fixes the case of MathML attributes
                    // that are not all lowercase.)
                    // Adjust foreign attributes for the token. (This fixes the use of namespaced 
                    // attributes, in particular XLink.)
                    // Insert a foreign element for the token, in the MathML namespace.
                    // If the token has its self-closing flag set, pop the current node off the stack 
                    // of open elements and acknowledge the token's self-closing flag.
                    // Otherwise, if the insertion mode is not already "in foreign content", let the 
                    // secondary insertion mode be the current insertion mode, and then switch the 
                    // insertion mode to "in foreign content".
                    parser.ReconstructActiveFormattingElements(isFosterParentingRequired);
                    InForeignContentState.AdjustMathMLAttributes(tag);
                    InForeignContentState.AdjustForeignAttributes(tag);
                    parser.InsertElement(tag, tag.IsSelfClosing, isFosterParentingRequired, ActiveFormattingElementListState.None, Parser.MathMLNamespace);
                    if (!tag.IsSelfClosing && parser.State.GetType() != typeof(InForeignContentState))
                    {
                        parser.AdvanceState(new InForeignContentState(parser.State));
                    }

                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.SvgElementTagName:
                    // Reconstruct the active formatting elements, if any.
                    // Adjust SVG attributes for the token. (This fixes the case of SVG attributes 
                    // that are not all lowercase.)
                    // Adjust foreign attributes for the token. (This fixes the use of namespaced 
                    // attributes, in particular XLink in SVG.)
                    // Insert a foreign element for the token, in the SVG namespace.
                    // If the token has its self-closing flag set, pop the current node off the stack 
                    // of open elements and acknowledge the token's self-closing flag.
                    // Otherwise, if the insertion mode is not already "in foreign content", let the 
                    // secondary insertion mode be the current insertion mode, and then switch the 
                    // insertion mode to "in foreign content".
                    parser.ReconstructActiveFormattingElements(isFosterParentingRequired);
                    InForeignContentState.AdjustSvgAttributes(tag);
                    InForeignContentState.AdjustForeignAttributes(tag);
                    parser.InsertElement(tag, tag.IsSelfClosing, isFosterParentingRequired, ActiveFormattingElementListState.None, Parser.SvgNamespace);
                    if (!tag.IsSelfClosing && parser.State.GetType() != typeof(InForeignContentState))
                    {
                        parser.AdvanceState(new InForeignContentState(parser.State));
                    }

                    tokenProcessed = true;
                    break;

                case HtmlElementFactory.CaptionElementTagName:
                case HtmlElementFactory.ColElementTagName:
                case HtmlElementFactory.ColGroupElementTagName:
                case HtmlElementFactory.FrameElementTagName:
                case HtmlElementFactory.HeadElementTagName:
                case HtmlElementFactory.TBodyElementTagName:
                case HtmlElementFactory.TDElementTagName:
                case HtmlElementFactory.TFootElementTagName:
                case HtmlElementFactory.THElementTagName:
                case HtmlElementFactory.THeadElementTagName:
                case HtmlElementFactory.TRElementTagName:
                    // Parse error. Ignore the token.
                    parser.LogParseError("Found start tag for '" + tag.Name + "' directly in body", "ignoring token");
                    tokenProcessed = true;
                    break;

                default:
                    // Any other start tag
                    // Reconstruct the active formatting elements, if any.
                    // Insert an HTML element for the token.
                    parser.ReconstructActiveFormattingElements(isFosterParentingRequired);
                    parser.InsertElement(tag, false, isFosterParentingRequired);
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
                case HtmlElementFactory.BodyElementTagName:
                    tokenProcessed = ProcessBodyEndTag(tag, parser);
                    break;

                case HtmlElementFactory.HtmlElementTagName:
                    tokenProcessed = ProcessBodyEndTag(new TagToken(TokenType.EndTag, HtmlElementFactory.BodyElementTagName), parser);
                    if (tokenProcessed)
                    {
                        parser.AdvanceState(new AfterBodyState());
                        parser.State.ParseToken(parser);
                    }

                    break;
                    
                case HtmlElementFactory.AddressElementTagName:
                case HtmlElementFactory.ArticleElementTagName:
                case HtmlElementFactory.AsideElementTagName:
                case HtmlElementFactory.BlockQuoteElementTagName:
                case HtmlElementFactory.ButtonElementTagName:
                case HtmlElementFactory.CenterElementTagName:
                case HtmlElementFactory.DetailsElementTagName:
                case HtmlElementFactory.DirElementTagName:
                case HtmlElementFactory.DivElementTagName:
                case HtmlElementFactory.DLElementTagName:
                case HtmlElementFactory.FieldsetElementTagName:
                case HtmlElementFactory.FigureElementTagName:
                case HtmlElementFactory.FooterElementTagName:
                case HtmlElementFactory.HeaderElementTagName:
                case HtmlElementFactory.HGroupElementTagName:
                case HtmlElementFactory.ListingElementTagName:
                case HtmlElementFactory.MenuElementTagName:
                case HtmlElementFactory.NavElementTagName:
                case HtmlElementFactory.OLElementTagName:
                case HtmlElementFactory.PreElementTagName:
                case HtmlElementFactory.SectionElementTagName:
                case HtmlElementFactory.ULElementTagName:
                    // If the stack of open elements does not have an element in scope with the 
                    // same tag name as that of the token, then this is a parse error; ignore the token.
                    // Otherwise, run these steps:
                    // 1.Generate implied end tags.
                    // 2.If the current node is not an element with the same tag name as that of the
                    // token, then this is a parse error.
                    // 3.Pop elements from the stack of open elements until an element with the same 
                    // tag name as the token has been popped from the stack.
                    tokenProcessed = ProcessEndTag(tag, parser, ScopeType.Element, string.Empty, false);
                    break;

                case HtmlElementFactory.FormElementTagName:
                    // Let node be the element that the form element pointer is set to.
                    // Set the form element pointer to null.
                    // If node is null or the stack of open elements does not have node in scope,
                    // then this is a parse error; ignore the token.
                    // Otherwise, run these steps:
                    // 1.Generate implied end tags.
                    // 2.If the current node is not node, then this is a parse error.
                    // 3.Remove node from the stack of open elements.
                    HtmlElement formElement = parser.FormElement;
                    parser.FormElement = null;
                    if (formElement == null || !parser.OpenElementStack.HasElementInScope(formElement))
                    {
                        parser.LogParseError("End form tag found, but form element pointer was null or not in scope", "ignoring token");
                    }
                    else
                    {
                        parser.GenerateImpliedEndTags(string.Empty);
                        if (formElement != parser.CurrentNode)
                        {
                            parser.LogParseError("End form tag found. Expected element '" + tag.Name + "' at top of stack", "none");
                        }

                        parser.OpenElementStack.Remove(formElement);
                        tokenProcessed = true;
                    }
                    break;

                case HtmlElementFactory.LIElementTagName:
                    // If the stack of open elements does not have an element in list item scope 
                    // with the same tag name as that of the token, then this is a parse error; 
                    // ignore the token.
                    // Otherwise, run these steps:
                    // 1.Generate implied end tags, except for elements with the same tag name 
                    // as the token.
                    // 2.If the current node is not an element with the same tag name as that 
                    // of the token, then this is a parse error.
                    // 3.Pop elements from the stack of open elements until an element with the 
                    // same tag name as the token has been popped from the stack.
                    tokenProcessed = ProcessEndTag(tag, parser, ScopeType.ListItem, tag.Name, false);
                    break;

                case HtmlElementFactory.ParaElementTagName:
                    // If the stack of open elements does not have an element in scope with the 
                    // same tag name as that of the token, then this is a parse error; act as if 
                    // a start tag with the tag name "p" had been seen, then reprocess the current 
                    // token.
                    // Otherwise, run these steps:
                    // 1.Generate implied end tags, except for elements with the same tag name as 
                    // the token.
                    // 2.If the current node is not an element with the same tag name as that of 
                    // the token, then this is a parse error.
                    // 3.Pop elements from the stack of open elements until an element with the 
                    // same tag name as the token has been popped from the stack.
                    tokenProcessed = ProcessEndTag(tag, parser, ScopeType.Element, tag.Name, true);
                    break;

                case HtmlElementFactory.DDElementTagName:
                case HtmlElementFactory.DTElementTagName:
                    // If the stack of open elements does not have an element in scope with the same 
                    // tag name as that of the token, then this is a parse error; ignore the token.
                    // Otherwise, run these steps:
                    // 1.Generate implied end tags, except for elements with the same tag name as the token.
                    // 2.If the current node is not an element with the same tag name as that of the token, 
                    // then this is a parse error.
                    // 3.Pop elements from the stack of open elements until an element with the same
                    // tag name as the token has been popped from the stack.
                    tokenProcessed = ProcessEndTag(tag, parser, ScopeType.Element, tag.Name, false);
                    break;

                case HtmlElementFactory.H1ElementTagName:
                case HtmlElementFactory.H2ElementTagName:
                case HtmlElementFactory.H3ElementTagName:
                case HtmlElementFactory.H4ElementTagName:
                case HtmlElementFactory.H5ElementTagName:
                case HtmlElementFactory.H6ElementTagName:
                    // If the stack of open elements does not have an element in scope whose tag name is 
                    // one of "h1", "h2", "h3", "h4", "h5", or "h6", then this is a parse error; ignore the token.
                    // Otherwise, run these steps:
                    // 1.Generate implied end tags.
                    // 2.If the current node is not an element with the same tag name as that of the token, 
                    // then this is a parse error.
                    // 3.Pop elements from the stack of open elements until an element whose tag name is 
                    // one of "h1", "h2", "h3", "h4", "h5", or "h6" has been popped from the stack.
                    if (!parser.OpenElementStack.HasElementOfTypeInScope(HtmlElementFactory.H1ElementTagName) &&
                        !parser.OpenElementStack.HasElementOfTypeInScope(HtmlElementFactory.H2ElementTagName) &&
                        !parser.OpenElementStack.HasElementOfTypeInScope(HtmlElementFactory.H3ElementTagName) &&
                        !parser.OpenElementStack.HasElementOfTypeInScope(HtmlElementFactory.H4ElementTagName) &&
                        !parser.OpenElementStack.HasElementOfTypeInScope(HtmlElementFactory.H5ElementTagName) &&
                        !parser.OpenElementStack.HasElementOfTypeInScope(HtmlElementFactory.H6ElementTagName))
                    {
                        parser.LogParseError("Header element end tag found. Expected header element in scope", "ignoring token");
                    }
                    else
                    {
                        parser.GenerateImpliedEndTags(string.Empty);
                        if (tag.Name != parser.CurrentNode.Name)
                        {
                            parser.LogParseError("End tag found. Expected element '" + tag.Name + "' at top of stack", "none");
                        }

                        while (parser.CurrentNode.Name != HtmlElementFactory.H1ElementTagName &&
                            parser.CurrentNode.Name != HtmlElementFactory.H2ElementTagName &&
                            parser.CurrentNode.Name != HtmlElementFactory.H3ElementTagName &&
                            parser.CurrentNode.Name != HtmlElementFactory.H4ElementTagName &&
                            parser.CurrentNode.Name != HtmlElementFactory.H5ElementTagName &&
                            parser.CurrentNode.Name != HtmlElementFactory.H6ElementTagName)
                        {
                            parser.PopElementFromStack();
                        }

                        tokenProcessed = true;
                    }

                    break;

                case HtmlElementFactory.AnchorElementTagName:
                case HtmlElementFactory.BElementTagName:
                case HtmlElementFactory.BigElementTagName:
                case HtmlElementFactory.CodeElementTagName:
                case HtmlElementFactory.EmElementTagName:
                case HtmlElementFactory.FontElementTagName:
                case HtmlElementFactory.IElementTagName:
                case HtmlElementFactory.NoBrElementTagName:
                case HtmlElementFactory.SElementTagName:
                case HtmlElementFactory.SmallElementTagName:
                case HtmlElementFactory.StrikeElementTagName:
                case HtmlElementFactory.StrongElementTagName:
                case HtmlElementFactory.TTElementTagName:
                case HtmlElementFactory.UElementTagName:
                    tokenProcessed = ProcessEndTagWithAdoptionAgency(tag, parser);
                    break;

                case HtmlElementFactory.AppletElementTagName:
                case HtmlElementFactory.MarqueeElementTagName:
                case HtmlElementFactory.ObjectElementTagName:
                    tokenProcessed = ProcessEndTag(tag, parser, ScopeType.Element, string.Empty, false);
                    parser.ActiveFormattingElementList.ClearToLastMarker();
                    break;

                case HtmlElementFactory.BRElementTagName:
                    // Parse error. Act as if a start tag token with the tag name "br" had been seen. 
                    // Ignore the end tag token.
                    parser.LogParseError("End tag for 'br' element seen.", "acting as if 'br' start token seen, and ignoring end tag token");
                    ProcessStartTagToken(new TagToken(TokenType.StartTag, tag.Name), parser);
                    tokenProcessed = true;
                    break;

                default:
                    // Any other end tag
                    // Run these steps:
                    // 1.Initialize node to be the current node (the bottommost node of the stack).
                    // 2.If node has the same tag name as the token, then:
                    //     1.Generate implied end tags.
                    //     2.If the tag name of the end tag token does not match the tag name of the current node, 
                    //     this is a parse error.
                    //     3.Pop all the nodes from the current node up to node, including node, then stop these steps.
                    // 3.Otherwise, if node is in neither the formatting category nor the phrasing category, 
                    // then this is a parse error; ignore the token, and abort these steps.
                    // 4.Set node to the previous entry in the stack of open elements.
                    // 5.Return to step 2.
                    bool continueProcessing = true;
                    int nodeIndex = 0;
                    while (continueProcessing && nodeIndex < parser.OpenElementStack.Count)
                    {
                        HtmlElement node = parser.OpenElementStack[nodeIndex];
                        if (node.Name == tag.Name)
                        {
                            parser.GenerateImpliedEndTags(string.Empty);
                            if (tag.Name != parser.CurrentNode.Name)
                            {
                                parser.LogParseError("After generating implied end tags, current element name '" + parser.CurrentNode.Name + "' does not equal tag name '" + tag.Name + "'", "none");
                            }

                            while (parser.OpenElementStack.Contains(node))
                            {
                                parser.PopElementFromStack();
                            }

                            continueProcessing = false;
                        }
                        else
                        {
                            if (!HtmlElementFactory.IsFormattingElementName(node.Name) && !HtmlElementFactory.IsPhrasingElementName(node.Name))
                            {
                                parser.LogParseError("Current element name '" + parser.CurrentNode.Name + "' is not in phrasing or formatting category", "ignoring token");
                                continueProcessing = false;
                            }
                            else
                            {
                                nodeIndex++; 
                            }
                        }
                    }

                    tokenProcessed = true;
                    break;
            }

            return tokenProcessed;
        }

        protected override bool ProcessUnprocessedToken(Parser parser)
        {
            return true;
        }

        protected override bool ProcessEndOfFileToken(Parser parser)
        {
            List<string> legalOpenTokenNames = new List<string>();
            legalOpenTokenNames.Add(HtmlElementFactory.DDElementTagName);
            legalOpenTokenNames.Add(HtmlElementFactory.DTElementTagName);
            legalOpenTokenNames.Add(HtmlElementFactory.LIElementTagName);
            legalOpenTokenNames.Add(HtmlElementFactory.ParaElementTagName);
            legalOpenTokenNames.Add(HtmlElementFactory.TBodyElementTagName);
            legalOpenTokenNames.Add(HtmlElementFactory.TDElementTagName);
            legalOpenTokenNames.Add(HtmlElementFactory.TFootElementTagName);
            legalOpenTokenNames.Add(HtmlElementFactory.THElementTagName);
            legalOpenTokenNames.Add(HtmlElementFactory.THeadElementTagName);
            legalOpenTokenNames.Add(HtmlElementFactory.TRElementTagName);
            legalOpenTokenNames.Add(HtmlElementFactory.BodyElementTagName);
            legalOpenTokenNames.Add(HtmlElementFactory.HtmlElementTagName);

            foreach (HtmlElement element in parser.OpenElementStack)
            {
                if (!legalOpenTokenNames.Contains(element.Name))
                {
                    parser.LogParseError("Open '" + element.Name + "' token still present at end of file", "none, parsing ending");
                }
            }

            return true;
        }

        private void ProcessAnchorStartTag(TagToken tag, Parser parser)
        {
            HtmlElement element = parser.ActiveFormattingElementList.GetLastElement(tag.Name);
            if (element != null)
            {
                parser.LogParseError("Unclosed 'a' element on list of active formatting elements", "closing tag and removing from list");
                ProcessEndTagToken(new TagToken(TokenType.EndTag, HtmlElementFactory.AnchorElementTagName), parser);
                if (parser.ActiveFormattingElementList.Contains(element))
                {
                    parser.ActiveFormattingElementList.Remove(element);
                }

                if (parser.OpenElementStack.Contains(element))
                {
                    parser.OpenElementStack.Remove(element);
                }
            }

            parser.ReconstructActiveFormattingElements(isFosterParentingRequired);
            parser.InsertElement(tag, false, isFosterParentingRequired, ActiveFormattingElementListState.Add);
        }

        private void ProcessIsIndexStartTag(TagToken tag, Parser parser)
        {
            parser.LogParseError("Found 'isindex' start tag, which is unsupported", "none");
            if (parser.FormElement == null)
            {
                string actionAttribute = string.Empty;
                if (tag.Attributes.ContainsKey("action"))
                {
                    actionAttribute = tag.Attributes["action"].Value;
                }

                string promptAttribute = "This is a searchable index. Enter search keywords: ";
                string inputPrompt = string.Empty;
                if (tag.Attributes.ContainsKey("prompt"))
                {
                    promptAttribute = tag.Attributes["prompt"].Value;
                    inputPrompt = string.Empty;
                }

                ProcessStartTagToken(new TagToken(TokenType.StartTag, HtmlElementFactory.FormElementTagName), parser);
                if (!string.IsNullOrEmpty(actionAttribute))
                {
                    parser.FormElement.SetAttribute("action", actionAttribute);
                }

                TagToken hrToken = new TagToken(TokenType.StartTag, HtmlElementFactory.HrElementTagName);
                hrToken.IsSelfClosing = true;
                ProcessStartTagToken(hrToken, parser);
                ProcessStartTagToken(new TagToken(TokenType.StartTag, HtmlElementFactory.LabelElementTagName), parser);
                foreach (char ch in promptAttribute)
                {
                    ProcessCharacterToken(new CharacterToken(ch), parser);
                }

                TagToken inputToken = new TagToken(TokenType.StartTag, HtmlElementFactory.InputElementTagName);
                inputToken.Attributes.Add("name", new TagTokenAttribute("name", "isindex"));
                foreach (string attributeName in tag.Attributes.Keys)
                {
                    if (attributeName != "name" && attributeName != "action" && attributeName != "prompt")
                    {
                        inputToken.Attributes.Add(attributeName, tag.Attributes[attributeName]);
                    }
                }

                ProcessStartTagToken(inputToken, parser);
                foreach (char ch in inputPrompt)
                {
                    ProcessCharacterToken(new CharacterToken(ch), parser);
                }


                ProcessEndTagToken(new TagToken(TokenType.EndTag, HtmlElementFactory.LabelElementTagName), parser);
                ProcessStartTagToken(hrToken, parser);
                ProcessEndTagToken(new TagToken(TokenType.EndTag, HtmlElementFactory.FormElementTagName), parser);
            }
        }

        private bool ProcessEndTag(TagToken tag, Parser parser, ScopeType stackScope, string tagNameToExclude, bool reprocessToken)
        {
            bool tokenProcessed = false;
            if (!parser.OpenElementStack.HasElementOfTypeInScope(tag.Name, stackScope))
            {
                if (!reprocessToken)
                {
                    parser.LogParseError("End tag found. Expected element '" + tag.Name + "' in scope", "ignoring token");
                }
                else
                {
                    parser.LogParseError("End tag found. Expected element '" + tag.Name + "' in scope", "acting as if start token was found then reprocessing");
                    ProcessStartTagToken(new TagToken(TokenType.StartTag, tag.Name), parser);
                    ProcessEndTagToken(tag, parser);
                }
            }
            else
            {
                parser.GenerateImpliedEndTags(tagNameToExclude);
                if (tag.Name != parser.CurrentNode.Name)
                {
                    parser.LogParseError("End tag found. Expected element '" + tag.Name + "' at top of stack", "none");
                }

                while (tag.Name != parser.CurrentNode.Name)
                {
                    parser.PopElementFromStack();
                }

                // Element with tag name is on top of stack. Pop it.
                parser.PopElementFromStack();

                tokenProcessed = true;
            }
            return tokenProcessed;
        }

        private bool ProcessEndTagWithAdoptionAgency(TagToken tag, Parser parser)
        {
            bool tokenProcessed = false;
            bool continueProcessing = true;

            while (continueProcessing)
            {
                // 1.Let the formatting element be the last element in the list of active 
                // formatting elements that:
                //     is between the end of the list and the last scope marker in the list, if any, 
                //     or the start of the list otherwise, and
                //     has the same tag name as the token.
                // If there is no such node, or, if that node is also in the stack of open elements 
                // but the element is not in scope, then this is a parse error; ignore the token, 
                // and abort these steps.
                //
                // Otherwise, if there is such a node, but that node is not in the stack of open 
                // elements, then this is a parse error; remove the element from the list, and abort 
                // these steps.
                // 
                // Otherwise, there is a formatting element and that element is in the stack and is 
                // in scope. If the element is not the current node, this is a parse error. In any 
                // case, proceed with the algorithm as written in the following steps.
                HtmlElement formattingElement = parser.ActiveFormattingElementList.GetLastElement(tag.Name);
                if (formattingElement == null || (parser.OpenElementStack.Contains(formattingElement) && !parser.OpenElementStack.HasElementInScope(formattingElement)))
                {
                    parser.LogParseError("Found '" + tag.Name + "' end tag, but tag not in list of active formatting elements", "ignoring token");
                    continueProcessing = false;
                    tokenProcessed = true;
                }
                else
                {
                    if (!parser.OpenElementStack.Contains(formattingElement))
                    {
                        parser.LogParseError("Found '" + tag.Name + "' end tag, but tag not in open element stack", "removing from active formatting element list");
                        parser.ActiveFormattingElementList.Remove(formattingElement);
                        continueProcessing = false;
                        tokenProcessed = true;
                    }
                    else
                    {
                        if (formattingElement != parser.CurrentNode)
                        {
                            parser.LogParseError("Found '" + tag.Name + "' end tag, but element is not current element", "none");
                        }

                        // 2.Let the furthest block be the topmost node in the stack of open elements that is 
                        // lower in the stack than the formatting element, and is not an element in the phrasing 
                        // or formatting categories. There might not be one.
                        int formattingElementIndex = parser.OpenElementStack.IndexOf(formattingElement);
                        int furthestBlockIndex = formattingElementIndex - 1;
                        HtmlElement furthestBlock = null;
                        while (furthestBlock == null && furthestBlockIndex >= 0)
                        {
                            HtmlElement currentElement = parser.OpenElementStack[furthestBlockIndex];
                            if (!HtmlElementFactory.IsFormattingElementName(currentElement.Name) && !HtmlElementFactory.IsPhrasingElementName(currentElement.Name))
                            {
                                furthestBlock = currentElement;
                            }
                            else
                            {
                                furthestBlockIndex--;
                            }
                        }

                        // 3. If there is no furthest block, then the UA must skip the subsequent steps 
                        // and instead just pop all the nodes from the bottom of the stack of open elements, 
                        // from the current node up to and including the formatting element, and remove the 
                        // formatting element from the list of active formatting elements.
                        if (furthestBlock == null)
                        {
                            for (int i = 0; i <= formattingElementIndex; i++)
                            {
                                parser.PopElementFromStack();
                            }

                            parser.ActiveFormattingElementList.Remove(formattingElement);
                            continueProcessing = false;
                        }
                        else
                        {
                            // 4.Let the common ancestor be the element immediately above the formatting 
                            // element in the stack of open elements.
                            HtmlElement commonAncestor = parser.OpenElementStack[formattingElementIndex + 1];

                            // 5.Let a bookmark note the position of the formatting element in the list 
                            // of active formatting elements relative to the elements on either side of 
                            // it in the list.
                            int bookmark = parser.ActiveFormattingElementList.IndexOf(formattingElement);

                            // 6.Let node and last node be the furthest block. Follow these steps:
                            HtmlElement node = furthestBlock;
                            HtmlElement lastNode = furthestBlock;
                            int nodeIndex = furthestBlockIndex;
                            bool continueLoop = true;

                            while (continueLoop)
                            {
                                // 1.Let node be the element immediately above node in the stack of open elements, 
                                // or if node is no longer in the stack of open elements (e.g. because it got 
                                // removed by the next step), the element that was immediately above node in 
                                // the stack of open elements before node was removed.
                                nodeIndex++;
                                node = parser.OpenElementStack[nodeIndex];

                                // 2.If node is not in the list of active formatting elements, then remove node 
                                // from the stack of open elements and then go back to step 1.
                                if (!parser.ActiveFormattingElementList.Contains(node))
                                {
                                    parser.OpenElementStack.RemoveAt(nodeIndex);
                                    nodeIndex--;
                                }
                                else
                                {
                                    if (node == formattingElement)
                                    {
                                        // 3.Otherwise, if node is the formatting element, then go to the next step in the
                                        // overall algorithm.
                                        continueLoop = false;
                                    }
                                    else
                                    {
                                        // 4.Otherwise, if last node is the furthest block, then move the aforementioned 
                                        // bookmark to be immediately after the node in the list of active formatting elements.
                                        if (lastNode == furthestBlock)
                                        {
                                            bookmark = parser.ActiveFormattingElementList.IndexOf(node);
                                        }

                                        // 5.Create an element for the token for which the element node was created, 
                                        // replace the entry for node in the list of active formatting elements with 
                                        // an entry for the new element, replace the entry for node in the stack of 
                                        // open elements with an entry for the new element, and let node be the new element.
                                        HtmlElement newElement = parser.CreateElement(new TagToken(TokenType.StartTag, node.Name), Parser.HtmlNamespace);
                                        parser.OpenElementStack.ReplaceElement(node, newElement);
                                        parser.ActiveFormattingElementList.ReplaceElement(node, newElement);
                                        node = newElement;

                                        // 6.Insert last node into node, first removing it from its previous parent node if any.
                                        if (lastNode.ParentNode != null)
                                        {
                                            lastNode.ParentNode.RemoveChild(lastNode);
                                        }

                                        node.AppendChild(lastNode);

                                        // 7.Let last node be node.
                                        lastNode = node;
                                        //nodeIndex = parser.OpenElementStack.IndexOf(node);
                                        // 8.Return to step 1 of this inner set of steps.
                                    }
                                }
                            }

                            // 7.If the common ancestor node is a table, tbody, tfoot, thead, or tr element, 
                            // then, foster parent whatever last node ended up being in the previous step, 
                            // first removing it from its previous parent node if any.
                            // Otherwise, append whatever last node ended up being in the previous step to 
                            // the common ancestor node, first removing it from its previous parent node if any.
                            if (commonAncestor.Name == HtmlElementFactory.TableElementTagName ||
                                commonAncestor.Name == HtmlElementFactory.TBodyElementTagName ||
                                commonAncestor.Name == HtmlElementFactory.TFootElementTagName ||
                                commonAncestor.Name == HtmlElementFactory.THeadElementTagName ||
                                commonAncestor.Name == HtmlElementFactory.TRElementTagName)
                            {
                                parser.FosterParentElement(lastNode);
                            }
                            else
                            {
                                if (lastNode.ParentNode != null)
                                {
                                    lastNode.ParentNode.RemoveChild(lastNode);
                                }

                                commonAncestor.AppendChild(lastNode);
                            }

                            // 8.Create an element for the token for which the formatting element was created.
                            HtmlElement newFormattingElement = parser.CreateElement(new TagToken(TokenType.StartTag, formattingElement.Name), Parser.HtmlNamespace);

                            // 9.Take all of the child nodes of the furthest block and append them to the 
                            // element created in the last step.
                            while (furthestBlock.HasChildNodes)
                            {
                                newFormattingElement.AppendChild(furthestBlock.FirstChild);
                            }

                            // 10.Append that new element to the furthest block.
                            furthestBlock.AppendChild(newFormattingElement);

                            // 11.Remove the formatting element from the list of active formatting 
                            // elements, and insert the new element into the list of active formatting
                            // elements at the position of the aforementioned bookmark.
                            parser.ActiveFormattingElementList.Remove(formattingElement);
                            parser.ActiveFormattingElementList.Insert(bookmark, newFormattingElement);

                            // 12.Remove the formatting element from the stack of open elements, and insert 
                            // the new element into the stack of open elements immediately below the position 
                            // of the furthest block in that stack.
                            parser.OpenElementStack.Remove(formattingElement);
                            parser.OpenElementStack.Insert(furthestBlockIndex, newFormattingElement);

                            // 13.Jump back to step 1 in this series of steps.
                        }
                    }

                    tokenProcessed = true;
                }
            }
            return tokenProcessed;
        }

        private void ProcessHtmlStartTag(TagToken tag, Parser parser)
        {
            parser.LogParseError("Cannot have 'html' start tag in '" + Description + "' insertion mode", "appending attributes to existing html tag");
            HtmlElement element = parser.OpenElementStack[parser.OpenElementStack.Count - 1];
            foreach (string attributeName in tag.Attributes.Keys)
            {
                if (!element.HasAttribute(attributeName))
                {
                    element.SetAttribute(attributeName, tag.Attributes[attributeName].Value);
                }
            }
        }

        private void ProcessBodyStartTag(TagToken tag, Parser parser)
        {
            string action = "ignoring token";
            if (BodyElementIsPresent(parser))
            {
                action = "appending attributes to existing body tag";
                HtmlElement element = parser.OpenElementStack[parser.OpenElementStack.Count - 2];
                foreach (string attributeName in tag.Attributes.Keys)
                {
                    if (!element.HasAttribute(attributeName))
                    {
                        element.SetAttribute(attributeName, tag.Attributes[attributeName].Value);
                    }
                }
            }

            parser.LogParseError("Cannot have 'body' start tag in '" + Description + "' insertion mode", action);
        }

        private void ProcessFramesetStartTag(TagToken tag, Parser parser)
        {
            string action = "ignoring token";
            if (BodyElementIsPresent(parser) && parser.IsFramesetOK)
            {
                int bodyElementStackIndex = parser.OpenElementStack.Count - 2;
                HtmlElement element = parser.OpenElementStack[bodyElementStackIndex];
                if (element.ParentNode != null)
                {
                    element.ParentNode.RemoveChild(element);
                }

                while (bodyElementStackIndex >= 0)
                {
                    parser.OpenElementStack.Pop();
                    bodyElementStackIndex--;
                }

                parser.InsertElement(tag, false, isFosterParentingRequired);
                parser.AdvanceState(new InFramesetState());
            }

            parser.LogParseError("Cannot have 'frameset' start tag in '" + Description + "' insertion mode", action);
        }

        private void ProcessHeaderStartTag(TagToken tag, Parser parser)
        {
            if (parser.OpenElementStack.HasElementOfTypeInScope(HtmlElementFactory.ParaElementTagName))
            {
                ProcessParaEndTag(new TagToken(TokenType.EndTag, HtmlElementFactory.ParaElementTagName), parser);
            }

            if (parser.CurrentNode.Name == HtmlElementFactory.H1ElementTagName ||
                parser.CurrentNode.Name == HtmlElementFactory.H2ElementTagName ||
                parser.CurrentNode.Name == HtmlElementFactory.H3ElementTagName ||
                parser.CurrentNode.Name == HtmlElementFactory.H4ElementTagName ||
                parser.CurrentNode.Name == HtmlElementFactory.H5ElementTagName ||
                parser.CurrentNode.Name == HtmlElementFactory.H6ElementTagName)
            {
                parser.LogParseError("Attempting to nest '" + tag.Name + "' inside '" + parser.CurrentNode.Name, "popping element off stack");
                parser.PopElementFromStack();
            }

            parser.InsertElement(tag, false, isFosterParentingRequired);
        }

        private bool ProcessFormStartTag(TagToken tag, Parser parser)
        {
            bool tokenProcessed = false;
            if (parser.FormElement != null)
            {
                parser.LogParseError("Found a 'form' start tag while form pointer is not null", "ignoring token");
            }
            else
            {
                if (parser.OpenElementStack.HasElementOfTypeInScope(HtmlElementFactory.ParaElementTagName))
                {
                    ProcessParaEndTag(new TagToken(TokenType.EndTag, HtmlElementFactory.ParaElementTagName), parser);
                }

                parser.InsertElement(tag, false, isFosterParentingRequired);
                parser.FormElement = parser.CurrentNode as HtmlFormElement;
                tokenProcessed = true;
            }

            return tokenProcessed;
        }

        private void ProcessListItemStartTag(TagToken tag, Parser parser, List<string> elementsToAutoClose)
        {
            parser.IsFramesetOK = false;
            bool elementFound = false;
            int elementIndex = 0;
            while (elementIndex < parser.OpenElementStack.Count && !elementFound)
            {
                HtmlElement currentElement = parser.OpenElementStack[elementIndex];
                if (elementsToAutoClose.Contains(currentElement.Name))
                {
                    ProcessEndTagToken(new TagToken(TokenType.EndTag, currentElement.Name), parser);
                    elementFound = true;
                }

                if (!elementFound &&
                    !HtmlElementFactory.IsFormattingElementName(currentElement.Name) &&
                    !HtmlElementFactory.IsPhrasingElementName(currentElement.Name) &&
                    currentElement.Name != HtmlElementFactory.AddressElementTagName &&
                    currentElement.Name != HtmlElementFactory.DivElementTagName &&
                    currentElement.Name != HtmlElementFactory.ParaElementTagName)
                {
                    elementFound = true;
                }

                if (!elementFound)
                {
                    elementIndex++;
                }
            }

            if (parser.OpenElementStack.HasElementOfTypeInScope(HtmlElementFactory.ParaElementTagName))
            {
                ProcessParaEndTag(new TagToken(TokenType.EndTag, HtmlElementFactory.ParaElementTagName), parser);
            }

            parser.InsertElement(tag, false, isFosterParentingRequired);
        }

        private bool ProcessBodyEndTag(TagToken tag, Parser parser)
        {
            bool tokenProcessed = false;
            if (!parser.OpenElementStack.HasElementOfTypeInScope(HtmlElementFactory.BodyElementTagName))
            {
                parser.LogParseError("Ending body tag found but no body element in scope", "ignoring token");
            }
            else
            {
                List<string> legalOpenTokenNames = new List<string>();
                legalOpenTokenNames.Add(HtmlElementFactory.DDElementTagName);
                legalOpenTokenNames.Add(HtmlElementFactory.DTElementTagName);
                legalOpenTokenNames.Add(HtmlElementFactory.LIElementTagName);
                legalOpenTokenNames.Add(HtmlElementFactory.OptGroupElementTagName);
                legalOpenTokenNames.Add(HtmlElementFactory.OptionElementTagName);
                legalOpenTokenNames.Add(HtmlElementFactory.ParaElementTagName);
                legalOpenTokenNames.Add(HtmlElementFactory.RpElementTagName);
                legalOpenTokenNames.Add(HtmlElementFactory.RtElementTagName);
                legalOpenTokenNames.Add(HtmlElementFactory.TBodyElementTagName);
                legalOpenTokenNames.Add(HtmlElementFactory.TDElementTagName);
                legalOpenTokenNames.Add(HtmlElementFactory.TFootElementTagName);
                legalOpenTokenNames.Add(HtmlElementFactory.THElementTagName);
                legalOpenTokenNames.Add(HtmlElementFactory.THeadElementTagName);
                legalOpenTokenNames.Add(HtmlElementFactory.TRElementTagName);
                legalOpenTokenNames.Add(HtmlElementFactory.BodyElementTagName);
                legalOpenTokenNames.Add(HtmlElementFactory.HtmlElementTagName);

                foreach (HtmlElement element in parser.OpenElementStack)
                {
                    if (!legalOpenTokenNames.Contains(element.Name))
                    {
                        parser.LogParseError("Open '" + element.Name + "' token still present at end body token", "none");
                    }
                }

                parser.AdvanceState(new AfterBodyState());
                tokenProcessed = true;
            }

            return tokenProcessed;
        }

        private void ProcessParaEndTag(TagToken tagToken, Parser parser)
        {
            TagToken fakeParaEndTag = new TagToken(TokenType.EndTag, HtmlElementFactory.ParaElementTagName);
            ProcessEndTag(fakeParaEndTag, parser, ScopeType.Element, fakeParaEndTag.Name, false);
        }

        private bool BodyElementIsPresent(Parser parser)
        {
            return parser.OpenElementStack.Count > 1 && parser.OpenElementStack[parser.OpenElementStack.Count - 2].Name == HtmlElementFactory.BodyElementTagName;
        }
    }
}
