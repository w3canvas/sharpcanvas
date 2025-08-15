using System.Collections.Generic;
using SharpCanvas.Interop;
using SharpCanvas.Shared;

namespace SharpCanvas.Browser
{
    public class Node : INode
    {
        protected Node? _parentNode;
        protected List<INode> _childNodes = new List<INode>();
        public string name { get; set; } = string.Empty;

        public INode? parentNode => _parentNode;

        public List<INode> childNodes => _childNodes;

        public virtual void appendChild(object child)
        {
            // This will be implemented by subclasses
        }

        public virtual void removeChild(object child)
        {
            // This will be implemented by subclasses
        }

        public object? ownerDocument
        {
            get
            {
                if (_parentNode == null)
                {
                    return null;
                }
                if (this is IDocument)
                {
                    return this;
                }
                return _parentNode.ownerDocument;
            }
        }
    }
}
