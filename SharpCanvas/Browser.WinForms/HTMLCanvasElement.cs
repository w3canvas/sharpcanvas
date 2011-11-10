using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using SharpCanvas.Forms;
using SharpCanvas.Host.Browser;
using SharpCanvas.Interop;
using SharpCanvas.Shared;
using _HTML_PAINT_ZORDER = SharpCanvas.Interop._HTML_PAINT_ZORDER;
using _HTML_PAINTER = SharpCanvas.Interop._HTML_PAINTER;
using _HTML_PAINTER_INFO = SharpCanvas.Interop._HTML_PAINTER_INFO;
using IHTMLElement = SharpCanvas.Interop.IHTMLElement;
using IHTMLPainter = SharpCanvas.Interop.IHTMLPainter;
using Image = SharpCanvas.Host.Browser.Image;
using tagPOINT = SharpCanvas.Interop.tagPOINT;
using tagRECT = SharpCanvas.Interop.tagRECT;
using tagSIZE = SharpCanvas.Interop.tagSIZE;
using Timer = System.Timers.Timer;

namespace SharpCanvas.Browser.Forms
{
    [ComVisible(true),
     ClassInterface(ClassInterfaceType.AutoDispatch),
     Guid("20e14abc-5a67-4723-8da4-c1b00e0853d5"),
     ComSourceInterfaces(typeof(global::SharpCanvas.Interop.IHTMLCanvasElement))]
    public class HTMLCanvasElement : HTMLElement, IHTMLPainter, global::SharpCanvas.Interop.IHTMLCanvasElement, IStyleSupported
    {
        #region Fields

        private static readonly object sync = new object();

        private readonly Timer timer = new Timer();
        protected ICanvasRenderingContext2D _canvas = null;
        private Graphics _graphics;
        protected int _height;
        new private bool _isChanged;
        protected bool _isVisible = true;
        private IHTMLPaintSite _paintSite;

        //HtmlCanvasElement
        protected int _width;

        //animation
        private BufferedGraphicsContext GraphicManager;
        private BufferedGraphics ManagedBackBuffer;

        //changes tracking
        private Random randomizer = new Random();

        //back reference to proxy class
        private ICanvasProxy _proxy;
        
        private object _init;
        private ICSSStyleDeclaration _style;

        private object _sync = new object();

        #endregion

        #region Properties

        /// <summary>
        /// Function to execute on init
        /// </summary>
        public object init
        {
            get { return _init; }
            set { _init = value; }
        }

        public IHTMLPaintSite PaintSite
        {
            get { return _paintSite; }
            set { _paintSite = value; }
        }

        #endregion

        #region HtmlCanvasElement Implementation

        public HTMLCanvasElement(ICanvasProxy proxy)
        {
            //by default timer for surface update is disabled
            timer.Enabled = false;
            Paint += HTMLCanvasElement_Paint;
            //Resize += HTMLCanvasElement_Resize;
            Name = "HTMLCanvasElement";
            _proxy = proxy;
            //style
            _style = new CSSStyleDeclaration(this);
            _style.StyleChanged += new StyleChangedHandler(OnStyleChanged);
        }

        /// <summary>
        /// Added for debugging purposes
        /// </summary>
        /// <returns></returns>
        public string GetElementId()
        {
            if (_proxy is IReflect)
            {
                MemberInfo[] memberInfos = ((IReflect)_proxy).GetMember("id",
                                                                              BindingFlags.Public | BindingFlags.GetProperty |
                                                                              BindingFlags.GetField);
                if (memberInfos.Length > 0)
                {
                    MemberInfo idInfo = memberInfos[0];
                    if (idInfo is PropertyInfo)
                    {
                        return (string)((PropertyInfo)idInfo).GetValue(_proxy, BindingFlags.Instance, null, null, null);
                    }
                }
            }
            return string.Empty;
        }

        void OnStyleChanged(string attribute, object value)
        {
            //TODO: make attribute assigning generic
            //provide CSSStyleDeclaration with map: field name - css attribute
            switch(attribute)
            {
                case "left":
                    Left = (int) value;
                    break;
                case "top":
                    Top = (int) value;
                    break;
                case "width":
                    width = (int) value;
                    _canvas.ChangeSize(_width, _height, false);
                    break;
                case "height":
                    height = (int) value;
                    _canvas.ChangeSize(_width, _height, false);
                    break;
                case "z-index": 
                    if(this.parent is UserControl)//will be called if current control already in DOM
                    {
                        UserControl parentUserControl = ((UserControl)parent);
                        parentUserControl.SuspendLayout();
                        int newIndex = 999 - (int)value;
                        parentUserControl.Controls.SetChildIndex(this, newIndex);
                        parentUserControl.ResumeLayout();
                        parentUserControl.Refresh();
                    }
                    break;
            }
        }

