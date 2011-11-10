// FIXME: Merge with WinForms Version,
// move platform specific code into the
// RenderingContext implementations.

// This is an implementation of the HTMLCanvasElement and IHTMLPainter.
// FIXME: Separate the IHTMLPainter.

using System;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using SharpCanvas.Interop;
using SharpCanvas.Media;
using SharpCanvas.Host.Browser;
using SharpCanvas.Shared;
using _HTML_PAINT_ZORDER = SharpCanvas.Interop._HTML_PAINT_ZORDER;
using _HTML_PAINTER = SharpCanvas.Interop._HTML_PAINTER;
using _HTML_PAINTER_INFO = SharpCanvas.Interop._HTML_PAINTER_INFO;
using Brushes = System.Windows.Media.Brushes;
using Color = System.Windows.Media.Color;
using IHTMLElement = SharpCanvas.Interop.IHTMLElement;
using IHTMLPainter = SharpCanvas.Interop.IHTMLPainter;
using Matrix = System.Drawing.Drawing2D.Matrix;
using Pen = System.Windows.Media.Pen;
using Point = System.Windows.Point;
using Rectangle = System.Drawing.Rectangle;
using tagPOINT = SharpCanvas.Interop.tagPOINT;
using tagRECT = SharpCanvas.Interop.tagRECT;
using tagSIZE = SharpCanvas.Interop.tagSIZE;

namespace SharpCanvas.Browser.Media
{
    [ComVisible(true),
     ClassInterface(ClassInterfaceType.AutoDispatch),
     Guid("35a22abc-5a67-4723-8da4-c1b00e0853f4"),
     ComSourceInterfaces(typeof(global::SharpCanvas.Interop.IHTMLCanvasElement))]
    public class HTMLCanvasElement : HTMLElement, IHTMLPainter, global::SharpCanvas.Interop.IHTMLCanvasElement, IStyleSupported
    {
        #region Fields

        #region Delegates

        public delegate void CreateCanvasHandler();

        #endregion

        //animation
        private static readonly object sync = new object();
        private readonly Regex regexDisplay = new Regex(@"display:\s*none");
        private readonly Regex rLeft = new Regex(@"left:(?<left>\d+)");
        private readonly Regex rTop = new Regex(@"top:(?<top>\d+)");
        private readonly DispatcherTimer timer = new DispatcherTimer();
        protected ICanvasRenderingContext2D _canvas;
        private Graphics _graphics;
        protected int _height;
        new private bool _isChanged;
        protected bool _isVisible = true;
        private IHTMLPaintSite _paintSite;

        //HtmlCanvasElement
        protected int _width;
        private int _zIndex;
        

        #endregion

        #region Properties

        private object _init;

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

        public int ZIndex
        {
            get { return _zIndex; }
            set { _zIndex = value; }
        }

        public bool IsVisible
        {
            get { return _isVisible; }
        }

        #endregion

        #region HtmlCanvasElement Implementation

        private Canvas canvas;

        public HTMLCanvasElement()
        {
            //by default timer for surface update is disabled
            timer.Stop();
            Loaded += new System.Windows.RoutedEventHandler(HTMLCanvasElement_Loaded);
            Name = "HTMLCanvasElement";

            //dv = new DrawingVisual();

            //DrawingContext dc = dv.RenderOpen();

            //Rect rect = new Rect(new Point(75, 75), new Point(25, 50));

            //dc.DrawRectangle(Brushes.Yellow, new Pen(Brushes.Navy, 2), rect);

            //dc.DrawEllipse(Brushes.Wheat, new Pen(Brushes.Brown, 3), new Point(200, 150), 25, 50);

            //dc.Close();

            //AddVisualChild(dv);

            canvas = new System.Windows.Controls.Canvas();
            canvas.Width = _width;
            canvas.Height = _height;
            this.Children.Add(canvas);
        }


        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index > 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            return canvas;
        }

       

        #region IHTMLCanvasElement Members

        public int width
        {
            get { return _width; }
            set
            {
                _width = value;
                if (_width > 0 && _height > 0)
                {
                    //Size = new Size(_width, _height);
                    this.Width = _width;
                    this.Height = _height;
                    canvas.Width = _width;
                    canvas.Height = _height;
                    //if (_canvas != null)
                    //    _canvas.cutImage(0, 0, (int)_width, (int)_height);
                }
            }
        }

