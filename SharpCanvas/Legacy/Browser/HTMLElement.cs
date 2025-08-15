using System;
using System.Collections.Generic;
// FIXME: Is System.ComponentModel used?
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using SharpCanvas.Interop;
using SharpCanvas.Shared;

namespace SharpCanvas.Browser
{
    [ComVisible(true)]
    public class HTMLElement : Node, IHTMLElementBase, IEventTarget
    {
        #region Fields

        protected Dictionary<string, List<IEventRegistration>> _events = new Dictionary<string, List<IEventRegistration>>();


        private readonly object sync = new object();
        private IHTMLDocument4 _document = null!;

        private object _parent = null!;
        private new List<INode> _childNodes = new List<INode>();

        protected bool _isChanged;

        #endregion

        #region Properties

        //TODO: remove - event is obsolete

        /// <summary>
        /// Flag to determine wherever image on the current surface was changed or not
        /// </summary>
        public bool IsChanged
        {
            get { return _isChanged; }
            set { _isChanged = value; }
        }

        /// <summary>
        /// An object representing the declarations of an element's style attributes.
        /// </summary>
        //TODO: migrate to ICSSStyleDeclaration usage
        public object style { get; set; } = new object();

        /// <summary>
        /// Gets/sets the name attribute of an element.
        /// </summary>
        public new string name { get; set; } = string.Empty;

        /// <summary>
        /// Workaround for IE env. - uses instead of native id property
        /// </summary>
        public string Identifier { get; set; } = string.Empty;

        /// <summary>
        /// Reference to the direct parent object
        /// </summary>
        public object parent
        {
            get { return _parent; }
        }

        /// <summary>
        /// Reference to the native document object
        /// </summary>
        public IHTMLDocument4 document
        {
            get { return _document; }
            set { _document = value; }
        }

        #endregion

        #region Constructors


        public HTMLElement(string _name, Node _parent)
            : this(_name)
        {
            this._parentNode = _parent;
        }

        public HTMLElement(string _name)
            : this()
        {
            this.name = _name;
        }

        public HTMLElement()
        {
        }

        #endregion

        #region Implementation of IEventTarget

        /// <summary>
        /// This method allows the registration of event listeners on the event target. If an EventListener is added to an EventTarget while it is processing an event, it will not be triggered by the current actions but may be triggered during a later stage of event flow, such as the bubbling phase.
        /// If multiple identical EventListeners are registered on the same EventTarget with the same parameters the duplicate instances are discarded. They do not cause the EventListener to be called twice and since they are discarded they do not need to be removed with the removeEventListener method.
        /// </summary>
        /// <param name="type">The event type for which the user is registering</param>
        /// <param name="listener">The listener parameter takes an interface implemented by the user which contains the methods to be called when the event occurs.</param>
        /// <param name="useCapture">If true, useCapture indicates that the user wishes to initiate capture. After initiating capture, all events of the specified type will be dispatched to the registered EventListener before being dispatched to any EventTargets beneath them in the tree. Events which are bubbling upward through the tree will not trigger an EventListener designated to use capture.</param>
        public void addEventListener(string type, Delegate listener, bool useCapture)
        {
            lock (sync)
            {
                EventModel.addEventListener(this, _events, type, listener, useCapture);
            }
        }

        /// <summary>
        /// This method allows the removal of event listeners from the event target. If an EventListener is removed from an EventTarget while it is processing an event, it will not be triggered by the current actions. EventListeners can never be invoked after being removed.
        /// Calling removeEventListener with arguments which do not identify any currently registered EventListener on the EventTarget has no effect.
        /// </summary>
        /// <param name="type">Specifies the event type of the EventListener being removed.</param>
        /// <param name="listener">The EventListener parameter indicates the EventListener to be removed.</param>
        /// <param name="useCapture">Specifies whether the EventListener being removed was registered as a capturing listener or not. If a listener was registered twice, one with capture and one without, each must be removed separately. Removal of a capturing listener does not affect a non-capturing version of the same listener, and vice versa.</param>
        public void removeEventListener(string type, Delegate listener, bool useCapture)
        {
            lock (sync)
            {
                EventModel.removeEventListener(this, type, listener, useCapture);
            }
        }

        public Dictionary<string, List<IEventRegistration>> GetEventsCollection()
        {
            return EventModel.CloneDictionaryCloningValues(_events);
        }

        #endregion

        public void Redraw()
        {
            // Do nothing
        }

        #region INode

        Random r = new Random();
        private static Color[] colors = new Color[]{ Color.Orange, Color.Pink, Color.Gray, Color.Lime, Color.Red};


        /// <summary>
        /// Reference to the direct parent node
        /// </summary>
        public new INode? parentNode
        {
            get { return _parent as INode; }
        }

        /// <summary>
        /// A NodeList that contains all children of this node. If there are no children, this is a NodeList containing no nodes.
        /// </summary>
        public new List<INode> childNodes
        {
            get { return _childNodes; }
        }

        /// <summary>
        /// Add child node to the object
        /// </summary>
        /// <param name="child"></param>
        public new void appendChild(object child)
        {
            if (child is INode node)
            {
                _childNodes.Add(node);
            }
        }


        /// <summary>
        /// Remove child object from the object
        /// </summary>
        /// <param name="child"></param>
        public new void removeChild(object child)
        {
            if (child is INode node)
            {
                _childNodes.Remove(node);
            }
        }

        /// <summary>
        /// The Document object associated with this node. This is also the Document object used to create new nodes.
        /// </summary>
        public new object? ownerDocument
        {
            get
            {
                if(_parent == null)
                {
                    return null;
                }
                var parentElement = _parent as IHTMLElementBase;
                if (parentElement != null)
                {
                    return name == "body" ? _parent : parentElement.ownerDocument;
                }
                return name == "body" ? _parent : null;
            }
        }

        #endregion

        /// <summary>
        /// Returns the element whose ID is specified.
        /// </summary>
        /// <param name="id">id is a case-sensitive string representing the unique ID of the element being sought. </param>
        /// <returns></returns>
        public object? getElementById(string id)
        {
            foreach (INode node in _childNodes)
            {
                if (node is IHTMLElementBase element)
                {
                    if (element.Identifier == id)
                    {
                        return element;
                    }
                    var childElement = element.getElementById(id);
                    if (childElement != null)
                    {
                        return childElement;
                    }
                }
            }
            return null;
        }


        public bool hasOwnProperty(string propName)
        {
            MemberInfo[] member = this.GetType().GetMember(propName);
            if(member != null && member.Length > 0)
            {
                return true;
            }
            return false;
        }



        public void SetParent(object parent)
        {
            _parentNode = parent as Node;
        }

        public int offsetLeft { get; set; }
        public int offsetTop { get; set; }


        #region Associative Array Members

        //TODO: add support of associative arrays here

        #endregion


    }
}