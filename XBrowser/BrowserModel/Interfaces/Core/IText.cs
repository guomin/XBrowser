using System;
using System.Collections.Generic;
using System.Text;

namespace XBrowserProject.HtmlDom
{
    public interface IText : ICharacterData
    {
        /// <summary>
        /// Breaks this Text node into two Text nodes at the specified offset, keeping both in the tree as siblings. This node then only contains all the content up to the offset point. And a new Text node, which is inserted as the next sibling of this node, contains all the content at and after the offset point.
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        IText splitText(int offset);
                                      //raises(DOMException);

    }
}
