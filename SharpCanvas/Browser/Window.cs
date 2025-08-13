using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using SharpCanvas.Host;
using SharpCanvas.Interop;
using SharpCanvas.Shared;

namespace SharpCanvas.Host.Browser
{
    public delegate void LoadAssemblyHandler();

    /// <summary>
    /// Represents window element, but in standalone environment
    /// </summary>
    [ComVisible(true)]
    public class Window : UserControl, IWindow, ITimeout, IEventTarget
    {
        #region Private Variables

        private const int WM_ERASEBACKGROUND = 0x0014;

        private object sync = new object();
        private Document _document;
        private int _innerHeight;
        private int _innerWidth;
        // FIXME: Abstract to Host.
        private ITimeout _timeout = null!;
        private SaveFileDialog _dlgSaveFile;
        private bool isDialogOpen;
        private string _src = string.Empty;
        private IDocument _parent = null!;
        private Navigator _navigator = new Navigator();
        protected Dictionary<string, List<IEventRegistration>> _events = new Dictionary<string, List<IEventRegistration>>();
        private EventModel _eventModel;
        private IWindow _parentWindow = null!;
        private List<INode> _childNodes;

        #endregion

        public Window(IDocument parentDocument):this()
        {
            _parent = parentDocument;
        }

        public Window()
        {
            _eventModel = new EventModel(this);
            //by default window is a parent control for all other controls, canvases, etc.
            Visible = true;
            Paint += Window_Paint;
            _innerWidth = ClientRectangle.Width;
            _innerHeight = ClientRectangle.Height;
            //BackColor = Color.Empty;
            UpdateClientRect();
            Initialize();
            //ConfigureHook();
            Name = "window";
            _location = new Location();
            _location.OnSaveFile += location_OnSaveFile;
            // 
            // dlgSaveFile
            // 
            _dlgSaveFile = new SaveFileDialog();
            _dlgSaveFile.Title = "Save PDF File";
            _dlgSaveFile.Filter = "PDF|*.pdf";
            //
            // document
            //
            _document = new Document(this);
            _document.Width = _innerWidth;
            _document.Height = _innerHeight;
            Controls.Add(_document);
            _childNodes = new List<INode>();
            _childNodes.Add(_document);
            //
            // Events
            //);
            this.Resize += new EventHandler(Window_Resize);
        }

        void Window_Resize(object? sender, EventArgs e)
        {
            _document.Width = this.Width;
            _document.Height = this.Height;
        }

        #region IWindow members

        //private object _globalScope;

        //public object globalScope
        //{
        //    get { return _globalScope; }
        //    set { _globalScope = value; }
        //}

        public INavigator navigator
        {
            get { return _navigator; }
        }


        private ILocation? _location;
        
        /// <summary>
        /// The value of the location attribute MUST be the Location object that represents the window's current location.
        /// </summary>
        public ILocation? location
        {
            get
            {
                return _location;
            }
            set
            {
#pragma warning disable CS8601 // Possible null reference assignment.
                _location = value;
#pragma warning restore CS8601 // Possible null reference assignment.
            }
        }

        /// <summary>
        /// Retrieves a reference to the current window or frame. 
        /// </summary>
        public IWindow window
        {
            get { return this; }
        }

        /// <summary>
        /// Retrieves a reference to the current window or frame. 
        /// </summary>
        public IWindow self
        {
            get { return this; }
        }

        /// <summary>
        /// Reference to the document object
        /// </summary>
        public IDocument document
        {
            get { return _document; }
        }

        public IDocument parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        #endregion 

        public object onload { get; set; } = new object();

        public int innerHeight
        {
            get { return _innerHeight; }
            set
            {
                if (value >= 0)
                {
                   this.Height = _innerHeight = value;
                   UpdateClientRect();
                }
            }
        }

        public int innerWidth
        {
            get { return _innerWidth; }
            set
            {
                if (value >= 0)
                {
                    this.Width = _innerWidth = value;
                    UpdateClientRect();
                }
            }
        }

        public void setAttribute(object name, object value)
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            string strName = name.ToString();
            string strValue = value.ToString();
            switch (strName.ToLower())
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            {
                case "width":
                    {
                        int width = 0;
                        int.TryParse(strValue, out width);
                        innerWidth = width;
                        break;
                    }
                case "height":
                    {
                        int height = 0;
                        int.TryParse(strValue, out height);
                        innerHeight = height;
                        break;
                    }
                case "name":
                    {
                        Name = strValue;
                        break;
                    }

                case "src":
                    {
                        src = strValue;
                        break;
                    }
            }
        }

