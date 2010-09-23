namespace XBrowserProject.Internal.Html.DOM.Parsing
{
    internal class ParseError
    {
        private string errorDescription;
        private int errorPosition;
        private int errorLineNumber;
        private string errorActionTaken;

        public ParseError(string description, string actionTaken, int lineNumber, int position)
        {
            errorDescription = description;
            errorPosition = position;
            errorActionTaken = actionTaken;
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
            get { return errorLineNumber; }
        }

        public int Position
        {
            get { return errorPosition; }
        }

        public override string ToString()
        {
            return string.Format("{0}, line: {1}, position: {2}, action taken: {3}", Description, LineNumber, Position, ActionTaken);
        }
    }
}
