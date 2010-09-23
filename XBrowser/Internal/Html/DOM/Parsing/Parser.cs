using System;
using System.Collections.Generic;
using System.Xml;
using System.Collections.ObjectModel;
using System.IO;
using XBrowserProject.Internal.Html.DOM.Parsing.HtmlTokenizer;
using XBrowserProject.Internal.Html.DOM.Parsing.States;
using XBrowserProject.Internal.Html.Interfaces;

namespace XBrowserProject.Internal.Html.DOM.Parsing
{
    internal enum ActiveFormattingElementListState
    {
        None,
        Add,
        AddAsMarker
    }

    internal class Parser
    {
    	private readonly Uri _url;
    	public const string EmptyDocTypeName = "::EmptyDocType::";
        public const string HtmlNamespace = "http://www.w3.org/1999/xhtml";
        public const string MathMLNamespace = "http://www.w3.org/1998/Math/MathML";
        public const string SvgNamespace = "http://www.w3.org/2000/svg";
        public const string XLinkNamespace = "http://www.w3.org/1999/xlink";
        public const string XmlNamespace = "http://www.w3.org/XML/1998/namespace";
        public const string XmlNsNamespace = "http://www.w3.org/2000/xmlns/";

        private ElementStack openElementStack = new ElementStack();
        private FormattingElementList activeFormattingElementList = new FormattingElementList();
        private Tokenizer lexer;
        private HtmlDocument doc;
        private Token currentToken;
        private ParserState state = new InitialState();
        private List<ParseError> parseErrors = new List<ParseError>();
        private XmlNode lastInsertedNode;
        private HtmlElement headElement;
        private HtmlFormElement formElement;
        private List<CharacterToken> pendingTableCharacterTokens;
        private HtmlElement contextElement;
        private QuirksMode documentQuirksMode = QuirksMode.None;
        private bool isScriptingEnabled = true;
        private bool isFragmentParser = false;
        private bool isFramesetOK = false;
        private bool isIFrameSourceDoc = false;
        private int scriptNestingLevel;

        private HtmlTextReader reader;

        public int ScriptNestingLevel
        {
            get { return scriptNestingLevel; }
            set { scriptNestingLevel = value; }
        }

        public Parser(Window window, Uri url)
        {
        	_url = url;
        	doc = new HtmlDocument(window, url);
        }

    	public ReadOnlyCollection<ParseError> ParseErrors
        {
            get { return parseErrors.AsReadOnly(); }
        }

        public List<CharacterToken> PendingTableCharacterTokens
        {
            get { return pendingTableCharacterTokens; }
            set { pendingTableCharacterTokens = value; }
        }

        public QuirksMode DocumentQuirksMode
        {
            get { return documentQuirksMode; }
            set { documentQuirksMode = value; }
        }

        public FormattingElementList ActiveFormattingElementList
        {
            get { return activeFormattingElementList; }
        }

        public ElementStack OpenElementStack
        {
            get { return openElementStack; }
        }

        public bool IsFramesetOK
        {
            get { return isFramesetOK; }
            set { isFramesetOK = value; }
        }

        public HtmlElement HeadElement
        {
            get { return headElement; }
            set { headElement = value; }
        }

        public HtmlFormElement FormElement
        {
            get { return formElement; }
            set { formElement = value; }
        }

        public bool IsFragmentParser
        {
            get { return isFragmentParser; }
        }

        public bool IsScriptingEnabled
        {
            get { return isScriptingEnabled; }
            set { isScriptingEnabled = value; }
        }

        public bool IsIFrameSourceDoc
        {
            get { return isIFrameSourceDoc; }
            set { isIFrameSourceDoc = value; }
        }

        public ParserState State
        {
            get { return state; }
            set { state = value; }
        }

        public Token CurrentToken
        {
            get { return currentToken; }
            set { currentToken = value; }
        }

        public HtmlDocument Document
        {
            get { return doc; }
        }

        public void LogParseError(string errorDescription, string actionTaken)
        {
            parseErrors.Add(new ParseError(errorDescription, actionTaken, reader.LineNumber, reader.LinePosition));
        }

