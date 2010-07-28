using System;
using System.Collections.Generic;
using XBrowserProject.BrowserModel.Internal;
using XBrowserProject.BrowserModel.Internal.HtmlDom;
using XBrowserProject.HtmlDom;
using XBrowserProject.HtmlParser.HtmlTokenizer;

namespace XBrowserProject.HtmlParser.States
{
    public class InForeignContentState : ParserState
    {
        private static Dictionary<string, string> mathMLAttributes;
        private static Dictionary<string, string> svgAttributes;
        private static Dictionary<string, string> svgElementNames;
        private static Dictionary<string, string> foreignAttributes;

        ParserState secondaryInsertionMode;

        public InForeignContentState(ParserState secondaryMode)
        {
            secondaryInsertionMode = secondaryMode;
        }

        public override string Description
        {
            get { return "in foreign content"; }
        }

        private static Dictionary<string, string> ForeignAttributes
        {
            get
            {
                if (foreignAttributes == null)
                {
                    foreignAttributes = new Dictionary<string, string>();
                    foreignAttributes.Add("xlink:actuate", Parser.XLinkNamespace);
                    foreignAttributes.Add("xlink:arcrole", Parser.XLinkNamespace);
                    foreignAttributes.Add("xlink:href", Parser.XLinkNamespace);
                    foreignAttributes.Add("xlink:role", Parser.XLinkNamespace);
                    foreignAttributes.Add("xlink:show", Parser.XLinkNamespace);
                    foreignAttributes.Add("xlink:title", Parser.XLinkNamespace);
                    foreignAttributes.Add("xlink:type", Parser.XLinkNamespace);
                    foreignAttributes.Add("xml:base", Parser.XmlNamespace);
                    foreignAttributes.Add("xml:lang", Parser.XmlNamespace);
                    foreignAttributes.Add("xml:space", Parser.XmlNamespace);
                    foreignAttributes.Add("xmlns", Parser.XmlNsNamespace);
                    foreignAttributes.Add("xmlns:xlink", Parser.XmlNsNamespace);
                }

                return foreignAttributes;
            }
        }

        private static Dictionary<string, string> MathMLAttributes
        {
            get
            {
                if (mathMLAttributes == null)
                {
                    mathMLAttributes = new Dictionary<string, string>();
                    mathMLAttributes.Add("definitionurl", "definitionUrl");
                }

                return mathMLAttributes;
            }
        }

        private static Dictionary<string, string> SvgElements
        {
            get
            {
                if (svgElementNames == null)
                {
                    svgElementNames = new Dictionary<string, string>();
                    svgElementNames.Add("altglyph", "altGlyph");
                    svgElementNames.Add("altglyphdef", "altGlyphDef");
                    svgElementNames.Add("altglyphitem", "altGlyphItem");
                    svgElementNames.Add("animatecolor", "animateColor");
                    svgElementNames.Add("animatemotion", "animateMotion");
                    svgElementNames.Add("animatetransform", "animateTransform");
                    svgElementNames.Add("clippath", "clipPath");
                    svgElementNames.Add("feblend", "feBlend");
                    svgElementNames.Add("fecolormatrix", "feColorMatrix");
                    svgElementNames.Add("fecomponenttransfer", "feComponentTransfer");
                    svgElementNames.Add("fecomposite", "feComposite");
                    svgElementNames.Add("feconvolvematrix", "feConvolveMatrix");
                    svgElementNames.Add("fediffuselighting", "feDiffuseLighting");
                    svgElementNames.Add("fedisplacementmap", "feDisplacementMap");
                    svgElementNames.Add("fedistantlight", "feDistantLight");
                    svgElementNames.Add("feflood", "feFlood");
                    svgElementNames.Add("fefunca", "feFuncA");
                    svgElementNames.Add("fefuncb", "feFuncB");
                    svgElementNames.Add("fefuncg", "feFuncG");
                    svgElementNames.Add("fefuncr", "feFuncR");
                    svgElementNames.Add("fegaussianblur", "feGaussianBlur");
                    svgElementNames.Add("feimage", "feImage");
                    svgElementNames.Add("femerge", "feMerge");
                    svgElementNames.Add("femergenode", "feMergeNode");
                    svgElementNames.Add("femorphology", "feMorphology");
                    svgElementNames.Add("feoffset", "feOffset");
                    svgElementNames.Add("fepointlight", "fePointLight");
                    svgElementNames.Add("fespecularlighting", "feSpecularLighting");
                    svgElementNames.Add("fespotlight", "feSpotLight");
                    svgElementNames.Add("fetile", "feTile");
                    svgElementNames.Add("feturbulence", "feTurbulence");
                    svgElementNames.Add("foreignobject", "foreignObject");
                    svgElementNames.Add("glyphref", "glyphRef");
                    svgElementNames.Add("lineargradient", "linearGradient");
                    svgElementNames.Add("radialgradient", "radialGradient");
                    svgElementNames.Add("textpath", "textPath");
                }

                return svgElementNames;
            }
        }

