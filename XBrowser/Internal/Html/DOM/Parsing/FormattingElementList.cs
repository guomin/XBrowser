using System.Collections.Generic;

namespace XBrowserProject.Internal.Html.DOM.Parsing
{
    internal class FormattingElementList : List<HtmlElement>
    {
        public HtmlElement GetLastElement(string elementType)
        {
            HtmlElement elementToReturn = null;
            int elementIndex = Count - 1;
            while (elementIndex >= 0 && elementToReturn == null)
            {
                HtmlElement element = this[elementIndex];
                if (element.Name == elementType)
                {
                    elementToReturn = element;
                }
                else if (element.IsFormattingScopeMarker)
                {
                    elementToReturn = element;
                }
                else
                {
                    elementIndex--;
                }
            }

            if (elementToReturn != null && elementToReturn.Name != elementType)
            {
                elementToReturn = null;
            }

            return elementToReturn;
        }

        public void ReplaceElement(HtmlElement oldElement, HtmlElement newElement)
        {
            int index = IndexOf(oldElement);
            this[index] = newElement;
        }

        internal void ClearToLastMarker()
        {
            bool markerFound = false;
            int index = Count - 1;
            while (index >= 0 && !markerFound)
            {
                if (this[index].IsFormattingScopeMarker)
                {
                    markerFound = true;
                }

                // Go ahead and remove this, even if the marker is found.
                // The spec says "clearing to the last marker" includes 
                // removing the marker element itself.
                RemoveAt(index);
                index--;
            }
        }
    }
}
