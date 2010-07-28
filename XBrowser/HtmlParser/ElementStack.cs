using System;
using System.Collections.Generic;
using System.Text;
using XBrowserProject.BrowserModel.Internal;
using XBrowserProject.BrowserModel.Internal.HtmlDom;
using XBrowserProject.HtmlDom;

namespace XBrowserProject.HtmlParser
{
    public enum ScopeType
    {
        Element,
        ListItem,
        Table
    }

    public class ElementStack : List<HtmlElement>
    {
        private static Dictionary<ScopeType, List<string>> scopeElementNames;

        public void Push(HtmlElement element)
        {
            Insert(0, element);
        }

        public HtmlElement Pop()
        {
            HtmlElement element = this[0];
            RemoveAt(0);
            return element;
        }

        public HtmlElement Peek()
        {
            return this[0];
        }

        public bool HasElementInScope(HtmlElement element)
        {
            return HasElementInScope(element, ScopeType.Element);
        }

        public bool HasElementInScope(HtmlElement element, ScopeType scope)
        {
            // The stack of open elements is said to have an element in a specific scope consisting
            // of a list of element types list when the following algorithm terminates in a match state:

            // 1.Initialize node to be the current node (the bottommost node of the stack).

            // 2.If node is the target node, terminate in a match state.

            // 3.Otherwise, if node is one of the element types in list, terminate in a failure state.

            // 4.Otherwise, set node to the previous entry in the stack of open elements and return to step 2. 
            // (This will never fail, since the loop will always terminate in the previous step if the top of 
            // the stack — an html element — is reached.)
            bool elementFound = false;

            List<string> elementScopeNameList = GetScopeNameList(scope);
            foreach (HtmlElement stackElement in this)
            {
                if (stackElement == element)
                {
                    elementFound = true;
                    break;
                }

                if (elementScopeNameList.Contains(stackElement.Name))
                {
                    break;
                }
            }

            return elementFound;
        }

        public bool HasElementOfTypeInScope(string elementType)
        {
            return HasElementOfTypeInScope(elementType, ScopeType.Element);
        }

        public bool HasElementOfTypeInScope(string elementType, ScopeType scope)
        {
            bool elementFound = false;

            List<string> elementScopeNameList = GetScopeNameList(scope);
            foreach (HtmlElement element in this)
            {
                if (element.Name == elementType)
                {
                    elementFound = true;
                    break;
                }

                if (elementScopeNameList.Contains(element.Name) || (element.Name == "foreignObject" && element.NamespaceURI == Parser.SvgNamespace))
                {
                    break;
                }
            }

            return elementFound;
        }

        public void ReplaceElement(HtmlElement oldElement, HtmlElement newElement)
        {
            int index = IndexOf(oldElement);
            this[index] = newElement;
        }

        private static List<string> GetScopeNameList(ScopeType type)
        {
            if (scopeElementNames == null)
            {
                scopeElementNames = new Dictionary<ScopeType, List<string>>();
                scopeElementNames.Add(ScopeType.Element, GetElementInScopeNameList());
                scopeElementNames.Add(ScopeType.Table, GetElementInTableScopeNameList());
                scopeElementNames.Add(ScopeType.ListItem, GetElementInListItemScopeNameList());
            }

            return scopeElementNames[type];
        }

        private static List<string> GetElementInScopeNameList()
        {
            List<string> elementInScopeNameList = new List<string>();
            elementInScopeNameList.Add(HtmlElementFactory.AppletElementTagName);
            elementInScopeNameList.Add(HtmlElementFactory.CaptionElementTagName);
            elementInScopeNameList.Add(HtmlElementFactory.HtmlElementTagName);
            elementInScopeNameList.Add(HtmlElementFactory.TableElementTagName);
            elementInScopeNameList.Add(HtmlElementFactory.TDElementTagName);
            elementInScopeNameList.Add(HtmlElementFactory.THElementTagName);
            elementInScopeNameList.Add(HtmlElementFactory.ButtonElementTagName);
            elementInScopeNameList.Add(HtmlElementFactory.ObjectElementTagName);
            elementInScopeNameList.Add(HtmlElementFactory.MarqueeElementTagName);
            
            return elementInScopeNameList; 
        }

        private static List<string> GetElementInTableScopeNameList()
        {
            List<string> elementInTableScopeNameList = new List<string>();
            elementInTableScopeNameList.Add(HtmlElementFactory.HtmlElementTagName);
            elementInTableScopeNameList.Add(HtmlElementFactory.TableElementTagName);

            return elementInTableScopeNameList; 
        }

        private static List<string> GetElementInListItemScopeNameList()
        {
            List<string> elementInListItemScopeNameList = new List<string>();
            elementInListItemScopeNameList.AddRange(GetElementInScopeNameList());
            elementInListItemScopeNameList.Add(HtmlElementFactory.OLElementTagName);
            elementInListItemScopeNameList.Add(HtmlElementFactory.ULElementTagName);

            return elementInListItemScopeNameList;
        }

        internal HtmlElement GetFosterParentElement()
        {
            HtmlElement fosterParentElement = null;
            HtmlElement lastTable = GetLastTable();
            HtmlElement lastTableParent = lastTable.ParentNode as HtmlElement;

            if (lastTable.Name == HtmlElementFactory.TableElementTagName && lastTableParent != null)
            {
                fosterParentElement = lastTableParent;
            }
            else
            {
                if (lastTable.Name == HtmlElementFactory.HtmlElementTagName)
                {
                    fosterParentElement = lastTable;
                }
                else
                {
                    fosterParentElement = this[IndexOf(lastTable) + 1];
                }
            }

            return fosterParentElement;
        }

        internal HtmlElement GetLastTable()
        {
            HtmlElement lastTable = null;
            for (int i = 0; i < Count; i++)
            {
                if (this[i].Name == HtmlElementFactory.TableElementTagName)
                {
                    lastTable = this[i];
                    break;
                }
            }

            if (lastTable == null)
            {
                lastTable = this[Count - 1];
            }

            return lastTable;
        }
    }
}