        public ReadOnlyCollection<XmlNode> ParseFragment(TextReader htmlSource, HtmlElement context)
        {
            reader = new HtmlTextReader(htmlSource);
            XmlNode parent = null;
            doc = new HtmlDocument(doc.Window, null);
            isFragmentParser = true;
            if (context != null)
            {
                contextElement = context;
                switch (context.Name)
                {
                    case HtmlElementFactory.TitleElementTagName:
                    case HtmlElementFactory.TextAreaElementTagName:
                        lexer = new Tokenizer(reader, InitialTokenizerState.RCData);
                        lexer.ParseError += new EventHandler<ParseErrorEventArgs>(lexer_ParseError);
                        break;

                    case HtmlElementFactory.StyleElementTagName:
                    case HtmlElementFactory.XmpElementTagName:
                    case HtmlElementFactory.IFrameElementTagName:
                    case HtmlElementFactory.NoEmbedElementTagName:
                    case HtmlElementFactory.NoFramesElementTagName:
                        lexer = new Tokenizer(reader, InitialTokenizerState.RawText);
                        lexer.ParseError += new EventHandler<ParseErrorEventArgs>(lexer_ParseError);
                        break;

                    case HtmlElementFactory.ScriptElementTagName:
                        lexer = new Tokenizer(reader, InitialTokenizerState.Script);
                        lexer.ParseError += new EventHandler<ParseErrorEventArgs>(lexer_ParseError);
                        break;

                    case HtmlElementFactory.NoScriptElementTagName:
                        if (isScriptingEnabled)
                        {
                            lexer = new Tokenizer(reader, InitialTokenizerState.RawText);
                            lexer.ParseError += new EventHandler<ParseErrorEventArgs>(lexer_ParseError);
                        }
                        else
                        {
                            lexer = new Tokenizer(reader, InitialTokenizerState.Data);
                            lexer.ParseError += new EventHandler<ParseErrorEventArgs>(lexer_ParseError);
                        }
                        break;

                    case HtmlElementFactory.PlainTextElementTagName:
                        lexer = new Tokenizer(reader, InitialTokenizerState.PlainText);
                        lexer.ParseError += new EventHandler<ParseErrorEventArgs>(lexer_ParseError);
                        break;

                    default:
                        lexer = new Tokenizer(reader, InitialTokenizerState.Data);
                        lexer.ParseError += new EventHandler<ParseErrorEventArgs>(lexer_ParseError);
                        break;
                }

                InsertElement(new TagToken(TokenType.StartTag, HtmlElementFactory.HtmlElementTagName));
                ResetInsertionMode();

                // TODO: walk the context for a form
                parent = doc.ChildNodes[0];
            }
            else
            {
                lexer = new Tokenizer(reader);
                lexer.ParseError += new EventHandler<ParseErrorEventArgs>(lexer_ParseError);
                parent = doc;
            }

            Execute();
            List<XmlNode> elementList = new List<XmlNode>();
            foreach (XmlNode node in parent.ChildNodes)
            {
                elementList.Add(node);
            }

            return elementList.AsReadOnly();
        }

        private void lexer_ParseError(object sender, ParseErrorEventArgs e)
        {
            parseErrors.Add(new ParseError(e.Description, e.ActionTaken, e.LineNumber, e.LinePosition));
        }

		public HtmlDocument ParseDocument(string htmlSource)
		{
			return ParseDocument(new StringReader(htmlSource ?? ""));
		}

    	public HtmlDocument ParseDocument(TextReader htmlSource)
        {
            reader = new HtmlTextReader(htmlSource);
            lexer = new Tokenizer(reader);
            lexer.ParseError += new EventHandler<ParseErrorEventArgs>(lexer_ParseError);
            Execute();
            return doc;
        }

        private void Execute()
        {
            parseErrors = new List<ParseError>();
            bool parsingComplete = false;
            while (!parsingComplete)
            {
                currentToken = lexer.GetNextToken();
                bool tokenParsed = state.ParseToken(this);
                while (!tokenParsed)
                {
                    tokenParsed = state.ParseToken(this);
                }

                parsingComplete = currentToken.TokenType == TokenType.EndOfFile;
            }

            while (openElementStack.Count > 0)
            {
                PopElementFromStack();
            }
        }

        public void AdvanceState(ParserState nextState)
        {
            state = nextState;
        }

        public void InsertElement(TagToken token)
        {
            InsertElement(token, false);
        }

        public void InsertElement(TagToken token, bool immediatelyPopElement)
        {
            InsertElement(token, immediatelyPopElement, false);
        }

        public void InsertElement(TagToken token, bool immediatelyPopElement, bool elementIsFosterParented)
        {
            InsertElement(token, immediatelyPopElement, elementIsFosterParented, ActiveFormattingElementListState.None);
        }