        private static Dictionary<string, string> SvgAttributes
        {
            get
            {
                if (svgAttributes == null)
                {
                    svgAttributes = new Dictionary<string, string>();
                    svgAttributes.Add("attributename", "attributeName");
                    svgAttributes.Add("attributetype", "attributeType");
                    svgAttributes.Add("basefrequency", "baseFrequency");
                    svgAttributes.Add("baseprofile", "baseProfile");
                    svgAttributes.Add("calcmode", "calcMode");
                    svgAttributes.Add("clippathunits", "clipPathUnits");
                    svgAttributes.Add("contentscripttype", "contentScriptType");
                    svgAttributes.Add("contentstyletype", "contentStyleType");
                    svgAttributes.Add("diffuseconstant", "diffuseConstant");
                    svgAttributes.Add("edgemode", "edgeMode");
                    svgAttributes.Add("externalresourcesrequired", "externalResourcesRequired");
                    svgAttributes.Add("filterres", "filterRes");
                    svgAttributes.Add("filterunits", "filterUnits");
                    svgAttributes.Add("glyphref", "glyphRef");
                    svgAttributes.Add("gradienttransform", "gradientTransform");
                    svgAttributes.Add("gradientunits", "gradientUnits");
                    svgAttributes.Add("kernelmatrix", "kernelMatrix");
                    svgAttributes.Add("kernelunitlength", "kernelUnitLength");
                    svgAttributes.Add("keypoints", "keyPoints");
                    svgAttributes.Add("keysplines", "keySplines");
                    svgAttributes.Add("keytimes", "keyTimes");
                    svgAttributes.Add("lengthadjust", "lengthAdjust");
                    svgAttributes.Add("limitingconeangle", "limitingConeAngle");
                    svgAttributes.Add("markerheight", "markerHeight");
                    svgAttributes.Add("markerunits", "markerUnits");
                    svgAttributes.Add("markerwidth", "markerWidth");
                    svgAttributes.Add("maskcontentunits", "maskContentUnits");
                    svgAttributes.Add("maskunits", "maskUnits");
                    svgAttributes.Add("numoctaves", "numOctaves");
                    svgAttributes.Add("pathlength", "pathLength");
                    svgAttributes.Add("patterncontentunits", "patternContentUnits");
                    svgAttributes.Add("patterntransform", "patternTransform");
                    svgAttributes.Add("patternunits", "patternUnits");
                    svgAttributes.Add("pointsatx", "pointsAtX");
                    svgAttributes.Add("pointsaty", "pointsAtY");
                    svgAttributes.Add("pointsatz", "pointsAtZ");
                    svgAttributes.Add("preservealpha", "preserveAlpha");
                    svgAttributes.Add("preserveaspectratio", "preserveAspectRatio");
                    svgAttributes.Add("primitiveunits", "primitiveUnits");
                    svgAttributes.Add("refx", "refX");
                    svgAttributes.Add("refy", "refY");
                    svgAttributes.Add("repeatcount", "repeatCount");
                    svgAttributes.Add("repeatdur", "repeatDur");
                    svgAttributes.Add("requiredextensions", "requiredExtensions");
                    svgAttributes.Add("requiredfeatures", "requiredFeatures");
                    svgAttributes.Add("specularconstant", "specularConstant");
                    svgAttributes.Add("specularexponent", "specularExponent");
                    svgAttributes.Add("spreadmethod", "spreadMethod");
                    svgAttributes.Add("startoffset", "startOffset");
                    svgAttributes.Add("stddeviation", "stdDeviation");
                    svgAttributes.Add("stitchtiles", "stitchTiles");
                    svgAttributes.Add("surfacescale", "surfaceScale");
                    svgAttributes.Add("systemlanguage", "systemLanguage");
                    svgAttributes.Add("tablevalues", "tableValues");
                    svgAttributes.Add("targetx", "targetX");
                    svgAttributes.Add("targety", "targetY");
                    svgAttributes.Add("textlength", "textLength");
                    svgAttributes.Add("viewbox", "viewBox");
                    svgAttributes.Add("viewtarget", "viewTarget");
                    svgAttributes.Add("xchannelselector", "xChannelSelector");
                    svgAttributes.Add("ychannelselector", "yChannelSelector");
                    svgAttributes.Add("zoomandpan", "zoomAndPan");
                    svgAttributes.Add("", "");
                }

                return svgAttributes;
            }
        }