        public int height
        {
            get { return _height; }
            set
            {
                _height = value;
                if (_width > 0 && _height > 0)
                {
                    //Size = new Size(_width, _height);
                    this.Width = _width;
                    this.Height = _height;
                    canvas.Width = _width;
                    canvas.Height = _height;
                    //if (_canvas != null)
                    //    _canvas.cutImage(0, 0, (int)_width, (int)_height);
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
            if (name == "style")
            {
                if (regexDisplay.IsMatch(value.ToString()))
                {
                    _isVisible = false;
                    //canvas.Visibility = Visibility.Hidden;
                    this.Children.Remove(canvas);
                }

                if (rTop.IsMatch(value.ToString()))
                {
                    int y = Convert.ToInt32(rTop.Match(value.ToString()).Groups["top"].Value);
                }

                if (rLeft.IsMatch(value.ToString()))
                {
                    int x = Convert.ToInt32(rLeft.Match(value.ToString()).Groups["left"].Value);
                }

                string regexZIndex = @"z-index:\s*(?<index>\d+)";
                var rZIndex = new Regex(regexZIndex);
                if (rZIndex.IsMatch(value.ToString()))
                {
                    int zIndex = 0;
                    int.TryParse(rZIndex.Match(value.ToString()).Groups["index"].Value, out zIndex);
                    _zIndex = zIndex;
                    Canvas.SetZIndex(this, _zIndex);
                }
            }
        }

        public ICSSStyleDeclaration style
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// TODO: implement
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public string toDataURL(string type, params object[] args)
        {
            //var m = new MemoryStream();
            //i.Save(m, System.Drawing.Imaging.ImageFormat.Png);
            //return Convert.ToBase64String(m.ToArray());
            return type;
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
                lock (sync)
                {
                    CreateCanvas();
                }
                return _canvas;
            }
            return null;
        }

        public ICanvasRenderingContext2D getCanvas()
        {
            return null;
        }

        public ICanvasProxy GetProxy()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Requests drawing execution on next WM_PAINT event for the control
        /// </summary>
        public void RequestDraw()
        {
            //Paint += HTMLCanvasElement_Paint;
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
                //element.style.setAttribute("top", Top.ToString(), 0);
                //element.style.setAttribute("left", Left.ToString(), 0);
            }
        }

        #endregion

        /// <summary>
        /// Internal method for creating new CanvasRenderingContext2D instance. Different for WinForm and WPF versions.
        /// </summary>
        public void CreateCanvas()
        {
            if (width > 0 && height > 0)
            {
                _canvas = new CanvasRenderingContext2D(canvas, this, _isVisible, this);
                _canvas.OnPartialDraw += ReDraw;
                //this.Children.Add(((CanvasRenderingContext2D)_canvas).GetCanvas());
            }
        }

        /// <summary>
        /// Draws the latest version of the image to the buffer and flush the image to the control's surface
        /// </summary>
        public void ForceDraw()
        {
            //this.Children.Add(((CanvasRenderingContext2D)_canvas).GetCanvas());
            //canvas.UpdateLayout();
            //LineGeometry myLineGeometry = new LineGeometry();
            //myLineGeometry.StartPoint = new Point(10, 20);
            //myLineGeometry.EndPoint = new Point(100, 130);

            //Path myPath = new Path();
            //myPath.Stroke = Brushes.Black;
            //myPath.StrokeThickness = 1;
            //myPath.Data = myLineGeometry;
            //canvas.Children.Add(myPath);
            
            //_canvas.fillStyle = "#FF8B1E";
            //_canvas.clearRect(0, 0, width, height);
            //_canvas.fillRect(0, 0, 100, 100);
            //_canvas.fill();
            //_canvas.GetBitmap().Save(@"c:\bitmap.bmp");
            //_isChanged = true;
            
            //PathFigure f = new PathFigure();

            //f.Segments.Add(new PolyLineSegment(new[] { new Point(0, 0), new Point(100, 0), new Point(100, 100), new Point(0, 100) }, true));
            //PathGeometry g = new PathGeometry();

            //g.Figures.Add(f);
            //GeometryGroup gg = new GeometryGroup();
            //gg.Children.Add(g);
            //System.Windows.Shapes.Path p = new System.Windows.Shapes.Path();
            //p.Data = gg;
            //p.Stroke = new SolidColorBrush(Color.FromRgb(255, 139, 30));
            //canvas.Children.Add(p);
        }


        /// <summary>
        /// Updates surface of the control in case when image was changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            if (_canvas != null && _canvas.IsVisible) //redrawing make sense only in case when element is visible
            {
                bool isLocked = !Monitor.TryEnter(sync);
                //if drawing resources is not checked out by another process
                if (!isLocked)
                {
                    //if redrawing is necessary
                    //if (_isChanged)
                    {
                        ForceDraw();
                    }
                    _isChanged = false;
                    Monitor.Exit(sync);
                }
            }
        }

        void HTMLCanvasElement_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            //we use timer to display animation correctly
            timer.Interval = TimeSpan.FromMilliseconds(30);
            timer.Tick += timer_Tick;
            timer.Start();
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
        // FIXME: Move this to an IE specific HTMLCanvasElement implementation.
        public void Draw(tagRECT rcBounds, tagRECT rcUpdate, int lDrawFlags, IntPtr hdc, IntPtr pvDrawObject)
        {
            //Paint -= HTMLCanvasElement_Paint;
            _graphics = Graphics.FromHdc(hdc);
            //_graphics.CompositingMode = CompositingMode.SourceOver;

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
            //_graphics.SetClip(clip, CombineMode.Replace);

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
            _paintSite.InvalidateRegion(IntPtr.Zero);
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
                //rect.bottom = Top + height;
                rect.bottom = height;
                rect.left = 0;
                rect.right = width;
                //rect.right = Left + width;
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
                //_canvas.commit();
                bitmap = _canvas.GetBitmap();
                int x = rcUpdate.left;
                int y = rcUpdate.top;
                int x1 = rcUpdate.left - rcBounds.left;
                int y1 = rcUpdate.top - rcBounds.top;
                int width1 = rcUpdate.right - rcUpdate.left;
                int height1 = rcUpdate.bottom - rcUpdate.top;
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
                //graphics.DrawRectangle(new Pen(Color.White), rect);

                graphics.DrawImage(bitmap, x, y,
                                   rect,
                                   GraphicsUnit.Pixel);
            }
        }

        public void onresize(tagSIZE size)
        {
            _paintSite.InvalidateRegion(IntPtr.Zero);
        }

        #endregion
    }
}