        public void InsertElement(TagToken token, bool immediatelyPopElement, bool elementIsFosterParented, ActiveFormattingElementListState addToActiveFormattingElementList)
        {
            InsertElement(token, immediatelyPopElement, elementIsFosterParented, addToActiveFormattingElementList, Parser.HtmlNamespace);
        }

        public void InsertElement(TagToken token, bool immediatelyPopElement, bool elementIsFosterParented, ActiveFormattingElementListState addToActiveFormattingElementList, string namespaceUri)
        {
            HtmlElement element = CreateElement(token, namespaceUri);

            IResettable elementAsResettable = element as IResettable;
            if (elementAsResettable != null)
            {
                elementAsResettable.Reset();
            }

            if (CurrentNode != null)
            {
                if (elementIsFosterParented)
                {
                    FosterParentElement(element);
                }
                else
                {
                    CurrentNode.AppendChild(element);
                }
            }
            else
            {
                doc.AppendChild(element);
            }

            IFormChild elementAsFormChild = element as IFormChild;
            if (elementAsFormChild != null)
            {
                elementAsFormChild.SetForm(formElement);
            }

            PushElementToStack(element);

            if (immediatelyPopElement)
            {
                PopElementFromStack();
            }

            lastInsertedNode = element;
            if (addToActiveFormattingElementList != ActiveFormattingElementListState.None)
            {
                if (addToActiveFormattingElementList == ActiveFormattingElementListState.AddAsMarker)
                {
                    element.IsFormattingScopeMarker = true;
                }

                activeFormattingElementList.Add(element);
            }
        }

        public HtmlElement CreateElement(TagToken token, string namespaceUri)
        {
            if (namespaceUri == HtmlNamespace)
            {
                namespaceUri = string.Empty;
            }

            HtmlElement element = doc.CreateElement(token.Name, namespaceUri) as HtmlElement;
            foreach (string attributeName in token.Attributes.Keys)
            {
                AddElementAtttribute(element, token.Attributes[attributeName]);
            }

            return element;
        }

        public void PushElementToStack(HtmlElement element)
        {
            openElementStack.Push(element);
        }

        public void PopElementFromStack()
        {
            openElementStack.Pop();
        }

        public void ActivateScriptState(TagToken token)
        {
            InsertElement(token, false);
            HtmlScriptElement scriptElement = CurrentNode as HtmlScriptElement;
            if (scriptElement != null)
            {
                scriptElement.ParserInserted = true;
                scriptElement.AlreadyStarted = IsFragmentParser;
            }

            lexer.SetInitialState(InitialTokenizerState.Script);
            AdvanceState(new TextState(state));
        }

        public void ActivateRCDataState(TagToken token, bool fosterParentingIsRequired)
        {
            InsertElement(token, false, fosterParentingIsRequired);
            lexer.SetInitialState(InitialTokenizerState.RCData);
            AdvanceState(new TextState(state));
        }

        public void ActivateRawTextState(TagToken token, bool fosterParentingIsRequired)
        {
            InsertElement(token, false, fosterParentingIsRequired);
            lexer.SetInitialState(InitialTokenizerState.RawText);
            AdvanceState(new TextState(state));
        }

        public void ActivatePlainTextState(TagToken token, bool fosterParentingIsRequired)
        {
            InsertElement(token, false, fosterParentingIsRequired);
            lexer.SetInitialState(InitialTokenizerState.PlainText);
        }

        public void AddElementAtttribute(HtmlElement element, TagTokenAttribute attribute)
        {
            if (string.IsNullOrEmpty(attribute.Namespace))
            {
                element.SetAttribute(attribute.Name, attribute.Value);
            }
            else
            {
                string prefix = string.Empty;
                string localName = attribute.Name;
                if (attribute.Name.Contains(":"))
                {
                    string[] attributeNameComponents = attribute.Name.Split(new char[] { ':' });
                    prefix = attributeNameComponents[0];
                    localName = attributeNameComponents[1];
                }
                
                element.SetAttribute(localName, attribute.Namespace, attribute.Value);
                XmlAttribute attributeNode = element.GetAttributeNode(localName, attribute.Namespace);
                attributeNode.Prefix = prefix;

                // XXX: Attributes in the XML namespace need to be at the end
                // of the attributes collection.
                List<XmlAttribute> xmlNamespaceAttributes = new List<XmlAttribute>();
                foreach (XmlAttribute attr in element.Attributes)
                {
                    if (attr.NamespaceURI == XmlNamespace)
                    {
                        xmlNamespaceAttributes.Add(attr);
                    }
                }

                foreach (XmlAttribute attr in xmlNamespaceAttributes)
                {
                    element.Attributes.Remove(attr);
                    element.Attributes.Append(attr);
                }
            }
        }

