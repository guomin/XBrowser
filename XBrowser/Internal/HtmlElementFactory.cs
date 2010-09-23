using XBrowserProject.Internal.Html.DOM;

namespace XBrowserProject.Internal
{
    internal class HtmlElementFactory
    {
        #region HTML pre-4.01 element names
        public const string BgSoundElementTagName = "bgsound";
        public const string ImageElementTagName = "image";
        public const string ListingElementTagName = "listing";
        public const string MarqueeElementTagName = "marquee";
        public const string NoBrElementTagName = "nobr";
        public const string NoEmbedElementTagName = "noembed";
        public const string PlainTextElementTagName = "plaintext";
        public const string WbrElementTagName = "wbr";
        public const string XmpElementTagName = "xmp";
        #endregion

        #region HTML 4.01 element names
        public const string AnchorElementTagName = "a";
        public const string AbbrElementTagName = "abbr";
        public const string AcronymElementTagName = "acronym";
        public const string AddressElementTagName = "address";
        public const string AppletElementTagName = "applet";
        public const string AreaElementTagName = "area";
        public const string BElementTagName = "b";
        public const string BaseElementTagName = "base";
        public const string BaseFontElementTagName = "basefont";
        public const string BdoElementTagName = "bdo";
        public const string BigElementTagName = "big";
        public const string BlockQuoteElementTagName = "blockquote";
        public const string BodyElementTagName = "body";
        public const string BRElementTagName = "br";
        public const string ButtonElementTagName = "button";
        public const string CaptionElementTagName = "caption";
        public const string CenterElementTagName = "center";
        public const string CiteElementTagName = "cite";
        public const string CodeElementTagName = "code";
        public const string ColElementTagName = "col";
        public const string ColGroupElementTagName = "colgroup";
        public const string DDElementTagName = "dd";
        public const string DelElementTagName = "del";
        public const string DfnElementTagName = "dfn";
        public const string DirElementTagName = "dir";
        public const string DivElementTagName = "div";
        public const string DLElementTagName = "dl";
        public const string DTElementTagName = "dt";
        public const string EmElementTagName = "em";
        public const string FieldsetElementTagName = "fieldset";
        public const string FontElementTagName = "font";
        public const string FormElementTagName = "form";
        public const string FrameElementTagName = "frame";
        public const string FramesetElementTagName = "frameset";
        public const string H1ElementTagName = "h1";
        public const string H2ElementTagName = "h2";
        public const string H3ElementTagName = "h3";
        public const string H4ElementTagName = "h4";
        public const string H5ElementTagName = "h5";
        public const string H6ElementTagName = "h6";
        public const string HeadElementTagName = "head";
        public const string HrElementTagName = "hr";
        public const string HtmlElementTagName = "html";
        public const string IElementTagName = "i";
        public const string IFrameElementTagName = "iframe";
        public const string ImgElementTagName = "img";
        public const string InputElementTagName = "input";
        public const string InsElementTagName = "ins";
        public const string IsIndexElementTagName = "isindex";
        public const string KbdElementTagName = "kbd";
        public const string LabelElementTagName = "label";
        public const string LegendElementTagName = "legend";
        public const string LIElementTagName = "li";
        public const string LinkElementTagName = "link";
        public const string MapElementTagName = "map";
        public const string MenuElementTagName = "menu";
        public const string MetaElementTagName = "meta";
        public const string NoFramesElementTagName = "noframes";
        public const string NoScriptElementTagName = "noscript";
        public const string ObjectElementTagName = "object";
        public const string OLElementTagName = "ol";
        public const string OptGroupElementTagName = "optgroup";
        public const string OptionElementTagName = "option";
        public const string ParaElementTagName = "p";
        public const string ParamElementTagName = "param";
        public const string PreElementTagName = "pre";
        public const string QElementTagName = "q";
        public const string SElementTagName = "s";
        public const string SampElementTagName = "samp";
        public const string ScriptElementTagName = "script";
        public const string SelectElementTagName = "select";
        public const string SmallElementTagName = "small";
        public const string SpanElementTagName = "span";
        public const string StrikeElementTagName = "strike";
        public const string StrongElementTagName = "strong";
        public const string StyleElementTagName = "style";
        public const string SubElementTagName = "sub";
        public const string SupElementTagName = "sup";
        public const string TableElementTagName = "table";
        public const string TBodyElementTagName = "tbody";
        public const string TDElementTagName = "td";
        public const string TextAreaElementTagName = "textarea";
        public const string TFootElementTagName = "tfoot";
        public const string THElementTagName = "th";
        public const string THeadElementTagName = "thead";
        public const string TitleElementTagName = "title";
        public const string TRElementTagName = "tr";
        public const string TTElementTagName = "tt";
        public const string UElementTagName = "u";
        public const string ULElementTagName = "ul";
        public const string VarElementTagName = "var";
        #endregion

