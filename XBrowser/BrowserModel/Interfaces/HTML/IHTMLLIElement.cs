using System;
namespace XBrowserProject.HtmlDom
{
    internal interface IHTMLLIElement
    {
        /**
         * List item bullet style. See the type attribute definition in Html 4.01. 
         * This attribute is deprecated in Html 4.01.
         */
        string type { get; set; }

        /**
         * Reset sequence number when used in <code>OL</code>. See the value 
         * attribute definition in Html 4.01. This attribute is deprecated in 
         * Html 4.01.
         */
        int value { get; set; }
    }
}