        #region IHTMLCanvasElement Members

        public int width
        {
            get { return _width; }
            set
            {
                lock (_sync)
                {
                    _width = value;
                    if (_width > 0 && _height > 0)
                    {

                        Size = new Size(_width, _height);
                        if (_canvas != null && _width != _canvas.GetWidth())
                        {
                            _canvas.ChangeSize(width, height, true);
                            //reflect size changed in the style
                            _style.SuspendEvnets();
                            _style.width = _width;
                            _style.ResumeEvents();
                        }
                    }
                }
            }
        }

        public int height
        {
            get { return _height; }
            set
            {
                lock (_sync)
                {
                    _height = value;
                    if (_width > 0 && _height > 0)
                    {

                        Size = new Size(_width, _height);
                        if (_canvas != null && _height != _canvas.GetHeight())
                        {
                            _canvas.ChangeSize(width, height, true);
                            //reflect size changed in the style
                            _style.SuspendEvnets();
                            _style.height = _height;
                            _style.ResumeEvents();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// For WinForm use, or internal use in case of browser hosting
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void setAttribute(string name, object value)
        {
            if (name == "style" && value is string)
            {
                _style.cssText = (string)value;
            }
        }

        public ICSSStyleDeclaration style
        {
            get { return _style; }
        }

        /// <summary>
        /// TODO: implement
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public string toDataURL(string type, params object[] args)
        {
           if(string.IsNullOrEmpty(type))
           {
               type = SharpCanvas.Forms.Image.DEFAULT_TYPE;
           }
            Bitmap bitmap = _canvas.GetBitmap();
            string base64String = Utils.ImageToBase64(bitmap, Utils.ImageFormatFromMediaType(type));
            return type + SharpCanvas.Forms.Image.BASE64 + base64String;
        }

        /// <summary>
        /// Returns an object that exposes an API for drawing on the canvas.
        /// Returns null if the given context ID is not supported.
        /// </summary>
        /// <param name="contextId">always should be equal to 2d</param>
        /// <returns></returns>
        public object getContext(string contextId)
        {
            if (contextId == "2d")
            {
                if (_canvas == null)
                {
                    if (width <= 0)
                    {
                        width = 300;
                    }
                    if (height <= 0)
                    {
                        height = 150;
                    }
                    lock (sync)
                    {
                        CreateCanvas();
                    }
                }
                // If the getContext() method has already been invoked on this element for the same contextId, 
                // return the same object as was returned that time
                return _canvas;
            }
            return null;
        }

        public ICanvasRenderingContext2D getCanvas()
        {
            return (ICanvasRenderingContext2D) getContext("2d");
        }

        public ICanvasProxy GetProxy()
        {
            return _proxy;
        }

        /// <summary>
        /// Requests drawing execution on next WM_PAINT event for the control
        /// </summary>
        public void RequestDraw()
        {
            Paint += HTMLCanvasElement_Paint;
            ////force raising Paint event in order to keep order of rendering of elements
            this.Invalidate(true);
            _isChanged = true;
            //ForceDraw();
        }

        #endregion

        #region IStyleSupported Members

        /// <summary>
        /// Sets the attribute value and update position of the child element on the page - workaround for correct positioning on the page
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="element">Child element, usually _canvasElt of the proxy-class</param>
        public void setAttribute(string name, object value, IHTMLElement element)
        {
            setAttribute(name, value);
            if (element != null)
            {
                element.style.setAttribute("top", Top.ToString(), 0);
                element.style.setAttribute("left", Left.ToString(), 0);
            }
        }

        #endregion

        /// <summary>
        /// Internal method for creating new CanvasRenderingContext2D instance. Different for WinForm and WPF versions.
        /// </summary>
        protected virtual void CreateCanvas()
        {
            if (width > 0 && height > 0)
            {
                var bmp = new Bitmap(_width, _height);
                Graphics tmp = Graphics.FromImage(bmp);
                _canvas = new ContextProxy(tmp, bmp, new Pen(Color.Black, 1),
                                                       new Fill(Color.Black), _isVisible, _proxy);
                _canvas.OnPartialDraw += ReDraw;
            }
        }

        /// <summary>
        /// Refresh buffer on resize event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HTMLCanvasElement_Resize(object sender, EventArgs e)
        {
            if (GraphicManager != null)
            {
                //GraphicManager.Dispose();
            }
        }

        /// <summary>
        /// Updates surface of the control in case when image was changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_canvas != null && _canvas.IsVisible) //redrawing make sense only in case when element is visible
            {
                bool isLocked = !Monitor.TryEnter(sync);
                //if drawing resources is not checked out by another process
                if (!isLocked)
                {
                    //if redrawing is necessary
                    if (_isChanged)
                    {
                        ForceDraw();
                        _isChanged = false;
                    }
                    Monitor.Exit(sync);
                }
            }
        }

        /// <summary>
        /// Draws the latest version of the image to the buffer and flush the image to the control's surface
        /// </summary>
        public void ForceDraw()
        {
            tagRECT bound = GetEmptyBound();
            tagRECT update = GetEmptyUpdate();
            if (ManagedBackBuffer != null)
            {
                //if standalone env.
                if (_paintSite == null)
                {
                    //ManagedBackBuffer.Graphics.Clear(Color.White);
                    ManagedBackBuffer.Graphics.Clear(BackColor);
                    //ManagedBackBuffer.Graphics.Clear(Color.Transparent);
                }
                Draw(ManagedBackBuffer.Graphics, bound, update);
                //ManagedBackBuffer.Graphics.DrawString(ManagedBackBuffer.Graphics.VisibleClipBounds.Width.ToString(), new Font("Arial", 5), new SolidBrush(Color.Black), 0, 0);
                ManagedBackBuffer.Render(CreateGraphics());
            }
        }

        /// <summary>
        /// We have to subscribe this event because of WinForms version, 
        /// this method will be called instead of Draw method in IE env.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HTMLCanvasElement_Paint(object sender, PaintEventArgs e)
        {
            tagRECT bound = GetEmptyBound();
            tagRECT update = GetEmptyUpdate();

            //we have to initialize GraphicManager here because we don't have timer in IE environment            
            InitGraphicManager(e);

            //we don't want to receive OnPaint events notifications anymore - we have timer to update the surface (animation support)
            Paint -= HTMLCanvasElement_Paint;

            //if standalone env.
            if (_paintSite == null)
            {
                //e.Graphics.Clear(Color.Transparent);
                //e.Graphics.Clear(Color.White);
                e.Graphics.Clear(BackColor);
            }
            //draw directly to the surface
            Draw(e.Graphics, bound, update);
        }

        /// <summary>
        /// Get rectagnle area equal to current control's size
        /// </summary>
        /// <returns></returns>
        private tagRECT GetEmptyUpdate()
        {
            var update = new tagRECT();
            update.left = 0;
            update.top = 0;
            update.right = width;
            update.bottom = height;
            return update;
        }

        /// <summary>
        /// Get zero-initialized rectangle
        /// </summary>
        /// <returns></returns>
        private tagRECT GetEmptyBound()
        {
            var bound = new tagRECT();
            bound.left = 0;
            bound.top = 0;
            return bound;
        }

        /// <summary>
        /// Initialize DoubleBuffering as well as re-drawing timer
        /// </summary>
        /// <param name="e"></param>
        private void InitGraphicManager(PaintEventArgs e)
        {
            if (GraphicManager == null)
            {
                SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
                SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                SetStyle(ControlStyles.ResizeRedraw, true);
                SetStyle(ControlStyles.UserPaint, true);
                SetStyle(ControlStyles.SupportsTransparentBackColor, true);

                GraphicManager = BufferedGraphicsManager.Current;
                GraphicManager.MaximumBuffer =
                    new Size(width + 1, height + 1);
                ManagedBackBuffer =
                    GraphicManager.Allocate(e.Graphics, DisplayRectangle);
                //we use timer to display animation correctly
                timer.Enabled = true;
                timer.Interval = 50;
                timer.Elapsed += timer_Elapsed;
            }
        }

        #endregion

        #region Implementation of IHTMLPainter

        /// <summary>
        /// IE will, in fact, call Draw() more than once in drawing the element, as it paints the element rectangle in successive horizontal bands. 
        /// For this reason, it is best to prepare an off-screen bitmap image before element invalidation, so that successive calls to Draw() 
        /// deal with an unchanging graphic. When IE calls Draw(), the drawing requirements dictated by IE are obtained through GetDrawInfo(), 
        /// and any necessary image transformation and clipping can then be applied before rendering with Graphics.DrawImage().
        /// </summary>
        /// <param name="rcBounds"></param>
        /// <param name="rcUpdate"></param>
        /// <param name="lDrawFlags"></param>
        /// <param name="hdc"></param>
        /// <param name="pvDrawObject"></param>
        public void Draw(tagRECT rcBounds, tagRECT rcUpdate, int lDrawFlags, IntPtr hdc, IntPtr pvDrawObject)
        {
            Paint -= HTMLCanvasElement_Paint;
            _graphics = Graphics.FromHdc(hdc);
            _graphics.CompositingMode = CompositingMode.SourceOver;

            // Apply any scaling, etc. to the output.
            _HTML_PAINT_DRAW_INFO info;
            _paintSite.GetDrawInfo(
                (int)_HTML_PAINT_DRAW_INFO_FLAGS.HTMLPAINT_DRAWINFO_XFORM |
                (int)_HTML_PAINT_DRAW_INFO_FLAGS.HTMLPAINT_DRAWINFO_UPDATEREGION,
                out info);
            var xform = new Matrix(
                info.xform.eM11, info.xform.eM12,
                info.xform.eM21, info.xform.eM22,
                info.xform.eDx - rcBounds.left, info.xform.eDy - rcBounds.top);
            _graphics.Transform = xform;

            // Update clipping region.
            var clip = new Region();
            if (info.hrgnUpdate != IntPtr.Zero)
            {
                Region updateclip = Region.FromHrgn(info.hrgnUpdate);
                clip.Intersect(updateclip);
                clip.Translate(rcBounds.left, rcBounds.top);
            }
            _graphics.SetClip(clip, CombineMode.Replace);

            if (init != null) //run only once
            {
                _init.GetType().InvokeMember("", BindingFlags.InvokeMethod, null, _init,
                                             new object[] { });
                _init = null;
            }

            Draw(_graphics, rcBounds, rcUpdate);
            _graphics.Dispose();
        }

        /// <summary>
        /// Refresh image on resize event.
        /// </summary>
        /// <param name="size"></param>
        public void OnResize(tagSIZE size)
        {
            //_isChanged = true;
           // _paintSite.InvalidateRegion(IntPtr.Zero);
        }

        /// <summary>
        /// Called by CanvasRenderingContext2D in order to notify parent objects 
        /// that some changes were made in image, so redrawing should be performed if necessary.
        /// </summary>
        public void ReDraw()
        {
            //if this is IE
            if (_paintSite != null)
            {
                var rect = new tagRECT();
                rect.top = 0;
                rect.bottom = Top + height;
                rect.left = 0;
                rect.right = Left + width;
                _paintSite.InvalidateRect(ref rect);
            }
            //mark current canvas element as changed
            _isChanged = true;
        }

        public void GetPainterInfo(out _HTML_PAINTER_INFO pInfo)
        {
            pInfo.lFlags = (int)(_HTML_PAINTER.HTMLPAINTER_TRANSPARENT |
                                  _HTML_PAINTER.HTMLPAINTER_NOPHYSICALCLIP
                                  | _HTML_PAINTER.HTMLPAINTER_SUPPORTS_XFORM | _HTML_PAINTER.HTMLPAINTER_NOSAVEDC);
            // Possibly also: | _HTML_PAINTER.HTMLPAINTER_HITTEST.

            pInfo.lZOrder = (int)_HTML_PAINT_ZORDER.HTMLPAINT_ZORDER_REPLACE_ALL;
            pInfo.iidDrawObject = Guid.Empty; // No drawing object; using GDI+.
            pInfo.rcExpand.left = pInfo.rcExpand.right =
                                  pInfo.rcExpand.top = pInfo.rcExpand.bottom = 0;
        }

        /// <summary>
        /// Not called unless pInfo.lFlags included _HTML_PAINTER.HTMLPAINTER_HITTEST.
        /// </summary>
        public void HitTestPoint(tagPOINT pt, out int pbHit, out int plPartID)
        {
            pbHit = 0;
            plPartID = 0;
        }

        /// <summary>
        /// Get necessary part of the whole image and draw it on the specified surface
        /// </summary>
        /// <param name="graphics">surface to draw</param>
        /// <param name="rcBounds">allowed rectangle to draw in</param>
        /// <param name="rcUpdate">rectangle to update</param>
        private void Draw(Graphics graphics, tagRECT rcBounds, tagRECT rcUpdate)
        {
            Bitmap bitmap;
            if (_canvas != null)
            {
                int x = rcUpdate.left;
                int y = rcUpdate.top;
                int x1 = rcUpdate.left - rcBounds.left;
                int y1 = rcUpdate.top - rcBounds.top;
                int width1 = rcUpdate.right - rcUpdate.left;
                int height1 = rcUpdate.bottom - rcUpdate.top;

                lock (sync)
                {
                    bitmap = _canvas.GetBitmap();
                    if (bitmap.Width < width1)
                    {
                        width1 = bitmap.Width;
                    }
                    if (bitmap.Height < height1)
                    {
                        height1 = bitmap.Height;
                    }
                    var rect = new Rectangle(x1,
                                             y1,
                                             width1,
                                             height1);
                    graphics.DrawImage(bitmap, x, y,
                                       rect,
                                       GraphicsUnit.Pixel);
                    //graphics.DrawString(string.Format("bW = {0}, gW = {1}", bitmap.Width, graphics.VisibleClipBounds.Width), new Font("Arial", 5), new SolidBrush(Color.Black), x1, y1);
                }
            }
        }

        #endregion


    }
}