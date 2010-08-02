using System;
using System.Collections.Generic;
using System.Text;

namespace XBrowserProject.HtmlDom
{
    internal interface IEntityReference : INode
    {
        // Introduced in DOM Level 2:
        string publicId { get; }

        // Introduced in DOM Level 2:
        string systemId { get; }

        string notationName { get; }
    }
}
