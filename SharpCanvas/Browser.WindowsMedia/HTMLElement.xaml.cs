using System;
using System.Collections.Generic;
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

namespace SharpCanvas.Browser.Media
{
    /// <summary>
    /// Interaction logic for HTMLElement.xaml
    /// </summary>
    public class HTMLElement : UserControl, IHTMLElementBase
    {
        private readonly List<string> _avoidBubblingEvents = new List<string> { "load" };
        //list of events which shouldn't be bubbled

        private readonly object sync = new object();
        private IHTMLDocument4 _document;

        protected bool _isChanged;
        private object _parent;

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

        public HTMLElement(string _name, UserControl _parent)
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
            InitializeComponent();

            InitializeEvents();

            if (this is IHTMLCanvasElement)
            {
                this.Width = (int) (this as IHTMLCanvasElement).width;
                this.Height = (int) (this as IHTMLCanvasElement).height;
            }
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
            //DragDrop += HTMLElement_DragDrop;
            //DragEnter += HTMLElement_DragEnter;
            //DragLeave += HTMLElement_DragLeave;
            //DragOver += HTMLElement_DragOver;
            //DoubleClick += HTMLElement_DoubleClick;
            //Enter += HTMLElement_Enter;
            //KeyDown += HTMLElement_KeyDown;
            //KeyPress += HTMLElement_KeyPress;
            //KeyUp += HTMLElement_KeyUp;
            //Load += HTMLElement_Load;
            //MouseClick += HTMLElement_MouseClick;
            //MouseHover += HTMLElement_MouseHover;
            //MouseDown += HTMLElement_MouseDown;
            //MouseUp += HTMLElement_MouseUp;
            //MouseMove += HTMLElement_MouseMove;
            //MouseLeave += HTMLElement_MouseLeave;
            //MouseWheel += HTMLElement_MouseWheel;
            //Scroll += HTMLElement_Scroll;
            //Move += HTMLElement_Move;
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
                    //int x = Cursor.Position.X;
                    //int y = Cursor.Position.Y;
                    //if (e is MouseEventArgs)
                    //{
                    //    x = (e as MouseEventArgs).X;
                    //    y = (e as MouseEventArgs).Y;
                    //}
                    //var argument = new EventArgument(x, y);
                    //if (e is KeyEventArgs)
                    //{
                    //    argument.keyCode = (int)((KeyEventArgs)e).KeyCode;
                    //}

                    //sf.Invoke(this, new object[] { argument });
                }
            }
        }

        /// <summary>
        /// Copy events subscriptions from events collection to the target object
        /// </summary>
        /// <param name="target"></param>
        /// <param name="events"></param>
        public void CloneEvents(IHTMLElementBase target, Dictionary<string, List<ScriptFunction>> events)
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
            //int x = Cursor.Position.X;
            //int y = Cursor.Position.Y;
            //if (e is MouseEventArgs)
            //{
            //    x = (e as MouseEventArgs).X;
            //    y = (e as MouseEventArgs).Y;
            //}
            ////object dummy = null;
            ////object oEvt = doc.CreateEventObject(ref dummy);
            //object oEvt = doc.CreateEventObject();
            //var evt = (IHTMLEventObj2)oEvt; //cast
            ////set various properties of the event here.
            //evt.clientX = x;
            //evt.clientY = y;

            ////fire
            //return doc.FireEvent(eventName, oEvt);
            return false;
        }

        #endregion

        /// <summary>
        /// Add child node to the object
        /// </summary>
        /// <param name="child"></param>
        public void appendChild(object child)
        {
            UserControl control = null;
            if (child is UserControl)
            {
                control = (UserControl)child;
            }
            if (child is ICanvasProxy)
            {
                control = ((ICanvasProxy)child).RealObject as UserControl;
            }
            if (control != null)
            {
                this.AddVisualChild(control);
                if (control is IHTMLElementBase)
                {
                    ((IHTMLElementBase)control).parent = this;
                }
            }
        }

        /// <summary>
        /// Remove child object from the object
        /// </summary>
        /// <param name="child"></param>
        public void removeChild(object child)
        {
            this.RemoveVisualChild(child as UserControl);
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

        public void HTMLElement_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        public void HTMLElement_MouseUp(object sender, MouseEventArgs e)
        {
            
        }

        public void HTMLElement_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        public void HTMLElement_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        public static IEnumerable<T> FindVisualChildren<T>(IHTMLElementBase depObj) where T: class, IHTMLElementBase
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
