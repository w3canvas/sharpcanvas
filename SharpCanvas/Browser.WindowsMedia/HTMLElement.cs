using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.JScript;
using SharpCanvas.Interop;
using SharpCanvas.Shared;

namespace SharpCanvas.Browser.Media
{
    /// <summary>
    /// Interaction logic for HTMLElement.xaml
    /// </summary>
    public class HTMLElement : Canvas, IHTMLElementBase
    {
        private readonly List<string> _avoidBubblingEvents = new List<string> { "load" };
        //list of events which shouldn't be bubbled

        private readonly object sync = new object();
        private IHTMLDocument4 _document;

        protected bool _isChanged;
        private object _parent;

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
            set { _parent = value; }
        }

        /// <summary>
        /// Reference to the native document object
        /// </summary>
        public IHTMLDocument4 document
        {
            get { return _document; }
            set { _document = value; }
        }

        #region Constructors

        public HTMLElement(string _name, Canvas _parent)
            : this(_name)
        {
            this._parent = _parent;
        }

        public HTMLElement(string _name)
            : this()
        {
            this.name = _name;
        }

        public HTMLElement()
        {
            InitializeEvents();

            if (this is IHTMLCanvasElement)
            {
                this.Width = (int)(this as IHTMLCanvasElement).width;
                this.Height = (int)(this as IHTMLCanvasElement).height;
            }
            Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
            IsHitTestVisible = true;
        }

        #endregion

        #region Transparent

        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle |= 0x20;
        //        return cp;
        //    }
        //}

        //protected override void OnPaintBackground(PaintEventArgs e)
        //{
        //    //Do not paint background
        //}

        ////Hack
        //public void Redraw()
        //{
        //    RecreateHandle();
        //}

        //private void HTMLElement_Move(object sender, EventArgs e)
        //{
        //    RecreateHandle();
        //}

        #endregion

        #region Events

        protected Dictionary<string, List<ScriptFunction>> _events = new Dictionary<string, List<ScriptFunction>>();

        public Dictionary<string, List<ScriptFunction>> GetEvents()
        {
            return _events;
        }

        private void InitializeEvents()
        {
            //Click += HTMLElement_Click;
            Drop += new DragEventHandler(HTMLElement_Drop);
            DragEnter += new DragEventHandler(HTMLElement_DragEnter);
            DragLeave += new DragEventHandler(HTMLElement_DragLeave);
            DragOver += new DragEventHandler(HTMLElement_DragOver);
            //DoubleClick += HTMLElement_DoubleClick;
            GotFocus += new RoutedEventHandler(HTMLElement_GotFocus);
            KeyDown += new KeyEventHandler(HTMLElement_KeyDown);
            //KeyPress += HTMLElement_KeyPress;
            KeyUp += new KeyEventHandler(HTMLElement_KeyUp);
            Loaded += new RoutedEventHandler(HTMLElement_Loaded);
            //MouseClick += HTMLElement_MouseClick;
            MouseEnter += new MouseEventHandler(HTMLElement_MouseEnter);
            MouseDown += new MouseButtonEventHandler(HTMLElement_MouseDown);
            MouseUp += new MouseButtonEventHandler(HTMLElement_MouseUp);
            //MouseLeftButtonDown += new MouseButtonEventHandler(HTMLElement_MouseDown);
            //MouseLeftButtonUp += new MouseButtonEventHandler(HTMLElement_MouseUp);
            //PreviewMouseDown += new MouseButtonEventHandler(HTMLElement_MouseDown);
            //PreviewMouseUp += new MouseButtonEventHandler(HTMLElement_MouseUp);
            //MouseUp += new MouseButtonEventHandler(HTMLElement_MouseUp);
            MouseMove += new MouseEventHandler(HTMLElement_MouseMove);
            MouseLeave += new MouseEventHandler(HTMLElement_MouseLeave);
            MouseWheel += new MouseWheelEventHandler(HTMLElement_MouseWheel);
            //Scroll += HTMLElement_Scroll;
            //Move += HTMLElement_Move;
        }