        public void CreateComment(string comment)
        {
            CreateComment(comment, CurrentNode);
        }

        public void CreateComment(string comment, HtmlElement element)
        {
            HtmlCommentNode commentNode = doc.createComment(comment) as HtmlCommentNode;
            if (openElementStack.Count == 0 || element == null)
            {
                doc.AppendChild(commentNode);
            }
            else
            {
                element.AppendChild(commentNode);
            }

            lastInsertedNode = commentNode;
        }

        public void InsertCharacterIntoNode(char character)
        {
            InsertCharacterIntoNode(character, false);
        }

        public void InsertCharacterIntoNode(char character, bool fosterParentCharacter)
        {
            HtmlElement parentElement = null;
            if (fosterParentCharacter)
            {
                parentElement = openElementStack.GetFosterParentElement();
            }
            else
            {
                parentElement = CurrentNode;
            }

            // When the [parsing rules] require the UA to insert a character into a node,
            // if that node has a child immediately before where the character is to 
            // be inserted, and that child is a Text node, and that Text node was the 
            // last node that the parser inserted into the document, then the character 
            // must be appended to that Text node; otherwise, a new Text node whose data 
            // is just that character must be inserted in the appropriate place.
            HtmlTextNode node = parentElement.LastChild as HtmlTextNode;
            if (node != null && node == lastInsertedNode)
            {
                node.Value += character;
            }
            else
            {
                HtmlTextNode textNode = doc.CreateTextNode(character.ToString()) as HtmlTextNode;
                if (fosterParentCharacter)
                {
                    parentElement.InsertBefore(textNode, openElementStack.GetLastTable());
                }
                else
                {
                    if (node != null)
                    {
                        parentElement.InsertAfter(textNode, node);
                    }
                    else
                    {
                        parentElement.AppendChild(textNode);
                    }
                }

                lastInsertedNode = textNode;
            }
        }

        public HtmlElement CurrentNode
        {
            get
            {
                HtmlElement element = null;
                if (openElementStack.Count > 0)
                {
                    element = openElementStack.Peek();
                }

                return element;
            }
        }

        public void GenerateImpliedEndTags(string tagToExclude)
        {
            // When the [...] UA [is required] to generate implied end tags, then, while the 
            // current node is a dd element, a dt element, an li element, an option element, 
            // an optgroup element, a p element, an rp element, or an rt element, the UA must 
            // pop the current node off the stack of open elements.
            //
            // If a step requires the UA to generate implied end tags but lists an element to 
            // exclude from the process, then the UA must perform the above steps as if that 
            // element was not in the above list.
            List<string> endTagNamesToGenerate = new List<string>();
            endTagNamesToGenerate.Add(HtmlElementFactory.DDElementTagName);
            endTagNamesToGenerate.Add(HtmlElementFactory.DTElementTagName);
            endTagNamesToGenerate.Add(HtmlElementFactory.LIElementTagName);
            endTagNamesToGenerate.Add(HtmlElementFactory.OptionElementTagName);
            endTagNamesToGenerate.Add(HtmlElementFactory.OptGroupElementTagName);
            endTagNamesToGenerate.Add(HtmlElementFactory.ParaElementTagName);
            endTagNamesToGenerate.Add(HtmlElementFactory.RpElementTagName);
            endTagNamesToGenerate.Add(HtmlElementFactory.RtElementTagName);

            if (!string.IsNullOrEmpty(tagToExclude) && endTagNamesToGenerate.Contains(tagToExclude))
            {
                endTagNamesToGenerate.Remove(tagToExclude);
            }

            while (endTagNamesToGenerate.Contains(CurrentNode.Name))
            {
                PopElementFromStack();
            }
        }