        #region HTML 5 element names
        public const string ArticleElementTagName = "article";
        public const string AsideElementTagName = "aside";
        public const string AudioElementTagName = "audio";
        public const string CanvasElementTagName = "canvas";
        public const string CommandElementTagName = "command";
        public const string DataListElementTagName = "datalist";
        public const string DetailsElementTagName = "details";
        public const string EmbedElementTagName = "embed";
        public const string FigCaptionElementTagName = "figcaption";
        public const string FigureElementTagName = "figure";
        public const string FooterElementTagName = "footer";
        public const string HeaderElementTagName = "header";
        public const string HGroupElementTagName = "hgroup";
        public const string KeyGenElementTagName = "keygen";
        public const string MarkElementTagName = "mark";
        public const string MeterElementTagName = "meter";
        public const string NavElementTagName = "nav";
        public const string ProgressElementTagName = "progress";
        public const string RpElementTagName = "rp";
        public const string RtElementTagName = "rt";
        public const string RubyElementTagName = "ruby";
        public const string SectionElementTagName = "section";
        public const string SourceElementTagName = "source";
        public const string SummaryElementTagName = "summary";
        public const string TimeElementTagName = "time";
        public const string VideoElementTagName = "video";
        #endregion

        #region HTML 5 foreign content element names
        public const string SvgElementTagName = "svg";
        public const string MathElementTagName = "math";
        #endregion