        protected override bool ProcessEndOfFileToken(Parser parser)
        {
            return ProcessHtmlOnlyToken(parser);
        }

        protected override bool ProcessCharacterToken(CharacterToken character, Parser parser)
        {
            if (HtmlCharacterUtilities.IsWhiteSpace(character.Data))
            {
                parser.IsFramesetOK = false;
            }

            parser.InsertCharacterIntoNode(character.Data);
            return true;
        }

        protected override bool ProcessStartTagToken(TagToken tag, Parser parser)
        {
            bool tokenProcessed = false;
            if (StartTagShouldBeProcessedUsingSecondaryInsertionMode(tag, parser))
            {
                tokenProcessed = ProcessUsingSecondaryInsertionMode(parser);
            }
            else if (tag.Name == HtmlElementFactory.BElementTagName ||
                tag.Name == HtmlElementFactory.BigElementTagName ||
                tag.Name == HtmlElementFactory.BlockQuoteElementTagName ||
                tag.Name == HtmlElementFactory.BodyElementTagName ||
                tag.Name == HtmlElementFactory.BRElementTagName ||
                tag.Name == HtmlElementFactory.CenterElementTagName ||
                tag.Name == HtmlElementFactory.CodeElementTagName ||
                tag.Name == HtmlElementFactory.DDElementTagName ||
                tag.Name == HtmlElementFactory.DivElementTagName ||
                tag.Name == HtmlElementFactory.DLElementTagName ||
                tag.Name == HtmlElementFactory.DTElementTagName ||
                tag.Name == HtmlElementFactory.EmElementTagName ||
                tag.Name == HtmlElementFactory.EmbedElementTagName ||
                tag.Name == HtmlElementFactory.H1ElementTagName ||
                tag.Name == HtmlElementFactory.H2ElementTagName ||
                tag.Name == HtmlElementFactory.H3ElementTagName ||
                tag.Name == HtmlElementFactory.H4ElementTagName ||
                tag.Name == HtmlElementFactory.H5ElementTagName ||
                tag.Name == HtmlElementFactory.H6ElementTagName ||
                tag.Name == HtmlElementFactory.HeadElementTagName ||
                tag.Name == HtmlElementFactory.HrElementTagName ||
                tag.Name == HtmlElementFactory.IElementTagName ||
                tag.Name == HtmlElementFactory.ImageElementTagName ||
                tag.Name == HtmlElementFactory.LIElementTagName ||
                tag.Name == HtmlElementFactory.ListingElementTagName ||
                tag.Name == HtmlElementFactory.MenuElementTagName ||
                tag.Name == HtmlElementFactory.MetaElementTagName ||
                tag.Name == HtmlElementFactory.NoBrElementTagName ||
                tag.Name == HtmlElementFactory.OLElementTagName ||
                tag.Name == HtmlElementFactory.ParaElementTagName ||
                tag.Name == HtmlElementFactory.PreElementTagName ||
                tag.Name == HtmlElementFactory.RubyElementTagName ||
                tag.Name == HtmlElementFactory.SElementTagName ||
                tag.Name == HtmlElementFactory.SmallElementTagName ||
                tag.Name == HtmlElementFactory.SpanElementTagName ||
                tag.Name == HtmlElementFactory.StrongElementTagName ||
                tag.Name == HtmlElementFactory.StrikeElementTagName ||
                tag.Name == HtmlElementFactory.SubElementTagName ||
                tag.Name == HtmlElementFactory.SupElementTagName ||
                tag.Name == HtmlElementFactory.TableElementTagName ||
                tag.Name == HtmlElementFactory.TTElementTagName ||
                tag.Name == HtmlElementFactory.UElementTagName ||
                tag.Name == HtmlElementFactory.ULElementTagName ||
                tag.Name == HtmlElementFactory.VarElementTagName ||
                (tag.Name == HtmlElementFactory.FontElementTagName && (tag.Attributes.ContainsKey("color") || tag.Attributes.ContainsKey("face") || tag.Attributes.ContainsKey("size"))))
            {
                parser.LogParseError("Unexpected start tag '" + tag.Name + "' in foreign object state", "popping elements and reprocessing");
                tokenProcessed = ProcessHtmlOnlyToken(parser);
            }
            else
            {
                if (parser.CurrentNode.NamespaceURI == Parser.MathMLNamespace)
                {
                    AdjustMathMLAttributes(tag);
                }
                else if (parser.CurrentNode.NamespaceURI == Parser.SvgNamespace)
                {
                    AdjustSvgElementName(tag);
                    AdjustSvgAttributes(tag);
                }

                AdjustForeignAttributes(tag);
                parser.InsertElement(tag, tag.IsSelfClosing, false, ActiveFormattingElementListState.None, parser.CurrentNode.NamespaceURI);
                tokenProcessed = true;
            }
            return tokenProcessed;
        }

