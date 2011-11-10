using System;
using System.Collections.Generic;
// FIXME: Is System.ComponentModel used?
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Microsoft.JScript;
using SharpCanvas.Interop;
using SharpCanvas.Shared;

namespace SharpCanvas.Host.Browser
{
    [ComVisible(true)]
    public class HTMLElement : UserControl, IHTMLElementBase, IEventTarget
    {
        #region Fields

        protected Dictionary<string, List<IEventRegistration>> _events = new Dictionary<string, List<IEventRegistration>>();
        

        private readonly object sync = new object();
        private IHTMLDocument4 _document;

        private object _parent;
        private List<INode> _childNodes = new List<INode>();

        protected bool _isChanged;

        #endregion

        #region Properties

        //TODO: remove - event is obsolete
        public event ControlsTreeChangeHandler ControlsTreeChange;

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
        public object style { get; set; }

        /// <summary>
        /// Gets/sets the name attribute of an element.
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Workaround for IE env. - uses instead of native id property
        /// </summary>
        public string Identifier { get; set; }

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


        public HTMLElement(string _name, UserControl _parent)
            : this(_name)
        {
            this._parent = _parent;
            InitializeEvents();
        }

        public HTMLElement(string _name)
            : this()
        {
            this.name = _name;
        }

        public HTMLElement()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            //BackColor = Color.Transparent;
            
            if (this is IHTMLCanvasElement)
                this.Size = new Size((int)(this as IHTMLCanvasElement).width, (int)(this as IHTMLCanvasElement).height);
            //this.AutoSize = true;
        }

        #endregion

