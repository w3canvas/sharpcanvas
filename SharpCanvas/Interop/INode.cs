using System;
using System.Collections.Generic;
using System.Text;

namespace SharpCanvas.Interop
{
    public interface INode
    {
        /// <summary>
        /// Reference to the direct parent node
        /// </summary>
        INode? parentNode { get; }

        /// <summary>
        /// A NodeList that contains all children of this node. If there are no children, this is a NodeList containing no nodes.
        /// </summary>
        List<INode> childNodes { get; }

        /// <summary>
        /// Adds the node newChild to the end of the list of children of this node. If the newChild is already in the tree, it is first removed.
        /// </summary>
        /// <param name="child"></param>
        void appendChild(object child);

        /// <summary>
        /// Removes the child node indicated by oldChild from the list of children, and returns it.
        /// </summary>
        /// <param name="child"></param>
        void removeChild(object child);

        /// <summary>
        /// The Document object associated with this node. This is also the Document object used to create new nodes.
        /// </summary>
        object? ownerDocument { get; }
    }
}