        private bool ProcessHtmlOnlyToken(Parser parser)
        {
            bool foreignElementOnStack = false;
            foreach (HtmlElement element in parser.OpenElementStack)
            {
                if (element.Name == HtmlElementFactory.SvgElementTagName || element.Name == HtmlElementFactory.MathElementTagName)
                {
                    foreignElementOnStack = true;
                    break;
                }
            }

            if (foreignElementOnStack)
            {
                while (parser.CurrentNode.Name != HtmlElementFactory.SvgElementTagName && parser.CurrentNode.Name != HtmlElementFactory.MathElementTagName)
                {
                    parser.PopElementFromStack();
                }

                parser.PopElementFromStack();
            }

            parser.AdvanceState(secondaryInsertionMode);
            bool tokenProcessed = parser.State.ParseToken(parser);
            return tokenProcessed;
        }

        private bool ProcessUsingSecondaryInsertionMode(Parser parser)
        {
            // Process the token using the rules for the secondary insertion mode.
            // If, after doing so, the insertion mode is still "in foreign content", but 
            // there is no element in scope that has a namespace other than the HTML namespace, 
            // switch the insertion mode to the secondary insertion mode.
            bool tokenProcessed = secondaryInsertionMode.ParseToken(parser);
            if (parser.State.GetType() == this.GetType())
            {
                bool isElementInScopeWithNonHtmlNamespace = false;
                foreach (HtmlElement element in parser.OpenElementStack)
                {
                    if (parser.OpenElementStack.HasElementInScope(element) && (!string.IsNullOrEmpty(element.NamespaceURI) && element.NamespaceURI != Parser.HtmlNamespace))
                    {
                        isElementInScopeWithNonHtmlNamespace = true;
                        break;
                    }
                }

                if (!isElementInScopeWithNonHtmlNamespace)
                {
                    parser.AdvanceState(secondaryInsertionMode);
                }
            }
            return tokenProcessed;
        }

        public static void AdjustForeignAttributes(TagToken tag)
        {
            foreach (string attributeName in tag.Attributes.Keys)
            {
                if (ForeignAttributes.ContainsKey(attributeName))
                {
                    tag.Attributes[attributeName].Namespace = ForeignAttributes[attributeName];
                }
            }
        }

        private void AdjustSvgElementName(TagToken tag)
        {
            if (SvgElements.ContainsKey(tag.Name))
            {
                tag.Name = SvgElements[tag.Name];
            }
        }

        public static void AdjustSvgAttributes(TagToken tag)
        {
            List<string> originalAttributeNames = new List<string>(tag.Attributes.Keys);
            foreach (string attributeName in originalAttributeNames)
            {
                if (SvgAttributes.ContainsKey(attributeName))
                {
                    string fixedAttributeName = SvgAttributes[attributeName];
                    TagTokenAttribute attribute = tag.Attributes[attributeName];
                    attribute.Name = fixedAttributeName;
                    tag.Attributes.Remove(attributeName);
                    tag.Attributes.Add(fixedAttributeName, attribute);
                }
            }
        }

        public static void AdjustMathMLAttributes(TagToken tag)
        {
            foreach (string attributeName in tag.Attributes.Keys)
            {
                if (MathMLAttributes.ContainsKey(attributeName))
                {
                    string fixedAttributeName = MathMLAttributes[attributeName];
                    TagTokenAttribute attribute = tag.Attributes[attributeName];
                    attribute.Name = fixedAttributeName;
                    tag.Attributes.Remove(attributeName);
                    tag.Attributes.Add(fixedAttributeName, attribute);
                }
            }
        }