        public bool ReconstructActiveFormattingElements(bool fosterParentReconstructedElements)
        {
            // When the [...] UA [is required] to reconstruct the active formatting elements, 
            // the UA must perform the following steps:
            //
            // 1.If there are no entries in the list of active formatting elements, then 
            // there is nothing to reconstruct; stop this algorithm.
            //
            // 2.If the last (most recently added) entry in the list of active formatting 
            // elements is a marker, or if it is an element that is in the stack of open 
            // elements, then there is nothing to reconstruct; stop this algorithm.
            // 
            // 3.Let entry be the last (most recently added) element in the list of active
            // formatting elements.
            // 
            // 4.If there are no entries before entry in the list of active formatting
            // elements, then jump to step 8.
            // 
            // 5.Let entry be the entry one earlier than entry in the list of active 
            // formatting elements.
            // 
            // 6.If entry is neither a marker nor an element that is also in the stack of 
            // open elements, go to step 4.
            // 
            // 7.Let entry be the element one later than entry in the list of active 
            // formatting elements.
            // 
            // 8.Create an element for the token for which the element entry was created, 
            // to obtain new element.
            // 
            // 9.Append new element to the current node and push it onto the stack of open 
            // elements so that it is the new current node.
            // 
            // 10.Replace the entry for entry in the list with an entry for new element.
            // 
            // 11.If the entry for new element in the list of active formatting elements is 
            // not the last entry in the list, return to step 7.
            // 
            // This has the effect of reopening all the formatting elements that were opened 
            // in the current body, cell, or caption (whichever is youngest) that haven't been 
            // explicitly closed.
            bool elementsWereReconstructed = false;
            if (activeFormattingElementList.Count != 0)
            {
                int entryIndex = activeFormattingElementList.Count - 1;
                HtmlElement entry = activeFormattingElementList[entryIndex];
                if (!entry.IsFormattingScopeMarker && !openElementStack.Contains(entry))
                {
                    bool foundEntry = false;
                    while (entryIndex > 0 && !foundEntry)
                    {
                        entryIndex--;
                        entry = activeFormattingElementList[entryIndex];
                        if (entry.IsFormattingScopeMarker || openElementStack.Contains(entry))
                        {
                            foundEntry = true;
                        }
                    }

                    if (foundEntry)
                    {
                        entryIndex++;
                    }

                    while (entryIndex < activeFormattingElementList.Count)
                    {
                        entry = activeFormattingElementList[entryIndex];
                        TagToken newElementTag = new TagToken(TokenType.StartTag, entry.Name);
                        foreach (XmlAttribute attribute in entry.Attributes)
                        {
                            // newElementTag.Attributes.Add(attribute.Name, attribute.Value);
                            newElementTag.Attributes.Add(attribute.Name, new TagTokenAttribute(attribute.Name, attribute.Value));
                        }

                        InsertElement(newElementTag, false, fosterParentReconstructedElements);
                        activeFormattingElementList[entryIndex] = CurrentNode;
                        entryIndex++;
                    }

                    elementsWereReconstructed = true;
                }
            }

            return elementsWereReconstructed;
        }

        internal void FosterParentElement(HtmlElement elementToReparent)
        {
            HtmlElement lastTableElement = openElementStack.GetLastTable();
            HtmlElement fosterParentElement = openElementStack.GetFosterParentElement();
            if (elementToReparent.ParentNode != null)
            {
                elementToReparent.ParentNode.RemoveChild(elementToReparent);
            }

            if (fosterParentElement == lastTableElement.ParentNode)
            {
                fosterParentElement.InsertBefore(elementToReparent, lastTableElement);
            }
            else
            {
                fosterParentElement.AppendChild(elementToReparent);
            }
        }

