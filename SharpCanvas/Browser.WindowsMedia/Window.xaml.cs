using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.JScript;
using SharpCanvas.Host;
using SharpCanvas.Interop;
using SharpCanvas.Shared;
using Path = System.Windows.Shapes.Path;
using UserControl = System.Windows.Controls.UserControl;

namespace SharpCanvas.Browser.Media
{
    /// <summary>
    /// Represents window element, but in standalone environment
    /// </summary>
    public partial class Window : Canvas, IWindow, ITimeout
    {
        #region Private Variables

        private const int WM_ERASEBACKGROUND = 0x0014;

        private Document _document;
        private int _innerHeight;
        private int _innerWidth;
        // FIXME: Abstract to Host.
        private ITimeout _timeout;
        //private SaveFileDialog _dlgSaveFile;
        private bool isDialogOpen;
        private string _src;
        private IDocument _parent;

        #endregion

        public Window(IDocument parentDocument)
            : this()
        {
            _parent = parentDocument;
        }

        private Canvas element;
        public Window()
        {
            InitializeComponent();
            //
            // document
            //
            _document = new Document(this);
            _document.Width = _innerWidth;
            _document.Height = _innerHeight;
            this.Children.Add(_document);
            element = new Canvas();

            //LineGeometry myLineGeometry = new LineGeometry();
            //myLineGeometry.StartPoint = new Point(10, 20);
            //myLineGeometry.EndPoint = new Point(100, 130);

            //Path myPath = new Path();
            //myPath.Stroke = Brushes.Black;
            //myPath.StrokeThickness = 1;
            //myPath.Data = myLineGeometry;
            //element.Children.Add(myPath);
            //this.Children.Add(element);
            this.Loaded += new RoutedEventHandler(Window_Loaded);
            //MouseUp += new MouseButtonEventHandler(Window_MouseUp);
        }

        //void Window_MouseUp(object sender, MouseButtonEventArgs e)
        //{
            
        //}

        //protected override int VisualChildrenCount
        //{
        //    get { return 1; }
        //}

        //protected override Visual GetVisualChild(int index)
        //{
        //    if (index > 0)
        //    {
        //        throw new ArgumentOutOfRangeException("index");
        //    }

        //    //return _document;
        //    return element;
        //}

        
        #region IWindow members

        private ILocation _location;

        /// <summary>
        /// The value of the location attribute MUST be the Location object that represents the window's current location.
        /// </summary>
        public ILocation location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
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

        public object globalScope
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        #endregion 

        public object onload { get; set; }

        public int innerHeight
        {
            get { return _innerHeight; }
            set
            {
                _innerHeight = value;
                if (_document != null)
                    _document.Height = _innerHeight;
                UpdateClientRect();
            }
        }

        public int innerWidth
        {
            get { return _innerWidth; }
            set
            {
                _innerWidth = value;
                if(document != null)
                    _document.Width = _innerWidth;
                UpdateClientRect();
            }
        }

        /// <summary>
        /// Reference to parent window
        /// </summary>
        public IWindow parentWindow
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Left position of the control
        /// </summary>
        public int Left
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Top position of the control
        /// </summary>
        public int Top
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public void setAttribute(object name, object value)
        {
            string strName = name.ToString();
            string strValue = value.ToString();
            switch (strName.ToLower())
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

        /// <summary>
        /// Assign document to the window
        /// TODO: this method is obsolete. Remove it.
        /// </summary>
        /// <param name="doc"></param>
        public void assign(IDocument doc)
        {
            throw new NotImplementedException();
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
            this.Width = _innerWidth;
            this.Height = _innerHeight;
        }

        /// <summary>
        /// Assign document to the window
        /// </summary>
        /// <param name="doc"></param>
        public void assign(Document doc)
        {
            doc.parent = this;
            _document = doc;
            //if (!Controls.Contains(doc))
            //    appendChild(doc);
        }

        /// <summary>
        /// In this method we catch WM_ERASEBACKGROUND event, which indicates that all childrens or
        /// some part of them should be redrawn. And do redraw.
        /// We can't use WM_PAINT for this purpose because WM_PAINT triggers all the time re-paint occurs.
        /// </summary>
        /// <param name="m"></param>
        //protected override void WndProc(ref Message m)
        //{
        //    base.WndProc(ref m);

        //    if (m.Msg == WM_ERASEBACKGROUND)
        //    {
        //        RedrawChildren();
        //        m.Result = new IntPtr(1);
        //        return;
        //    }
        //}

        /// <summary>
        /// Redraw visible only childrens (take in count z-index of the children)
        /// </summary>
        public void RedrawChildren()
        {
            if (document != null && document.body != null)
            {
                var toRedraw = new Dictionary<int, List<global::SharpCanvas.Interop.IHTMLCanvasElement>>();
                int maxZIndex = 0;
                //build the order to redraw
                UserControl body = document.body as UserControl;
                //foreach (global::SharpCanvas.Interop.IHTMLCanvasElement element in body.Controls)
                //{
                //    if (element.IsVisible)
                //    {
                //        if (!toRedraw.ContainsKey(element.ZIndex))
                //        {
                //            toRedraw.Add(element.ZIndex, new List<global::SharpCanvas.Interop.IHTMLCanvasElement>());
                //        }
                //        toRedraw[element.ZIndex].Add(element);
                //        if (maxZIndex < element.ZIndex)
                //        {
                //            maxZIndex = element.ZIndex;
                //        }
                //    }
                //}
                //request redraw
                for (int i = 0; i <= maxZIndex; i++)
                {
                    if (toRedraw.ContainsKey(i))
                    {
                        foreach (global::SharpCanvas.Interop.IHTMLCanvasElement element in toRedraw[i])
                        {
                            element.RequestDraw();
                        }
                    }
                }
            }
        }

        public IEventModel eventModel
        {
            get { throw new NotImplementedException(); }
        }


        void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Loaded");
            InvokeOnload();
        }