        protected override bool ProcessEndTagToken(TagToken tag, Parser parser)
        {
            bool tokenProcessed = false;
            if (parser.CurrentNode.Name == HtmlElementFactory.ScriptElementTagName && parser.CurrentNode.NamespaceURI == Parser.SvgNamespace)
            {
                // TODO: handle scripting case.
                // An end tag whose tag name is "script", if the current node is a script element in the SVG namespace.
                // Pop the current node off the stack of open elements.
                // Let the old insertion point have the same value as the current insertion point. Let the insertion 
                // point be just before the next input character.
                // Increment the parser's script nesting level by one. Set the parser pause flag to true.
                // Process the script element according to the SVG rules, if the user agent supports SVG. [SVG]
                // Note: Even if this causes new characters to be inserted into the tokenizer, the parser will not 
                // be executed reentrantly, since the parser pause flag is true.
                // Decrement the parser's script nesting level by one. If the parser's script nesting level is zero,
                // then set the parser pause flag to false.
                // Let the insertion point have the value of the old insertion point. (In other words, restore the 
                // insertion point to its previous value. This value might be the "undefined" value.)
                tokenProcessed = true;
            }
            else if (!string.IsNullOrEmpty(parser.CurrentNode.NamespaceURI) && parser.CurrentNode.NamespaceURI != Parser.HtmlNamespace)
            {
                // An end tag, if the current node is not an element in the HTML namespace.
                // Run these steps:
                // 1.Initialize node to be the current node (the bottommost node of the stack).
                // 2.If node is not an element with the same tag name as the token, then this is a parse error.
                // 3.Loop: If node has the same tag name as the token, pop elements from the stack of open elements
                // until node has been popped from the stack, and then abort these steps.
                // 4.Set node to the previous entry in the stack of open elements.
                // 5.If node is an element in the HTML namespace, process the token using the rules for the secondary
                // insertion mode. If, after doing so, the insertion mode is still "in foreign content", but there is
                // no element in scope that has a namespace other than the HTML namespace, switch the insertion mode 
                // to the secondary insertion mode.
                // 6.Return to the step labeled loop.
                int nodeIndex = 0;
                HtmlElement node = parser.OpenElementStack[nodeIndex];
                if (string.Compare(node.Name, tag.Name, true) != 0)
                {
                    parser.LogParseError("Tag name '" + tag.Name + "' and stack element name '" + node.Name + "' do not match", "none");
                }

                bool continueLoop = true;
                while (continueLoop)
                {
                    if (string.Compare(node.Name, tag.Name, true) == 0)
                    {
                        while (parser.OpenElementStack.Contains(node))
                        {
                            parser.PopElementFromStack();
                        }

                        continueLoop = false;
                        break;
                    }

                    nodeIndex++;
                    if (nodeIndex < parser.OpenElementStack.Count)
                    {
                        node = parser.OpenElementStack[nodeIndex];
                        if (string.IsNullOrEmpty(node.NamespaceURI) ||node.NamespaceURI == Parser.HtmlNamespace)
                        {
                            ProcessUsingSecondaryInsertionMode(parser);
                        }
                    }
                    else
                    {
                        continueLoop = false;
                    }
                }

                tokenProcessed = true;
            }
            else
            {
                // Any other end tag
                tokenProcessed = ProcessUsingSecondaryInsertionMode(parser);
            }

            return tokenProcessed;
        }

        protected override bool ProcessUnprocessedToken(Parser parser)
        {
            throw new NotImplementedException();
        }

        private bool StartTagShouldBeProcessedUsingSecondaryInsertionMode(TagToken tag, Parser parser)
        {
            bool useSecondaryMode = false;
            if (parser.CurrentNode.NamespaceURI == Parser.MathMLNamespace)
            {
                if ((parser.CurrentNode.Name == "mi" ||
                    parser.CurrentNode.Name == "mo" ||
                    parser.CurrentNode.Name == "mn" ||
                    parser.CurrentNode.Name == "ms" ||
                    parser.CurrentNode.Name == "mtext") &&
                    tag.Name != "mglyph" && tag.Name != "malignmark")
                {
                    useSecondaryMode = true;
                }
                else if (parser.CurrentNode.Name == "annotation-xml" && tag.Name == HtmlElementFactory.SvgElementTagName)
                {
                    useSecondaryMode = true;
                }
            }
            else if (parser.CurrentNode.NamespaceURI == Parser.SvgNamespace)
            {
                if (parser.CurrentNode.Name == "foreignObject" ||
                    parser.CurrentNode.Name == "desc" ||
                    parser.CurrentNode.Name == "title")
                    useSecondaryMode = true;
            }
            else if (string.IsNullOrEmpty(parser.CurrentNode.NamespaceURI) || parser.CurrentNode.NamespaceURI == Parser.HtmlNamespace)
            {
                useSecondaryMode = true;
            }

            return useSecondaryMode;
        }
    }
}