        void HTMLElement_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (_events.ContainsKey("mousewheel") && _events["mousewheel"].Count > 0)
                InvokeEvent(sender, e, "mousewheel");
            BubbleEvent(sender, e, "mousewheel");
        }

        void HTMLElement_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_events.ContainsKey("mouseout") && _events["mouseout"].Count > 0)
                InvokeEvent(sender, e, "mouseout");
            BubbleEvent(sender, e, "mouseout");
        }

        void HTMLElement_MouseMove(object sender, MouseEventArgs e)
        {
            //MouseEventArgs abse = new MouseEventArgs(e.Button, e.Clicks, e.X + Left, e.Y + Top, e.Delta);
            MouseEventArgs abse = e;
            if (_events.ContainsKey("mousemove") && _events["mousemove"].Count > 0)
                InvokeEvent(sender, abse, "mousemove");
            //todo: make it more generic and implement it for the rest of events
            //bubble event in WinForms
            if (_parent != null && _parent is IHTMLElementBase)
            {
                //((IHTMLElementBase)_parent).HTMLElement_MouseMove(sender, abse);
            }
            BubbleEvent(sender, e, "mousemove");
        }

        void HTMLElement_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //MouseEventArgs abse = new MouseEventArgs(e.Button, e.Clicks, e.X + Left, e.Y + Top, e.Delta);
            MouseEventArgs abse = e;
            if (_events.ContainsKey("mouseup") && _events["mouseup"].Count > 0)
                InvokeEvent(sender, abse, "mouseup");
            //bubble event in WinForms
            if (_parent != null && _parent is IHTMLElementBase)
            {
                //((IHTMLElementBase)_parent).HTMLElement_MouseUp(sender, abse);
            }
            BubbleEvent(sender, e, "mouseup");
        }

        void HTMLElement_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //MouseEventArgs abse = new MouseEventArgs(e.Button, e.Clicks, e.X + Left, e.Y + Top, e.Delta);
            MouseEventArgs abse = e;
            if (_events.ContainsKey("mousedown") && _events["mousedown"].Count > 0)
                InvokeEvent(sender, abse, "mousedown");
            //bubble event in WinForms
            if (_parent != null && _parent is IHTMLElementBase)
            {
                //((IHTMLElementBase)_parent).HTMLElement_MouseDown(sender, abse);
            }
            BubbleEvent(sender, e, "mousedown");
        }

        void HTMLElement_MouseEnter(object sender, MouseEventArgs e)
        {
            if (_events.ContainsKey("mouseover") && _events["mouseover"].Count > 0)
                InvokeEvent(sender, e, "mouseover");
            BubbleEvent(sender, e, "mouseover");
        }

        void HTMLElement_Loaded(object sender, RoutedEventArgs e)
        {
            if (_events.ContainsKey("load") && _events["load"].Count > 0)
                InvokeEvent(sender, e, "load");
            BubbleEvent(sender, e, "load");
        }

        void HTMLElement_KeyUp(object sender, KeyEventArgs e)
        {
            if (_events.ContainsKey("keyup") && _events["keyup"].Count > 0)
                InvokeEvent(sender, e, "keyup");
            BubbleEvent(sender, e, "keyup");
        }

        void HTMLElement_KeyDown(object sender, KeyEventArgs e)
        {
            if (_events.ContainsKey("keydown") && _events["keydown"].Count > 0)
                InvokeEvent(sender, e, "keydown");
            //bubble event in WinForms
            if (_parent != null && _parent is IHTMLElementBase)
            {
                //((IHTMLElementBase)_parent).HTMLElement_KeyDown(sender, e);
            }
            BubbleEvent(sender, e, "keydown");
        }

        void HTMLElement_GotFocus(object sender, RoutedEventArgs e)
        {
            if (_events.ContainsKey("focus") && _events["focus"].Count > 0)
                InvokeEvent(sender, e, "focus");
            BubbleEvent(sender, e, "focus");
        }

        void HTMLElement_Drop(object sender, DragEventArgs e)
        {
            if (_events.ContainsKey("drag") && _events["drag"].Count > 0)
                InvokeEvent(sender, e, "drag");
            BubbleEvent(sender, e, "drag");
        }

        void HTMLElement_DragOver(object sender, DragEventArgs e)
        {
            if (_events.ContainsKey("dragover") && _events["dragover"].Count > 0)
                InvokeEvent(sender, e, "dragover");
            BubbleEvent(sender, e, "dragover");
        }

        void HTMLElement_DragLeave(object sender, DragEventArgs e)
        {
            if (_events.ContainsKey("dragleave") && _events["dragleave"].Count > 0)
                InvokeEvent(sender, e, "dragleave");
            BubbleEvent(sender, e, "dragleave");
        }

        void HTMLElement_DragEnter(object sender, DragEventArgs e)
        {
            if (_events.ContainsKey("dragenter") && _events["dragenter"].Count > 0)
                InvokeEvent(sender, e, "dragenter");
            BubbleEvent(sender, e, "dragenter");
        }

        public void Redraw()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method allows the registration of event listeners on the event target.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="listener"></param>
        /// <param name="useCapture"></param>
        public void addEventListener(string type, ScriptFunction listener, bool useCapture)
        {
            if (!_events.ContainsKey(type))
            {
                var value = new List<ScriptFunction>();
                value.Add(listener);
                _events.Add(type, value);
            }
            else
            {
                _events[type].Add(listener);
            }
        }

        public void attachEvent(object o1, object o2, bool b1)
        {
            throw new Exception("attachEvent function is not implemented for Windows environment. Use addEventListener instead.");
        }

        /// <summary>
        /// Trigger some javascript event on the object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="eventName"></param>
        public void InvokeEvent(object sender, EventArgs e, string eventName)
        {
            lock (sync)
            {
                foreach (ScriptFunction sf in _events[eventName])
                {
                    Point position = Mouse.GetPosition(this);
                    int x = (int)position.X;
                    int y = (int)position.Y;
                    
                    var argument = new EventArgument(x, y);
                    if (e is KeyEventArgs)
                    {
                        argument.keyCode = (int)(e as KeyEventArgs).Key;
                    }

                    sf.Invoke(this, new object[] { argument });
                }
            }
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

        /// <summary>
        /// Bubbles event to the document object directly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="eventName"></param>
        public void BubbleEvent(object sender, EventArgs e, string eventName)
        {
            if (_document != null && !_avoidBubblingEvents.Contains(eventName))
            {
                FireEvent(_document, "on" + eventName, e);
            }
        }

        /// <summary>
        /// Fires event on the document object
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="eventName"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool FireEvent(IHTMLDocument4 doc, string eventName, EventArgs e)
        {
            Point position = Mouse.GetPosition(this);
            int x = (int)position.X;
            int y = (int)position.Y;
            
            object oEvt = doc.CreateEventObject();
            var evt = (IHTMLEventObj2)oEvt; //cast
            //set various properties of the event here.
            evt.clientX = x;
            evt.clientY = y;

            //fire
            return doc.FireEvent(eventName, oEvt);
        }

        #endregion

        /// <summary>
        /// Reference to the direct parent node
        /// </summary>
        public INode parentNode
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// A NodeList that contains all children of this node. If there are no children, this is a NodeList containing no nodes.
        /// </summary>
        public List<INode> childNodes
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Add child node to the object
        /// </summary>
        /// <param name="child"></param>
        //List<Canvas> children = new List<Canvas>();
        public void appendChild(object child)
        {
            Canvas control = null;
            if (child is Canvas)
            {
                control = (Canvas)child;
            }
            if (child is ICanvasProxy)
            {
                control = ((ICanvasProxy)child).RealObject as Canvas;
            }
            if (control != null)
            {
                this.Children.Add(control);
                //children.Add(control);
                if (control is IHTMLElementBase)
                {
                    ((IHTMLElementBase)control).SetParent(this);
                }
            }
        }

        //protected override int VisualChildrenCount
        //{
        //    get { return children.Count; }
        //}

        //protected override Visual GetVisualChild(int index)
        //{
        //    if (index > children.Count)
        //    {
        //        throw new ArgumentOutOfRangeException("index");
        //    }
        //    return children[index];
        //}

        /// <summary>
        /// Remove child object from the object
        /// </summary>
        /// <param name="child"></param>
        public void removeChild(object child)
        {
            this.RemoveVisualChild(child as Canvas);
        }

        /// <summary>
        /// The Document object associated with this node. This is also the Document object used to create new nodes.
        /// </summary>
        public object ownerDocument
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Returns the element whose ID is specified. 
        /// </summary>
        /// <param name="id">id is a case-sensitive string representing the unique ID of the element being sought. </param>
        /// <returns></returns>
        public object getElementById(string id)
        {
            foreach (IHTMLElementBase o in FindVisualChildren<IHTMLElementBase>(this)) //Image is not HTMLElement!!
            {
                if (o.Identifier == id)
                    return o;
                else
                {
                    object c = o.getElementById(id);
                    if (c != null)
                        return c;
                }
            }
            return null;
        }

        public bool hasOwnProperty(string propName)
        {
            MemberInfo[] member = this.GetType().GetMember(propName);
            if (member != null && member.Length > 0)
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
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// offsetTop returns the distance of the current element relative to the top of the offsetParent node.
        /// </summary>
        public int offsetTop
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Assign parent for the element if element doesn't have any parent yet
        /// </summary>
        /// <param name="parent"></param>
        public void SetParent(object parent)
        {
            throw new NotImplementedException();
        }

        public void mousemove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void mouseup(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void mousedown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void keydown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            throw new NotImplementedException();
        }


        public static IEnumerable<T> FindVisualChildren<T>(IHTMLElementBase depObj) where T : class, IHTMLElementBase
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj as DependencyObject); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj as DependencyObject, i);
                    if (child != null && child is T)
                    {
                        yield return child as T;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child as T))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}