        /// <summary>
        /// Invokes onload function if specified and remove reference to the function
        /// </summary>
        public void InvokeOnload()
        {
            if (this.onload != null) //run only once
            {
                var function = (ScriptFunction)this.onload;
                this.onload = null;
                function.Invoke(this, new object[] { });
                (document as Canvas).UpdateLayout();
            }
        }

        private void location_OnSaveFile(byte[] data)
        {
            if (!isDialogOpen)
            {
                //DialogResult dialogResult = _dlgSaveFile.ShowDialog();
                //isDialogOpen = true;
                //if (DialogResult.OK == dialogResult)
                //{
                //    File.WriteAllBytes(_dlgSaveFile.FileName, data);
                //}
                //isDialogOpen = false;
            }
        }

        #endregion

        #region HTMLElement methods

        /// <summary>
        /// We can't derive the class from the HTMLElement class, because it should be derived from the Form
        /// so HTMLElement functionality here is just copy of HTMLElement class functionality
        /// </summary>
        private readonly Dictionary<string, List<ScriptFunction>> events =
            new Dictionary<string, List<ScriptFunction>>();

        /// <summary>
        /// An attribute containing a unique name used to refer to this Window object.
        /// Need to describe how this could come from a containing element. 
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// An attribute containing a reference to the topmost Window object in the hierarchy that contains this object.
        /// </summary>
        public object top { get; set; }

        /// <summary>
        /// referencing <html:frame>, <html:iframe>, <html:object>, <svg:foreignObject>,
        /// <svg:animation> or other embedding point, or null if none
        /// </summary>
        public object frameElement { get; set; }

        ///// <summary>
        ///// An attribute containing a reference to Window object that contains this object.
        ///// </summary>
        //public UserControl parent { get; set; }

        /// <summary>
        /// Add child node to the object
        /// </summary>
        /// <param name="child"></param>
        public void appendChild(UserControl child)
        {
            this.AddVisualChild(child);
        }

        /// <summary>
        /// Remove child object from the object
        /// </summary>
        /// <param name="child"></param>
        public void removeChild(UserControl child)
        {
            this.RemoveVisualChild(child);
        }

        #endregion

        #region Events

        

        /// <summary>
        /// This method allows the registration of event listeners on the event target.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="listener"></param>
        /// <param name="useCapture"></param>
        public void addEventListener(string type, ScriptFunction listener, bool useCapture)
        {
            if (!events.ContainsKey(type))
            {
                var value = new List<ScriptFunction>();
                value.Add(listener);
                events.Add(type, value);
            }
            else
            {
                events[type].Add(listener);
            }
        }

        /// <summary>
        /// Trigger some javascript event on the object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="eventName"></param>
        public void InvokeEvent(object sender, EventArgs e, string eventName)
        {
            foreach (ScriptFunction sf in events[eventName])
            {
                sf.GetType().InvokeMember("", BindingFlags.InvokeMethod, null, sf,
                                          new object[] { e });
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

        public INavigator navigator
        {
            get { throw new NotImplementedException(); }
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

        #region Implementation of INode

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
