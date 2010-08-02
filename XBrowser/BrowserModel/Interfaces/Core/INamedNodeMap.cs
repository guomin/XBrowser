using System;
using System.Collections.Generic;
using System.Text;

namespace XBrowserProject.HtmlDom
{
    internal interface INamedNodeMap
    {
        /// <summary>
        /// The number of nodes in the map. The range of valid child node indices is 0 to length-1 inclusive.
        /// </summary>
        int length
        {
            get;
        }

        /// <summary>
        /// Retrieves a node specified by name.
        /// </summary>
        /// <param name="name">Name of a node to retrieve.</param>
        /// <returns>A Node (of any type) with the specified name, or null if the specified name did not identify any node in the map.</returns>
        INode getNamedItem(string name);

        /// <summary>
        /// Adds a node using its nodeName attribute.
        /// As the nodeName attribute is used to derive the name which the node must be stored under, multiple nodes of certain types (those that have a "special" string value) cannot be stored as the names would clash. This is seen as preferable to allowing nodes to be aliased. 
        /// </summary>
        /// <param name="arg">A node to store in a named node map. The node will later be accessible using the value of the nodeName attribute of the node. If a node with that name is already present in the map, it is replaced by the new one.</param>
        /// <returns>If the new Node replaces an existing node with the same name the previously existing Node is returned, otherwise null is returned.</returns>
        INode setNamedItem(INode arg);
        //raises(DOMException);

        /// <summary>
        /// Removes a node specified by name. If the removed node is an Attr with a default value it is immediately replaced.
        /// </summary>
        /// <param name="name">The name of a node to remove.</param>
        /// <returns>The node removed from the map or null  if no node with such a name exists.</returns>
        INode removeNamedItem(string name);
        //raises(DOMException);

        /// <summary>
        /// Returns the indexth item in the map. If index is greater than or equal to the number of nodes in the map, this returns null.
        /// </summary>
        /// <param name="index">Index into the map.</param>
        /// <returns>The node at the indexth position in the NamedNodeMap, or null if that is not a valid index.</returns>
        INode item(int index);

        // Introduced  Level 2:
        INode getNamedItemNS(string namespaceURI, string localName);
       
        // Introduced  Level 2:
        INode setNamedItemNS(INode arg);
        //raises(DOMException);
        
        // Introduced  Level 2:
        INode removeNamedItemNS(string namespaceURI, string localName);
        //raises(DOMException);
    }
}