        public static HtmlElement CreateElement(string prefix, string localName, string namespaceUri, HtmlDocument doc)
        {
            HtmlElement elementToReturn = null;

            switch (localName)
            {
                case AnchorElementTagName:
                    elementToReturn = new HtmlAnchorElement(prefix, localName, namespaceUri, doc);
                    break;

                case AppletElementTagName:
                    elementToReturn = new HtmlAppletElement(prefix, localName, namespaceUri, doc);
                    break;

                case AreaElementTagName:
                    elementToReturn = new HtmlAreaElement(prefix, localName, namespaceUri, doc);
                    break;

                case BaseElementTagName:
                    elementToReturn = new HtmlBaseElement(prefix, localName, namespaceUri, doc);
                    break;

                case BaseFontElementTagName:
                    elementToReturn = new HtmlBaseFontElement(prefix, localName, namespaceUri, doc);
                    break;

                case BlockQuoteElementTagName:
                    elementToReturn = new HtmlQuoteElement(prefix, localName, namespaceUri, doc);
                    break;

                case BodyElementTagName:
                    elementToReturn = new HtmlBodyElement(prefix, localName, namespaceUri, doc);
                    break;

                case BRElementTagName:
                    elementToReturn = new HtmlBRElement(prefix, localName, namespaceUri, doc);
                    break;

                case ButtonElementTagName:
                    elementToReturn = new HtmlButtonElement(prefix, localName, namespaceUri, doc);
                    break;

                case CaptionElementTagName:
                    elementToReturn = new HtmlTableCaptionElement(prefix, localName, namespaceUri, doc);
                    break;

                case ColElementTagName:
                    elementToReturn = new HtmlTableSectionElement(prefix, localName, namespaceUri, doc);
                    break;

                case ColGroupElementTagName:
                    elementToReturn = new HtmlTableColElement(prefix, localName, namespaceUri, doc);
                    break;

                case DelElementTagName:
                case InsElementTagName:
                    elementToReturn = new HtmlModElement(prefix, localName, namespaceUri, doc);
                    break;

                case DirElementTagName:
                    elementToReturn = new HtmlDirectoryElement(prefix, localName, namespaceUri, doc);
                    break;

                case DivElementTagName:
                    elementToReturn = new HtmlDivElement(prefix, localName, namespaceUri, doc);
                    break;

                case DLElementTagName:
                    elementToReturn = new HtmlDListElement(prefix, localName, namespaceUri, doc);
                    break;

                case FieldsetElementTagName:
                    elementToReturn = new HtmlFieldSetElement(prefix, localName, namespaceUri, doc);
                    break;

                case FormElementTagName:
                    elementToReturn = new HtmlFormElement(prefix, localName, namespaceUri, doc);
                    break;

                case FrameElementTagName:
                    elementToReturn = new HtmlFrameElement(prefix, localName, namespaceUri, doc);
                    break;

                case FramesetElementTagName:
                    elementToReturn = new HtmlFrameSetElement(prefix, localName, namespaceUri, doc);
                    break;

                case H1ElementTagName:
                case H2ElementTagName:
                case H3ElementTagName:
                case H4ElementTagName:
                case H5ElementTagName:
                case H6ElementTagName:
                    elementToReturn = new HtmlHeadingElement(prefix, localName, namespaceUri, doc);
                    break;

                case HeadElementTagName:
                    elementToReturn = new HtmlHeadElement(prefix, localName, namespaceUri, doc);
                    break;

                case HrElementTagName:
                    elementToReturn = new HtmlHRElement(prefix, localName, namespaceUri, doc);
                    break;

                case HtmlElementTagName:
                    elementToReturn = new HtmlHtmlElement(prefix, localName, namespaceUri, doc);
                    break;

                case IFrameElementTagName:
                    elementToReturn = new HtmlIframeElement(prefix, localName, namespaceUri, doc);
                    break;

                case ImgElementTagName:
                    elementToReturn = new HtmlImageElement(prefix, localName, namespaceUri, doc);
                    break;

                case IsIndexElementTagName:
                    elementToReturn = new HtmlIsIndexElement(prefix, localName, namespaceUri, doc);
                    break;

                case LabelElementTagName:
                    elementToReturn = new HtmlLabelElement(prefix, localName, namespaceUri, doc);
                    break;

                case LegendElementTagName:
                    elementToReturn = new HtmlLegendElement(prefix, localName, namespaceUri, doc);
                    break;

                case LIElementTagName:
                    elementToReturn = new HtmlLIElement(prefix, localName, namespaceUri, doc);
                    break;

                case LinkElementTagName:
                    elementToReturn = new HtmlLinkElement(prefix, localName, namespaceUri, doc);
                    break;

                case MapElementTagName:
                    elementToReturn = new HtmlMapElement(prefix, localName, namespaceUri, doc);
                    break;

                case MenuElementTagName:
                    elementToReturn = new HtmlMenuElement(prefix, localName, namespaceUri, doc);
                    break;

                case MetaElementTagName:
                    elementToReturn = new HtmlMetaElement(prefix, localName, namespaceUri, doc);
                    break;

                case ObjectElementTagName:
                    elementToReturn = new HtmlObjectElement(prefix, localName, namespaceUri, doc);
                    break;

                case OLElementTagName:
                    elementToReturn = new HtmlOListElement(prefix, localName, namespaceUri, doc);
                    break;

                case OptGroupElementTagName:
                    elementToReturn = new HtmlOptGroupElement(prefix, localName, namespaceUri, doc);
                    break;

                case OptionElementTagName:
                    elementToReturn = new HtmlOptionElement(prefix, localName, namespaceUri, doc);
                    break;

                case ParaElementTagName:
                    elementToReturn = new HtmlParagraphElement(prefix, localName, namespaceUri, doc);
                    break;

                case ParamElementTagName:
                    elementToReturn = new HtmlParamElement(prefix, localName, namespaceUri, doc);
                    break;

                case PreElementTagName:
                    elementToReturn = new HtmlPreElement(prefix, localName, namespaceUri, doc);
                    break;

                case ScriptElementTagName:
                    elementToReturn = new HtmlScriptElement(prefix, localName, namespaceUri, doc);
                    break;

                case SelectElementTagName:
                    elementToReturn = new HtmlSelectElement(prefix, localName, namespaceUri, doc);
                    break;

                case StyleElementTagName:
                    elementToReturn = new HtmlStyleElement(prefix, localName, namespaceUri, doc);
                    break;

                case TableElementTagName:
                    elementToReturn = new HtmlTableElement(prefix, localName, namespaceUri, doc);
                    break;

                case TBodyElementTagName:
                case TFootElementTagName:
                case THeadElementTagName:
                    elementToReturn = new HtmlTableSectionElement(prefix, localName, namespaceUri, doc);
                    break;

                case TDElementTagName:
                    elementToReturn = new HtmlTableCellElement(prefix, localName, namespaceUri, doc);
                    break;

                case TextAreaElementTagName:
                    elementToReturn = new HtmlTextAreaElement(prefix, localName, namespaceUri, doc);
                    break;

                case TRElementTagName:
                    elementToReturn = new HtmlTableRowElement(prefix, localName, namespaceUri, doc);
                    break;

                case ULElementTagName:
                    elementToReturn = new HtmlUListElement(prefix, localName, namespaceUri, doc);
                    break;

                default:
                    elementToReturn = new HtmlElement(prefix, localName, namespaceUri, doc);
                    break;
            }

            return elementToReturn;
        }