        public string src
        {
            get { return _src; }
            set
            {
                _src = value;
                location.href = _src;
            }
        }

        #region Utils

        private void UpdateClientRect()
        {
            ClientSize = new Size(_innerWidth, _innerHeight);
        }

        /// <summary>
        /// In this method we catch WM_ERASEBACKGROUND event, which indicates that all childrens or
        /// some part of them should be redrawn. And do redraw.
        /// We can't use WM_PAINT for this purpose because WM_PAINT triggers all the time re-paint occurs.
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_ERASEBACKGROUND)
            {
                RedrawChildren();
                m.Result = new IntPtr(1);
                return;
            }
        }

        /// <summary>
        /// Redraw visible only childrens (take in count z-index of the children)
        /// </summary>
        public void RedrawChildren()
        {
            if (document != null && document.body != null)
            {
                var toRedraw = new Dictionary<int, List<global::SharpCanvas.Shared.IHTMLCanvasElement>>();
                int maxZIndex = 0;
                //build the order to redraw
                UserControl body = document.body as UserControl;
                foreach (object el in body.Controls)
                {
                    if (el is global::SharpCanvas.Shared.IHTMLCanvasElement)
                    {
                        global::SharpCanvas.Shared.IHTMLCanvasElement element = (global::SharpCanvas.Shared.IHTMLCanvasElement)el;
                        if (element.style.display != "none")
                        {
                            if (!toRedraw.ContainsKey(element.style.zIndex))
                            {
                                toRedraw.Add(element.style.zIndex, new List<global::SharpCanvas.Shared.IHTMLCanvasElement>());
                            }
                            toRedraw[element.style.zIndex].Add(element);
                            if (maxZIndex < element.style.zIndex)
                            {
                                maxZIndex = element.style.zIndex;
                            }
                        }
                    }
                }
                //request redraw
                for (int i = 0; i <= maxZIndex; i++)
                {
                    if (toRedraw.ContainsKey(i))
                    {
                        foreach (global::SharpCanvas.Shared.IHTMLCanvasElement element in toRedraw[i])
                        {
                            element.RequestDraw();
                        }
                    }
                }
            }
        }

        public IEventModel eventModel
        {
            get { return _eventModel; }
        }

        private void Window_Paint(object? sender, PaintEventArgs e)
        {
            InvokeOnload();
        }

        /// <summary>
        /// Invokes onload function if specified and remove reference to the function
        /// </summary>
        public void InvokeOnload()
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            if (this.onload != null) //run only once
            {
                if (onload is Delegate)
                {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    var function = (Delegate) this.onload;
                    this.onload = null;
                    function.DynamicInvoke(this, new object[] {});
                    RedrawChildren();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                //if(this.onload is LoadAssemblyHandler)
                    //{
                    //    var loadAssembly = (LoadAssemblyHandler)this.onload;
                    //    loadAssembly.Invoke();
                    //}
            }
            else if (location.assembly != null)
            {
                Type[] types = location.assembly.GetTypes();
                foreach (Type type in types)
                {
                    //todo: check type by interface rather than by name
                    //todo: fix exception in: Convert({ PT: 'PC', MM: { CM: 'M' }, IN: { FT: 'YD' }, EX: 'EM' });
                    if (type.Name == "Adsense")
                    {
                        ConstructorInfo[] constructors = type.GetConstructors();
                        foreach (ConstructorInfo constructor in constructors)
                        {
                            ParameterInfo[] parameterInfos = constructor.GetParameters();
                            if (parameterInfos.Length == 2)
                            {
                                //GlobalScope globalScope = 
                                //    Microsoft.JScript.Vsa.VsaEngine.CreateEngineAndGetGlobalScope(false, new string[]{});

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                                constructor.Invoke(new object[] {this._parentWindow, this._parentWindow.document});
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                                //constructor.Invoke(new object[] {});
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void location_OnSaveFile(byte[] data)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            if (!isDialogOpen)
            {
                DialogResult dialogResult = _dlgSaveFile.ShowDialog();
                isDialogOpen = true;
                if (DialogResult.OK == dialogResult)
                {
                    File.WriteAllBytes(_dlgSaveFile.FileName, data);
                }
                isDialogOpen = false;
            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        #endregion

        #region HTMLElement methods

        /// <summary>
        /// An attribute containing a unique name used to refer to this Window object.
        /// Need to describe how this could come from a containing element. 
        /// </summary>
        public string name { get; set; } = string.Empty;

        /// <summary>
        /// An attribute containing a reference to the topmost Window object in the hierarchy that contains this object.
        /// </summary>
        public object top { get; set; } = new object();

        /// <summary>
        /// referencing <html:frame>, <html:iframe>, <html:object>, <svg:foreignObject>,
        /// <svg:animation> or other embedding point, or null if none
        /// </summary>
        public object frameElement { get; set; } = new object();

        public IWindow parentWindow
        {
            get {
                return _parentWindow;
            }
            set {
                _parentWindow = value;
            }
        }

        #endregion

        #region Events

        private void Initialize()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            Click += _eventModel.click;
            DragDrop += _eventModel.drag;
            DragEnter += _eventModel.dragenter;
            DragLeave += _eventModel.dragleave;
            DragOver += _eventModel.dragover;
            DoubleClick += _eventModel.dblclick;
            Enter += _eventModel.focus;
            KeyDown += _eventModel.keydown;
            KeyPress += _eventModel.keypress;
            KeyUp += _eventModel.keyup;
            Load += _eventModel.load;
            MouseClick += _eventModel.click;
            MouseHover += _eventModel.mouseover;
            MouseDown += _eventModel.mousedown;
            MouseUp += _eventModel.mouseup;
            MouseMove += _eventModel.mousemove;
            MouseLeave += _eventModel.mouseout;
            MouseWheel += _eventModel.mousewheel;
            Scroll += _eventModel.scroll;
        }

        /// <summary>
        /// Trigger some javascript event on the object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="eventName"></param>
        public void InvokeEvent(object sender, EventArgs e, string eventName)
        {
            foreach (Delegate sf in _events[eventName])
            {
                sf.GetType().InvokeMember("", BindingFlags.InvokeMethod, null, sf,
                                          new object[] {e});
            }
        }

        #endregion

        #region Timeouts

        /// <summary>
        /// Executes a code snippet or a function after specified delay.
        /// </summary>
        /// <param name="func">func is the function you want to execute after delay milliseconds</param>
        /// <param name="milliseconds">is the number of milliseconds (thousandths of a second) that the function call should be delayed by.</param>
        /// <returns>timeoutID is the ID of the timeout, which can be used with window.clearTimeout.</returns>
        public int setTimeout(object func, object milliseconds)
        {
            EnsureTimeout();
            return _timeout.setTimeout(func, milliseconds);
        }

        /// <summary>
        /// Clears the delay set by window.setTimeout().
        /// </summary>
        /// <param name="key">where key is the ID of the timeout you wish to clear, as returned by window.setTimeout().</param>
        public void clearTimeout(int key)
        {
            EnsureTimeout();
            _timeout.clearTimeout(key);
        }

        /// <summary>
        /// Calls a function repeatedly, with a fixed time delay between each call to that function.
        /// </summary>
        /// <param name="func">func is the function you want to be called repeatedly.</param>
        /// <param name="milliseconds">is the number of milliseconds (thousandths of a second) that the setInterval() function should wait before each call to func.</param>
        /// <returns>unique interval ID you can pass to clearInterval().</returns>
        public int setInterval(object func, object milliseconds)
        {
            EnsureTimeout();
            return _timeout.setInterval(func, milliseconds);
        }

        /// <summary>
        /// Cancels repeated action which was set up using setInterval(). 
        /// </summary>
        /// <param name="key">is the identifier of the repeated action you want to cancel. This ID is returned from setInterval(). </param>
        public void clearInterval(int key)
        {
            EnsureTimeout();
            _timeout.clearInterval(key);
        }

        /// <summary>
        /// Ensures that timeout object initialized
        /// </summary>
        private void EnsureTimeout()
        {
            if (_timeout == null)
            {
                _timeout = StandaloneBootstrapper.Factory.CreateTimeout();
            }
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

        #region Implementation of INode

        /// <summary>
        /// Reference to the direct parent node
        /// </summary>
        public object parentNode
        {
            get {
#pragma warning disable CS8603 // Possible null reference return.
                return null;
#pragma warning restore CS8603 // Possible null reference return.
            }
        }

        /// <summary>
        /// A NodeList that contains all children of this node. If there are no children, this is a NodeList containing no nodes.
        /// </summary>
        public object childNodes
        {
            get { return _childNodes; }
        }

        /// <summary>
        /// Adds the node newChild to the end of the list of children of this node. If the newChild is already in the tree, it is first removed.
        /// </summary>
        /// <param name="child"></param>
        public void appendChild(object child)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes the child node indicated by oldChild from the list of children, and returns it.
        /// </summary>
        /// <param name="child"></param>
        public void removeChild(object child)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The Document object associated with this node. This is also the Document object used to create new nodes.
        /// </summary>
        public object ownerDocument
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}