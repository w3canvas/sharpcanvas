using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.JScript;
using SharpCanvas.Host.Prototype;
using SharpCanvas.Interop;
using SharpCanvas.Core.Shared;

namespace SharpCanvas.Host.Browser
{
    public class WindowProxy : ObjectWithPrototype, IWindow
    {
        private Window _realObject;
        
        public WindowProxy()
            : base(Guid.NewGuid())
        {
            _realObject = new Window();
            // Add all members with dispids from the concrete type
            base.AddIntrinsicMembers();
        }

        public WindowProxy(IDocument document)
            : base(Guid.NewGuid())
        {
            _realObject = new Window(document);
        }

        public override void GetMember<T>(string name, out T member)
        {
            base.GetMember(name, out member);
            //if member was not found in WindowProxy nor arbitrary fields collection, try to find it in GlobalScope
            if(member == null)
            {
                MemberInfo[] memberInfos = Browser.Instance.GlobalScope.GetMember(name,
                                                                                  BindingFlags.Public | BindingFlags.GetProperty |
                                                                                  BindingFlags.GetField);
                if(memberInfos.Length > 0)
                {
                    member = (T)memberInfos[0];
                }
            }
        }

        #region Implementation of IWindow

        [DispId(1)]
        public ILocation location
        {
            get { return _realObject.location; }
            set { _realObject.location = value; }
        }

        [DispId(2)]
        public IWindow self
        {
            get { return this; }
        }

        [DispId(3)]
        public IWindow window
        {
            get { return this; }
        }

        [DispId(4)]
        public IDocument document
        {
            get { return _realObject.document; }
        }

        [DispId(5)]
        public IDocument parent
        {
            get { return _realObject.parent; }
            set { _realObject.parent = value; }
        }

        /// <summary>
        /// An event handler for the load event of a window.
        /// </summary>
        [DispId(6)]
        public object onload
        {
            get { return _realObject.onload; }
            set { _realObject.onload = value; }
        }

        /// <summary>
        /// Gets the height of the content area of the browser window including, if rendered, the horizontal scrollbar.
        /// </summary>
        [DispId(7)]
        public int innerHeight
        {
            get { return _realObject.innerHeight; }
            set { _realObject.innerHeight = value; }
        }

        /// <summary>
        /// Gets the width of the content area of the browser window including, if rendered, the vertical scrollbar.
        /// </summary>
        [DispId(8)]
        public int innerWidth
        {
            get { return _realObject.innerWidth; }
            set { _realObject.innerWidth = value; }
        }

        /// <summary>
        /// Whenever the src attribute is set, the user agent must resolve the value of that attribute, relative to the element, and if that is successful, 
        /// the nested browsing context must be navigated to the resulting absolute URL, with the frame element's document's browsing context as the source browsing context.
        /// </summary>
        [DispId(9)]
        public string src
        {
            get { return _realObject.src; }
            set { _realObject.src = value; }
        }

        /// <summary>
        /// An attribute containing a unique name used to refer to this Window object.
        /// Need to describe how this could come from a containing element. 
        /// </summary>
        [DispId(10)]
        public string name
        {
            get { return _realObject.name; }
            set { _realObject.name = value; }
        }

        /// <summary>
        /// An attribute containing a reference to the topmost Window object in the hierarchy that contains this object.
        /// </summary>
        [DispId(11)]
        public object top
        {
            get { return _realObject.top; }
            set { _realObject.top = value; }
        }

        /// <summary>
        /// referencing <html:frame>, <html:iframe>, <html:object>, <svg:foreignObject>,
        /// <svg:animation> or other embedding point, or null if none
        /// </summary>
        [DispId(12)]
        public object frameElement
        {
            get { return _realObject.frameElement; }
            set { _realObject.frameElement = value; }
        }

        /// <summary>
        /// Reference to parent window
        /// </summary>
        [DispId(13)]
        public IWindow parentWindow
        {
            get { return _realObject.parentWindow; }
            set { _realObject.parentWindow = value; }
        }

