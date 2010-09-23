using System.Xml;

namespace XBrowserProject.Internal.Html.Interfaces
{
    internal interface INodeList
    {
        /// <summary>
        /// The number of nodes in the list. The range of valid child node indices is 0 to length-1 inclusive.
        /// </summary>
        int length
        {
            get;
        }

        XmlNode this[int index]
        {
            get;
        }
    }
}
