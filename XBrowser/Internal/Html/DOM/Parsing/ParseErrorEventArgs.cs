using System;
using System.Xml;

namespace XBrowserProject.Internal.Html.DOM.Parsing
{
    internal class ParseErrorEventArgs : EventArgs
    {
        private string errorDescription;
        private string errorActionTaken;
        private IXmlLineInfo errorPositionInfo;

        public ParseErrorEventArgs(string description, string actionTaken, IXmlLineInfo position)
        {
            errorDescription = description;
            errorActionTaken = actionTaken;
            errorPositionInfo = position;
        }

        public string Description
        {
            get { return errorDescription; }
        }

        public string ActionTaken
        {
            get { return errorActionTaken; }
        }

        public int LineNumber
        {
            get { return errorPositionInfo.LineNumber; }
        }

        public int LinePosition
        {
            get { return errorPositionInfo.LinePosition; }
        }
    }
}