        /// <summary>
        /// Left position of the control
        /// </summary>
        [DispId(14)]
        public int Left
        {
            get { return _realObject.Left; }
            set { _realObject.Left = value; }
        }

        /// <summary>
        /// Top position of the control
        /// </summary>
        [DispId(15)]
        public int Top
        {
            get { return _realObject.Top; }
            set { _realObject.Top = value; }
        }

        [DispId(16)]
        public void setAttribute(object name, object value)
        {
            _realObject.setAttribute(name, value);
        }

        /// <summary>
        /// This method allows the registration of event listeners on the event target.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="listener"></param>
        /// <param name="useCapture"></param>
        [DispId(17)]
        public void addEventListener(string type, ScriptFunction listener, bool useCapture)
        {
            _realObject.addEventListener(type, listener, useCapture);
        }

        /// <summary>
        /// Executes a code snippet or a function after specified delay.
        /// </summary>
        /// <param name="func">func is the function you want to execute after delay milliseconds</param>
        /// <param name="milliseconds">is the number of milliseconds (thousandths of a second) that the function call should be delayed by.</param>
        /// <returns>timeoutID is the ID of the timeout, which can be used with window.clearTimeout.</returns>
        [DispId(18)]
        public int setTimeout(object func, object milliseconds)
        {
            return _realObject.setTimeout(func, milliseconds);
        }

        /// <summary>
        /// Clears the delay set by window.setTimeout().
        /// </summary>
        /// <param name="key">where key is the ID of the timeout you wish to clear, as returned by window.setTimeout().</param>
        [DispId(19)]
        public void clearTimeout(int key)
        {
            _realObject.clearTimeout(key);
        }

        /// <summary>
        /// Calls a function repeatedly, with a fixed time delay between each call to that function.
        /// </summary>
        /// <param name="func">func is the function you want to be called repeatedly.</param>
        /// <param name="milliseconds">is the number of milliseconds (thousandths of a second) that the setInterval() function should wait before each call to func.</param>
        /// <returns>unique interval ID you can pass to clearInterval().</returns>
        [DispId(20)]
        public int setInterval(object func, object milliseconds)
        {
            return _realObject.setInterval(func, milliseconds);
        }

        /// <summary>
        /// Cancels repeated action which was set up using setInterval(). 
        /// </summary>
        /// <param name="key">is the identifier of the repeated action you want to cancel. This ID is returned from setInterval(). </param>
        [DispId(21)]
        public void clearInterval(int key)
        {
            _realObject.clearInterval(key);
        }

        [DispId(22)]
        public INavigator navigator
        {
            get { return _realObject.navigator; }
        }

        /// <summary>
        /// Redraw visible only childrens (take in count z-index of the children)
        /// </summary>
        public void RedrawChildren()
        {
            _realObject.RedrawChildren();
        }

        public IEventModel eventModel
        {
            get { return _realObject.eventModel; }
        }

        public IWindow GetRealObject()
        {
            return _realObject;
        }

        #endregion

        #region Implementation of INode

        /// <summary>
        /// Reference to the direct parent node
        /// </summary>
        public INode parentNode
        {
            get { return _realObject.parentNode; }
        }

        /// <summary>
        /// A NodeList that contains all children of this node. If there are no children, this is a NodeList containing no nodes.
        /// </summary>
        public List<INode> childNodes
        {
            get { return _realObject.childNodes; }
        }

        /// <summary>
        /// Adds the node newChild to the end of the list of children of this node. If the newChild is already in the tree, it is first removed.
        /// </summary>
        /// <param name="child"></param>
        public void appendChild(object child)
        {
            _realObject.appendChild(child);
        }

        /// <summary>
        /// Removes the child node indicated by oldChild from the list of children, and returns it.
        /// </summary>
        /// <param name="child"></param>
        public void removeChild(object child)
        {
            _realObject.removeChild(child);
        }

        /// <summary>
        /// The Document object associated with this node. This is also the Document object used to create new nodes.
        /// </summary>
        public object ownerDocument
        {
            get { return _realObject.ownerDocument; }
        }

        #endregion
    }
}