        public static bool IsPhrasingElementName(string elementName)
        {
            return !IsScopingElementName(elementName) && !IsSpecialElementName(elementName) && !IsFormattingElementName(elementName);
        }

        public static bool IsScopingElementName(string elementName)
        {
            // TODO: SVG's foreignObject
            return elementName == HtmlElementFactory.AnchorElementTagName ||
                elementName == HtmlElementFactory.CaptionElementTagName ||
                elementName == HtmlElementFactory.HtmlElementTagName ||
                elementName == HtmlElementFactory.MarqueeElementTagName ||
                elementName == HtmlElementFactory.ObjectElementTagName ||
                elementName == HtmlElementFactory.TableElementTagName ||
                elementName == HtmlElementFactory.TDElementTagName ||
                elementName == HtmlElementFactory.THElementTagName;
        }

        public static bool IsSpecialElementName(string elementName)
        {
            return elementName == HtmlElementFactory.AddressElementTagName ||
                elementName == HtmlElementFactory.AreaElementTagName ||
                elementName == HtmlElementFactory.ArticleElementTagName ||
                elementName == HtmlElementFactory.AsideElementTagName ||
                elementName == HtmlElementFactory.BaseElementTagName ||
                elementName == HtmlElementFactory.BaseFontElementTagName ||
                elementName == HtmlElementFactory.BgSoundElementTagName ||
                elementName == HtmlElementFactory.BlockQuoteElementTagName ||
                elementName == HtmlElementFactory.BodyElementTagName ||
                elementName == HtmlElementFactory.BRElementTagName ||
                elementName == HtmlElementFactory.CenterElementTagName ||
                elementName == HtmlElementFactory.ColElementTagName ||
                elementName == HtmlElementFactory.AreaElementTagName ||
                elementName == HtmlElementFactory.ColGroupElementTagName ||
                elementName == HtmlElementFactory.CommandElementTagName ||
                elementName == HtmlElementFactory.DDElementTagName ||
                elementName == HtmlElementFactory.DetailsElementTagName ||
                elementName == HtmlElementFactory.DirElementTagName ||
                elementName == HtmlElementFactory.DivElementTagName ||
                elementName == HtmlElementFactory.DLElementTagName ||
                elementName == HtmlElementFactory.DTElementTagName ||
                elementName == HtmlElementFactory.EmbedElementTagName ||
                elementName == HtmlElementFactory.FieldsetElementTagName ||
                elementName == HtmlElementFactory.FigCaptionElementTagName ||
                elementName == HtmlElementFactory.FooterElementTagName ||
                elementName == HtmlElementFactory.FormElementTagName ||
                elementName == HtmlElementFactory.FrameElementTagName ||
                elementName == HtmlElementFactory.H1ElementTagName ||
                elementName == HtmlElementFactory.H2ElementTagName ||
                elementName == HtmlElementFactory.H3ElementTagName ||
                elementName == HtmlElementFactory.H4ElementTagName ||
                elementName == HtmlElementFactory.H5ElementTagName ||
                elementName == HtmlElementFactory.H6ElementTagName ||
                elementName == HtmlElementFactory.HeadElementTagName ||
                elementName == HtmlElementFactory.HeaderElementTagName ||
                elementName == HtmlElementFactory.HGroupElementTagName ||
                elementName == HtmlElementFactory.HrElementTagName ||
                elementName == HtmlElementFactory.IFrameElementTagName ||
                elementName == HtmlElementFactory.ImgElementTagName ||
                elementName == HtmlElementFactory.InputElementTagName ||
                elementName == HtmlElementFactory.IsIndexElementTagName ||
                elementName == HtmlElementFactory.LIElementTagName ||
                elementName == HtmlElementFactory.LinkElementTagName ||
                elementName == HtmlElementFactory.ListingElementTagName ||
                elementName == HtmlElementFactory.MenuElementTagName ||
                elementName == HtmlElementFactory.MetaElementTagName ||
                elementName == HtmlElementFactory.NavElementTagName ||
                elementName == HtmlElementFactory.NoEmbedElementTagName ||
                elementName == HtmlElementFactory.NoFramesElementTagName ||
                elementName == HtmlElementFactory.NoScriptElementTagName ||
                elementName == HtmlElementFactory.OLElementTagName ||
                elementName == HtmlElementFactory.ParaElementTagName ||
                elementName == HtmlElementFactory.ParamElementTagName ||
                elementName == HtmlElementFactory.PlainTextElementTagName ||
                elementName == HtmlElementFactory.PreElementTagName ||
                elementName == HtmlElementFactory.ScriptElementTagName ||
                elementName == HtmlElementFactory.SectionElementTagName ||
                elementName == HtmlElementFactory.SelectElementTagName ||
                elementName == HtmlElementFactory.StyleElementTagName ||
                elementName == HtmlElementFactory.TBodyElementTagName ||
                elementName == HtmlElementFactory.TextAreaElementTagName ||
                elementName == HtmlElementFactory.TFootElementTagName ||
                elementName == HtmlElementFactory.THeadElementTagName ||
                elementName == HtmlElementFactory.TitleElementTagName ||
                elementName == HtmlElementFactory.TRElementTagName ||
                elementName == HtmlElementFactory.ULElementTagName ||
                elementName == HtmlElementFactory.WbrElementTagName ||
                elementName == HtmlElementFactory.XmpElementTagName;
        }

        public static bool IsFormattingElementName(string elementName)
        {
            return elementName == HtmlElementFactory.AnchorElementTagName ||
                elementName == HtmlElementFactory.BElementTagName ||
                elementName == HtmlElementFactory.BigElementTagName ||
                elementName == HtmlElementFactory.CodeElementTagName ||
                elementName == HtmlElementFactory.EmElementTagName ||
                elementName == HtmlElementFactory.FontElementTagName ||
                elementName == HtmlElementFactory.IElementTagName ||
                elementName == HtmlElementFactory.NoBrElementTagName ||
                elementName == HtmlElementFactory.SElementTagName ||
                elementName == HtmlElementFactory.SmallElementTagName ||
                elementName == HtmlElementFactory.StrongElementTagName ||
                elementName == HtmlElementFactory.TTElementTagName ||
                elementName == HtmlElementFactory.UElementTagName;
        }
    }
}