        internal void ResetInsertionMode()
        {
            // 1.Let last be false.
            bool last = false;

            // 2.Let foreign be false.
            bool foreign = false;
            bool continueExecuting = true;

            HtmlElement node= null;
            int nodeIndex = 0;
            while (continueExecuting)
            {
                // 3.Let node be the last node in the stack of open elements.
                node = openElementStack[nodeIndex];

                // 4.Loop: If node is the first node in the stack of open elements, 
                // then set last to true and set node to the context element. (fragment case)
                if (nodeIndex == openElementStack.Count - 1)
                {
                    last = true;
                    node = contextElement;
                }

                if (node.Name == HtmlElementFactory.SelectElementTagName)
                {
                    // 5.If node is a select element, then switch the insertion mode to "in select" 
                    // and jump to the step labeled end. (fragment case)
                    AdvanceState(new InSelectState());
                    continueExecuting = false;
                }
                else if ((node.Name == HtmlElementFactory.TDElementTagName || node.Name == HtmlElementFactory.THElementTagName) && !last)
                {
                    // 6.If node is a td or th element and last is false, then switch the insertion 
                    // mode to "in cell" and jump to the step labeled end.
                    AdvanceState(new InCellState());
                    continueExecuting = false;
                }
                else if (node.Name == HtmlElementFactory.TRElementTagName)
                {
                    // 7.If node is a tr element, then switch the insertion mode to "in row" and 
                    // jump to the step labeled end.
                    AdvanceState(new InRowState());
                    continueExecuting = false;
                }
                else if (node.Name == HtmlElementFactory.TBodyElementTagName ||
                    node.Name == HtmlElementFactory.THeadElementTagName ||
                    node.Name == HtmlElementFactory.TFootElementTagName)
                {
                    // 8.If node is a tbody, thead, or tfoot element, then switch the insertion 
                    // mode to "in table body" and jump to the step labeled end.
                    AdvanceState(new InTableBodyState());
                    continueExecuting = false;
                }
                else if (node.Name == HtmlElementFactory.CaptionElementTagName)
                {
                    // 9.If node is a caption element, then switch the insertion mode to "in caption" 
                    // and jump to the step labeled end.
                    AdvanceState(new InCaptionState());
                }
                else if (node.Name == HtmlElementFactory.ColGroupElementTagName)
                {
                    // 10.If node is a colgroup element, then switch the insertion mode to "in column group" 
                    // and jump to the step labeled end. (fragment case)
                    AdvanceState(new InColumnGroupState());
                    continueExecuting = false;
                }
                else if (node.Name == HtmlElementFactory.TableElementTagName)
                {
                    // 11.If node is a table element, then switch the insertion mode to "in table" and 
                    // jump to the step labeled end.
                    AdvanceState(new InTableState());
                    continueExecuting = false;
                }
                else if (node.Name == HtmlElementFactory.HeadElementTagName)
                {
                    // 12.If node is a head element, then switch the insertion mode to "in body" 
                    // ("in body"! not "in head"!) and jump to the step labeled end. (fragment case)
                    AdvanceState(new InBodyState());
                    continueExecuting = false;
                }
                else if (node.Name == HtmlElementFactory.BodyElementTagName)
                {
                    // 13.If node is a body element, then switch the insertion mode to "in body" and jump 
                    // to the step labeled end.
                    AdvanceState(new InBodyState());
                    continueExecuting = false;
                }
                else if (node.Name == HtmlElementFactory.FramesetElementTagName)
                {
                    // 14.If node is a frameset element, then switch the insertion mode to "in frameset" 
                    // and jump to the step labeled end. (fragment case)
                    AdvanceState(new InFramesetState());
                    continueExecuting = false;
                }
                else if (node.Name == HtmlElementFactory.HtmlElementTagName)
                {
                    // 15.If node is an html element, then switch the insertion mode to "before head" 
                    // Then, jump to the step labeled end. (fragment case)
                    AdvanceState(new BeforeHeadState());
                    continueExecuting = false;
                }
                
                if (continueExecuting && 
                    (node.NamespaceURI == MathMLNamespace ||
                    node.NamespaceURI == SvgNamespace))
                {
                    // 16.If node is an element from the MathML namespace or the SVG namespace, then set
                    // the foreign flag to true.
                    foreign = true;
                }

                if (continueExecuting && last)
                {
                    // 17.If last is true, then switch the insertion mode to "in body" and jump to the 
                    // step labeled end. (fragment case)
                    AdvanceState(new InBodyState());
                    continueExecuting = false;
                }

                // 18.Let node now be the node before node in the stack of open elements.
                nodeIndex++;

                // 19.Return to the step labeled loop.
            }

            // 20.End: If foreign is true, switch the secondary insertion mode to whatever the 
            // insertion mode is set to, and switch the insertion mode to "in foreign content".
            if (foreign)
            {
                AdvanceState(new InForeignContentState(state));
            }
        }

        internal void IgnoreNextLineFeedToken(TagToken tag, bool activateTextState, bool isFosterParentingRequired)
        {
            bool nextTokenIsLineFeed = lexer.IsNextTokenLineFeed();
            isFramesetOK = false;
            if (activateTextState)
            {
                ActivateRCDataState(tag, isFosterParentingRequired);
            }
            else
            {
                InsertElement(tag, false, isFosterParentingRequired);
            }

            if (nextTokenIsLineFeed)
            {
                lexer.GetNextToken();
            }
        }
    }
}