        #region Transparent

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20;
                return cp;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //Do not paint background
        }

        //Hack
        public void Redraw()
        {
            RecreateHandle();
        }

        private void HTMLElement_Move(object sender, EventArgs e)
        {
            RecreateHandle();
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
        public void addEventListener(string type, ScriptFunction listener, bool useCapture)
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
        public void removeEventListener(string type, ScriptFunction listener, bool useCapture)
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

        #region Events

        protected void InitializeEvents()
        {
            IEventModel eventModel;
            if (this is IDocument)
            {
                eventModel = ((IWindow)this._parent).eventModel;
            }
            else
            {
                eventModel = ((IDocument) ownerDocument).defaultView.eventModel;
            }
            Click += eventModel.click;
            DragDrop += eventModel.drag;
            DragEnter += eventModel.dragenter;
            DragLeave += eventModel.dragleave;
            DragOver += eventModel.dragover;
            DoubleClick += eventModel.dblclick;
            Enter += eventModel.focus;
            KeyDown += eventModel.keydown;
            KeyPress += eventModel.keypress;
            KeyUp += eventModel.keyup;
            Load += eventModel.load;
            MouseClick += eventModel.click;
            MouseHover += eventModel.mouseover;
            MouseDown += eventModel.mousedown;
            MouseUp += eventModel.mouseup;
            MouseMove += eventModel.mousemove;
            MouseLeave += eventModel.mouseout;
            MouseWheel += eventModel.mousewheel;
            Scroll += eventModel.scroll;
            Move += HTMLElement_Move;
        }

        /// <summary>
        /// Workaround for arrows keys events
        /// </summary>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool IsInputKey(Keys keyData)
        {
            if (keyData == Keys.Right || keyData == Keys.Left || keyData == Keys.Up || keyData == Keys.Down)
            {
                return true;
            }
            else
            {
                return base.IsInputKey(keyData);
            }
        }

        public void attachEvent(object o1, object o2, bool b1)
        {
            throw new Exception("attachEvent function is not implemented for Windows environment. Use addEventListener instead.");
        }

        /// <summary>
        /// Copy events subscriptions from events collection to the target object
        /// </summary>
        /// <param name="target"></param>
        /// <param name="events"></param>
        public void CloneEvents(IEventTarget target, Dictionary<string, List<ScriptFunction>> events)
        {
            foreach (var pair in events)
            {
                foreach (ScriptFunction function in pair.Value)
                {
                    target.addEventListener(pair.Key, function, false);
                }
            }
        }

        

        #endregion

        #region INode

        Random r = new Random();
        private static Color[] colors = new Color[]{ Color.Orange, Color.Pink, Color.Gray, Color.Lime, Color.Red};
        private static int i;
        

        /// <summary>
        /// Reference to the direct parent node
        /// </summary>
        public INode parentNode
        {
            get { return _parent as INode; }
        }

        /// <summary>
        /// A NodeList that contains all children of this node. If there are no children, this is a NodeList containing no nodes.
        /// </summary>
        public List<INode> childNodes
        {
            get { return _childNodes; }
        }

        /// <summary>
        /// Add child node to the object
        /// </summary>
        /// <param name="child"></param>
        public void appendChild(object child)
        {
            UserControl control = null;
            if (child is UserControl)
            {
                control = (UserControl) child;
            }
            if (child is ICanvasProxy)
            {
                control = ((ICanvasProxy) child).RealObject as UserControl;
            }
            if(child is IFrame)
            {
                IFrame frame = (IFrame)child;
                WindowProxy wndProxy = new WindowProxy();
                wndProxy.innerHeight = frame.getAttribute<int>("height");
                wndProxy.innerWidth = frame.getAttribute<int>("width");
                //todo: add coordinates parameters to the ad/iframe
                wndProxy.Left = 0;
                wndProxy.Top = 0;
                //reference to pagead.dll
                //todo: pass canvas_ad_client - reference to specific ad assembly
                var adPath = frame.getAttribute<string>("canvas_ad_client");
                //window.onload = new LoadAssemblyHandler(delegate()
                //                                     {
                //                                         //Uri uri = new Uri(adPath);
                //                                         //AssemblyLoader assemblyLoader = new AssemblyLoader(uri);
                //                                         //assemblyLoader.Load();
                                                        
                //                                         //todo: get loaded assembly and find startPoint class, create isntance of it, initialize document and window props
                //                                     });
                wndProxy.parentWindow = frame.ParentWindow;
                wndProxy.src = frame.getAttribute<string>("src");
                control = wndProxy.GetRealObject() as UserControl;
            }
            if (control != null)
            {
                //(control as UserControl).BackColor = colors[i++];
                Controls.Add(control);
                //keep internal childNodes collection up to date.
                if (control is INode)
                {
                    _childNodes.Add((INode)control);
                }
                if (control is IHTMLElementBase)
                {
                    ((IHTMLElementBase)control).SetParent(this);
                    if (control is IHTMLCanvasElement)
                    {
                        int newIndex = 999 - ((IHTMLCanvasElement) control).style.zIndex;
                        int normalizedIndex = Browser.Instance.GetRelativeZIndex(newIndex);
                        this.SuspendLayout();
                        this.Controls.SetChildIndex(control, normalizedIndex);
                        this.ResumeLayout(true);
                        this.Refresh();
                    }
                }
            }
        }


        /// <summary>
        /// Remove child object from the object
        /// </summary>
        /// <param name="child"></param>
        public void removeChild(object child)
        {
            Controls.Remove(child as UserControl);

            if (child is INode && _childNodes.Contains((INode)child))
            {
                _childNodes.Remove((INode)child);
            }
        }

        /// <summary>
        /// The Document object associated with this node. This is also the Document object used to create new nodes.
        /// </summary>
        public object ownerDocument
        {
            get
            {
                if(_parent == null)
                {
                    return null;
                }
                return name == "body" ? _parent : ((IHTMLElementBase) _parent).ownerDocument;
            }
        }

        #endregion

        /// <summary>
        /// Returns the element whose ID is specified. 
        /// </summary>
        /// <param name="id">id is a case-sensitive string representing the unique ID of the element being sought. </param>
        /// <returns></returns>
        public object getElementById(string id)
        {
            foreach (IHTMLElementBase o in Controls) //Image is not HTMLElement!!
            {
                if(o is IHTMLCanvasElement)
                {
                    IHTMLCanvasElement c = (IHTMLCanvasElement) o;
                    ICanvasProxy canvasProxy = c.GetProxy();
                    string objectgId = GetElementId(canvasProxy);
                    if (objectgId == id)
                    {
                        return canvasProxy;
                    }
                }
                else
                {
                    object c = o.getElementById(id);
                    if (c != null)
                        return c;
                }
            }
            return null;
        }

        public string GetElementId(ICanvasProxy canvasProxy)
        {
            if(canvasProxy is IReflect)
            {
                MemberInfo[] memberInfos = ((IReflect) canvasProxy).GetMember("id",
                                                                              BindingFlags.Public | BindingFlags.GetProperty |
                                                                              BindingFlags.GetField);
                if(memberInfos.Length > 0)
                {
                    MemberInfo idInfo = memberInfos[0];
                    if(idInfo is PropertyInfo)
                    {
                        return (string)((PropertyInfo)idInfo).GetValue(canvasProxy, BindingFlags.Instance, null, null, null);
                    }
                }
            }
            return string.Empty;
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

        /// <summary>
        /// Returns the number of pixels that the upper left corner of the current element is offset to the left within the offsetParent node.
        /// </summary>
        public int offsetLeft
        {
            get
            {
                return Left;
            }
            set
            {
                Left = value;
            }
        }

        /// <summary>
        /// offsetTop returns the distance of the current element relative to the top of the offsetParent node.
        /// </summary>
        public int offsetTop
        {
            get
            {
                return Top;
            }
            set
            {
                Top = value;
            }
        }

        /// <summary>
        /// Assign parent for the element if element doesn't have any parent yet
        /// </summary>
        /// <param name="parent"></param>
        public void SetParent(object parent)
        {
            if (_parent == null)
            {
                _parent = parent;
                InitializeEvents();
            }
        }

        #region Associative Array Members

        //TODO: add support of associative arrays here

        #endregion

        
    }
}