#pragma warning disable SYSLIB0014
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using System.Text.RegularExpressions;

using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
// Canvas surface
using System.Windows.Controls;

// FIXME: Used only for ImageFormat
using System.Drawing.Imaging;
// FIXME: Used only for FrameworkElement conversion.
using System.Windows;
// FIXME: Abuse of drawImage to load http url. Abstract to Host if necessary.
using System.Net;
// FIXME: Dynamic loading of Shaders should be handled in Host.
using System.IO;
using System.Reflection;

using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using Color = System.Windows.Media.Color;
using Convert = System.Convert;
using Image = System.Windows.Controls.Image;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Size = System.Windows.Size;

// FIXME: This library has not been converted to use the ObjectWithPrototype class.
// FIXME: Used only with InvokeMember, currently only used with drawImage (bug?).
using SharpCanvas.Interop;
using System.Globalization;
using SharpCanvas.Shared;
using SharpCanvas.Common;
//using SharpCanvas.Prototype;
// FIXME: Should be handled by Host
using SharpCanvas.StandardFilter.FilterSet;

namespace SharpCanvas.Media
{
    public delegate void OnPartialDrawHanlder();

    [ComVisible(true)]
    public class CanvasRenderingContext2D : ICanvasRenderingContext2D//, IExpando
    {
        #region Fields

        private const string CANVAS_STACK_UNDERFLOW =
            "restore() caused a a buffer underflow: There are no more saved states available to restore.";

        private const string INDEX_SIZE_ERR =
            "The specified offset is negative or greater than the number of characters in data, or if the specified count is negative";

        private const string NOT_SUPPORTED_ERR = "Some of the paramters are invalid";
        private const string TYPE_MISTMATCH_ERR = "Type mistmatch error";
        private readonly object _container;
        private readonly Regex _httpRegex = new Regex(@"http://.*");
        private readonly Stack<CanvasState> _stack;
        private readonly Canvas _surface;
        private readonly bool _visible = true;
        private Geometry _clip;
        //      private CompositeMode _compositeMode;
        private Compositer _compositier;
        private ImageSource _destination;
        private Assembly _effects;

        private Brush _fill;

        private object _fillStyle = string.Empty;
        private string _font;
        private double _globalAlpha = -1;
        private string _globalCompositeOperation;
        private bool _isGlobalComposition;
        private string _lineCap;
        private string _lineJoin;
        private double _lineWidth;
        private double _miterLimit;
        public CanvasPath _path;
        //private font _parsedFont = new font("sans-serif", 10);
        private double _shadowBlur;
        private string _shadowColor;
        private double _shadowOffsetX;
        private double _shadowOffsetY;
        private ImageSource _source;
        private ImageSource _sourceOutsideClip;
        private Brush _stroke;
        private object _strokeStyle = string.Empty;
        private CanvasStyle _style;
        private string _textAlign = string.Empty;
        private string _textBaseLine = string.Empty;

        public event OnPartialDrawHanlder OnPartialDraw;

        private IHTMLCanvasElement _canvasElement;

        #endregion

        #region constructors

        public CanvasRenderingContext2D(Canvas surface, object container, bool visible, IHTMLCanvasElement canvasElement)
            : this(surface, container, visible)
        {
            _canvasElement = canvasElement;
        }

        public CanvasRenderingContext2D(Canvas surface, object container, bool visible)
        {
            _surface = surface;
            _path = new CanvasPath(_surface);
            _stack = new Stack<CanvasState>();
            _visible = visible;
            _container = container;
            SetDefaultValues();
            OnPartialDraw += CanvasRenderingContext2D_OnPartialDraw;

            PluginShaderFilter();
        }

        private void PluginShaderFilter()
        {
            string cur = Directory.GetCurrentDirectory();
            string path = cur + "/" + "SharpCanvas.ShaderFilter.dll";
            if (File.Exists(path))
            {
                _effects = Assembly.Load("SharpCanvas.ShaderFilter");
                if (_effects != null)
                {
                    _isGlobalComposition = true;
                }
                else
                {
                    //exception was thrown during Assembly.Load?
                    throw new FileNotFoundException();
                }
            }
            else
            {
                _isGlobalComposition = false;
            }
        }

        private void SetDefaultValues()
        {
            _fill = Brushes.Black;
            _stroke = Brushes.Black;
            //store current style
            _style = new CanvasStyle(_path);
            _style.Fill = _fill;
            _style.Stroke = _stroke;
            _textAlign = "start";
            _textBaseLine = "alphabetic";
            _globalAlpha = 1.0;
            _globalCompositeOperation = "source-over";
            _shadowColor = "rgba(0,0,0,0)";
            _shadowOffsetX = 0;
            _shadowOffsetY = 0;
            _shadowBlur = 0;
            lineWidth = 1.0;
            lineCap = "butt";
            lineJoin = "miter";
            miterLimit = 10.0;
            globalAlpha = 1;
            _compositier = new Compositer();
        }

        #endregion

        private readonly DropShadowEffect shadow = new DropShadowEffect();

        public object canvas
        {
            get { return _canvasElement; }
        }

        #region ICanvasRenderingContext2D Members

        public object prototype()
        {
            throw new NotImplementedException();
        }

        public string direction { get; set; }
        public string filter { get; set; }
        public string fontKerning { get; set; }
        public string fontStretch { get; set; }
        public string fontVariantCaps { get; set; }
        public bool imageSmoothingEnabled { get; set; }
        public string imageSmoothingQuality { get; set; }
        public string lang { get; set; }
        public string letterSpacing { get; set; }
        public double lineDashOffset { get; set; }
        public string textRendering { get; set; }
        public string wordSpacing { get; set; }

        public bool isPointInStroke(double x, double y)
        {
            throw new NotImplementedException();
        }

        public object createConicGradient(double startAngle, double x, double y)
        {
            throw new NotImplementedException();
        }

        public object getLineDash()
        {
            throw new NotImplementedException();
        }

        public void setLineDash(object segments)
        {
            throw new NotImplementedException();
        }

        public void roundRect(double x, double y, double w, double h, object radii)
        {
            throw new NotImplementedException();
        }

        public void ellipse(double x, double y, double radiusX, double radiusY, double rotation, double startAngle, double endAngle, bool anticlockwise)
        {
            throw new NotImplementedException();
        }

        public void drawFocusIfNeeded(object element)
        {
            throw new NotImplementedException();
        }

        public bool isContextLost()
        {
            throw new NotImplementedException();
        }

        public void reset()
        {
            throw new NotImplementedException();
        }

        public object getTransform()
        {
            throw new NotImplementedException();
        }

        public void resetTransform()
        {
            throw new NotImplementedException();
        }

        byte[] ICanvasRenderingContext2D.GetBitmap()
        {
            throw new NotImplementedException();
        }

        public object __proto__
        {
            get { throw new NotImplementedException(); }
        }

        public void save()
        {
            _stack.Push(new CanvasState(_path.GetStyle(), _path.GetTransform()));
        }

        public void restore()
        {
            CanvasState state = _stack.Pop();
            _path.Commit();
            //let's assume that fill, stroke or any other commit-related method was called before, so we shouldn't commit empty path to the surface
            _path.ApplyStyle(state.Style);
            _path.SetTransform(state.Transformation);
        }

        public void scale(double x, double y)
        {
            _path.Scale(x, y);
        }

        public void rotate(double angle)
        {
            _path.Rotate(angle);
        }

        public void translate(double x, double y)
        {
            _path.Translate(x, y);
        }

        public void transform(double m11, double m12, double m21, double m22, double dx, double dy)
        {
            _path.Transform(new Matrix(m11, m12, m21, m22, dx, dy));
        }

        public void setTransform(double m11, double m12, double m21, double m22, double dx, double dy)
        {
            _path.SetTransform(new Matrix(m11, m12, m21, m22, dx, dy));
        }

        public double globalAlpha
        {
            get { return _globalAlpha; }
            set
            {
                if ((0.0 <= value) && (value <= 1.0))
                {
                    _style.GlobalAlpha = _globalAlpha = value;
                }
            }
        }

        public string globalCompositeOperation
        {
            get { return _globalCompositeOperation; }
            set
            {
                _globalCompositeOperation = value;
                commit();
                _source = GetImageOfControl(_surface);
                _surface.Children.Clear();
                //_surface = new Canvas();
            }
        }

        public object strokeStyle
        {
            get { return _strokeStyle; }
            set
            {
                if (_strokeStyle != value)
                {
                    _strokeStyle = value;
                    if (_strokeStyle is string)
                    {
                    System.Drawing.Color drawingColor = ColorUtils.ParseColor((string)_strokeStyle);
                    var mediaColor = System.Windows.Media.Color.FromArgb(drawingColor.A, drawingColor.R, drawingColor.G, drawingColor.B);
                    var brush = new SolidColorBrush(mediaColor);
                        _style.Stroke = brush;
                    }
                    if (_strokeStyle is ILinearCanvasGradient)
                    {
                        _style.Stroke = (Brush)((ILinearCanvasGradient)_strokeStyle).GetBrush();
                    }
                    if (_strokeStyle is IPathCanvasGradient)
                    {
                        _style.Stroke = (Brush)((IPathCanvasGradient)_strokeStyle).GetBrush();
                    }
                }
                else
                {
                    //apply current style to the path (to be sure we're drawing with correct Brush)
                    _path.ApplyStyle(_style);
                }
            }
        }

        public object fillStyle
        {
            get { return _fillStyle; }
            set
            {
                if (_fillStyle != value)
                {
                    _fillStyle = value;
                    if (_fillStyle is string)
                    {
                    System.Drawing.Color drawingColor = ColorUtils.ParseColor((string)_fillStyle);
                    var mediaColor = System.Windows.Media.Color.FromArgb(drawingColor.A, drawingColor.R, drawingColor.G, drawingColor.B);
                    var brush = new SolidColorBrush(mediaColor);
                        _style.Fill = brush;
                    }
                    if (_fillStyle is ILinearCanvasGradient)
                    {
                        _style.Fill = (Brush)((ILinearCanvasGradient)_fillStyle).GetBrush();
                    }
                    if (_fillStyle is IPathCanvasGradient)
                    {
                        _style.Fill = (Brush)((IPathCanvasGradient)_fillStyle).GetBrush();
                    }
                    if (_fillStyle is CanvasPattern)
                    {
                        _style.Fill = (((CanvasPattern)_fillStyle).GetBrush());
                    }
                }
                else
                {
                    //apply current style to the path (to be sure we're drawing with correct Brush)
                    _path.ApplyStyle(_style);
                }
            }
        }

        /// <summary>
        /// Values that are not finite values greater than zero are ignored.
        /// When the context is created, the lineWidth attribute must initially have the value 1.0.
        /// </summary>
        public double lineWidth
        {
            get { return _lineWidth; }
            set
            {
                if (value > 0)
                {
                    _lineWidth = value;
                    _style.StrokeWidth = (float)_lineWidth;
                }
            }
        }

        /// <summary>
        /// Returns the current line cap style.
        /// Can be set, to change the line cap style.
        /// The possible line cap styles are butt, round, and square. Other values are ignored.
        /// When the context is created, the lineCap attribute must initially have the value butt.
        /// </summary>
        public string lineCap
        {
            get { return _lineCap; }
            set
            {
                switch (value)
                {
                    case "butt":
                        _style.LineCap = PenLineCap.Flat;
                        _lineCap = value;
                        break;
                    case "round":
                        _style.LineCap = PenLineCap.Round;
                        _lineCap = value;
                        break;
                    case "square":
                        _style.LineCap = PenLineCap.Square;
                        _lineCap = value;
                        break;
                }
            }
        }

        /// <summary>
        /// Returns the current line join style.
        /// Can be set, to change the line join style.
        /// The possible line join styles are bevel, round, and miter. Other values are ignored.
        /// When the context is created, the lineJoin attribute must initially have the value miter.
        /// </summary>
        public string lineJoin
        {
            get { return _lineJoin; }
            set
            {
                switch (value)
                {
                    case "round":
                        _style.StrokeLineJoin = PenLineJoin.Round;
                        _lineJoin = value;

                        break;
                    case "bevel":
                        _style.StrokeLineJoin = PenLineJoin.Bevel;
                        _lineJoin = value;

                        break;
                    case "miter":
                        _style.StrokeLineJoin = PenLineJoin.Miter;
                        _lineJoin = value;

                        break;
                }
            }
        }

        /// <summary>
        /// Returns the current miter limit ratio.
        /// Can be set, to change the miter limit ratio. Values that are not finite values greater than zero are ignored.
        /// When the context is created, the miterLimit attribute must initially have the value 10.0.
        /// </summary>
        public double miterLimit
        {
            get { return _miterLimit; }
            set
            {
                if (value > 0)
                {
                    _miterLimit = value;
                    _style.StrokeMiterLimit = (float)_miterLimit;
                }
            }
        }

        public double shadowOffsetX
        {
            get { return _shadowOffsetX; }
            set { _shadowOffsetX = value; }
        }

        public double shadowOffsetY
        {
            get { return _shadowOffsetY; }
            set { _shadowOffsetY = value; }
        }

        public double shadowBlur
        {
            get { return _shadowBlur; }
            set { _shadowBlur = value; }
        }

        public string shadowColor
        {
            get { return _shadowColor; }
            set { _shadowColor = value; }
        }

        // The clearRect(x, y, w, h) method must clear the pixels in the specified rectangle that also intersect the current 
        // clipping region to a fully transparent black, erasing any previous image. If either height or width are zero, 
        // this method has no effect.
        public void clearRect(double x, double y, double w, double h)
        {
            if ((0 < w) && (0 < h))
            {
                if ((x <= 0) && (y <= 0) && (_surface.Width <= w) && (_surface.Height <= h))
                {
                    // Optimize common scenario of clearing the entire canvas
                    _surface.Children.Clear();
                }
                else
                {
                    Geometry initialClip = _surface.Clip ??
                                           new RectangleGeometry { Rect = new Rect(0, 0, _surface.Width, _surface.Height) };
                    var group = new GeometryGroup();
                    group.Children.Add(initialClip);
                    group.Children.Add(new RectangleGeometry { Rect = new Rect(x, y, w, h) });
                    _surface.Clip = group;
                    ImageSource source = GetImageOfControl(_surface);
                    //_surface = new Canvas();
                    _surface.Children.Clear();
                    var element = new Image();
                    element.Source = source;
                    _surface.Children.Add(element);
                    _surface.Clip = initialClip;
                }
                if (OnPartialDraw != null)
                {
                    OnPartialDraw();
                }
            }
        }

        public void fillRect(double x, double y, double w, double h)
        {
            _path.FillRect(x, y, w, h);
            ApplyShadows();
            ApplyGlobalCompositionFactory();
            ApplyClip();
            if (OnPartialDraw != null)
            {
                OnPartialDraw();
            }
            commit();
        }

        public void strokeRect(double x, double y, double w, double h)
        {
            _path.StrokeRect(x, y, w, h);
            ApplyShadows();
            ApplyGlobalCompositionFactory();
            ApplyClip();
            if (OnPartialDraw != null)
            {
                OnPartialDraw();
            }
        }

        public void drawImage(object image, float dx, float dy)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The beginPath()  method must empty the list of subpaths so that the context once again has zero subpaths.
        /// </summary>
        public void beginPath()
        {
            _path.BeginPath();
        }

        /// <summary>
        /// The closePath()  method must do nothing if the context has no subpaths. Otherwise, it must mark the last subpath 
        /// as closed, create a new subpath whose first point is the same as the previous subpath's first point, and finally 
        /// add this new subpath to the path.
        /// If the last subpath had more than one point in its list of points, then this is equivalent to adding a 
        /// straight line connecting the last point back to the first point, thus "closing" the shape, and then repeating the 
        /// last (possibly implied) moveTo() call.
        /// </summary>
        public void closePath()
        {
            _path.ClosePath();
        }

        /// <summary>
        /// The moveTo(x, y) method must create a new subpath with the specified point as its first (and only) point.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        //TODO: implement case when moveTo called not before any drawing in the figure, but in the middle
        public void moveTo(double x, double y)
        {
            _path.MoveTo(x, y);
        }

        /// <summary>
        /// The lineTo(x, y) method must ensure there is a subpath for (x, y) if the context has no subpaths. 
        /// Otherwise, it must connect the last point in the subpath to the given point (x, y) using a straight line, and must then 
        /// add the given point (x, y) to the subpath.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void lineTo(double x, double y)
        {
            _path.LineTo(x, y);
        }

        public void quadraticCurveTo(double cpx, double cpy, double x, double y)
        {
            _path.QuadraticCurveTo(cpx, cpy, x, y);
        }

        public void bezierCurveTo(double cp1x, double cp1y, double cp2x, double cp2y, double x, double y)
        {
            _path.BezierCurveTo(cp1x, cp1y, cp2x, cp2y, x, y);
        }

        public void arcTo(double x1, double y1, double x2, double y2, double radius)
        {
            if (radius < 0)
                throw new Exception(INDEX_SIZE_ERR);
            _path.ArcTo(x1, y1, x2, y2, radius);
        }

        public void arc(double x, double y, double r, double startAngle, double endAngle, bool clockwise)
        {
            if (r < 0)
                throw new Exception(INDEX_SIZE_ERR);
            _path.Arc(x, y, r, startAngle, endAngle, clockwise);
        }

        public void rect(double x, double y, double w, double h)
        {
            _path.Rect(x, y, w, h);
        }

        public void fill()
        {
            _path.Fill();
            ApplyShadows();
            ApplyGlobalCompositionFactory();
            ApplyClip();
            if (OnPartialDraw != null)
            {
                OnPartialDraw();
            }
        }

        /// <summary>
        /// The stroke() method must calculate the strokes of all the subpaths of the current path, using the lineWidth, lineCap, 
        /// lineJoin, and (if appropriate) miterLimit attributes, and then fill the combined stroke area using the strokeStyle  
        /// attribute.
        /// </summary>
        public void stroke()
        {
            _path.Stroke(false);
            ApplyShadows();
            ApplyGlobalCompositionFactory();
            ApplyClip();
            if (OnPartialDraw != null)
            {
                OnPartialDraw();
            }
        }

        public void clip()
        {
            Clip(null);
        }

        public string font
        {
            get { return _font; }
            set { _font = value; }
        }

        public string textAlign
        {
            get { return _textAlign; }
            set { _textAlign = value; }
        }

        public string textBaseLine
        {
            get { return _textBaseLine; }
            set { _textBaseLine = value; }
        }

        public void fillText(string text, double x, double y)
        {
            _path.FillText(text, x, y, _font, _textAlign, _textBaseLine);
            ApplyShadows();
            ApplyGlobalCompositionFactory();
            if (OnPartialDraw != null)
            {
                OnPartialDraw();
            }
        }

        public void strokeText(string text, double x, double y)
        {
            _path.StrokeText(text, x, y, _font, _textAlign, _textBaseLine);
            ApplyShadows();
            ApplyGlobalCompositionFactory();
            if (OnPartialDraw != null)
            {
                OnPartialDraw();
            }
        }

#pragma warning disable SYSLIB0014
        public void drawImage(object pImg, double sx, double sy, double sw, double sh, double dx, double dy, double dw,
                              double dh)
        {
            if (pImg is ImageData)
            {
                string url = ((ImageData)pImg).src;
                var httpRegex = new Regex(@"http://.*");
                var image = new Image();
                var imageSource = new BitmapImage();
                if (httpRegex.IsMatch(url))
                {
                    var client = new WebClient();
                    byte[] bytes = client.DownloadData(url);
                    using (var memoryStream = new MemoryStream(bytes))
                    {
                        imageSource.BeginInit();
                        imageSource.StreamSource = memoryStream;
                        imageSource.EndInit();
                    }
                }
#pragma warning restore SYSLIB0014
                else
                {
                    imageSource = new BitmapImage();
                    imageSource.BeginInit();
                    imageSource.CacheOption = BitmapCacheOption.OnLoad;
                    imageSource.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    imageSource.UriSource = new Uri(url, UriKind.RelativeOrAbsolute);
                    imageSource.EndInit();
                }
                image.Source = imageSource;

                image.SetValue(System.Windows.Controls.Canvas.TopProperty, dy - sy);
                image.SetValue(System.Windows.Controls.Canvas.LeftProperty, dx - sx);
                var rect = new RectangleGeometry(new Rect(sx, sy, sw, sh));
                image.Clip = rect;
                _surface.Children.Add(image);
            }
            if (OnPartialDraw != null)
            {
                OnPartialDraw();
            }
        }

        public void drawImage(object pImg, double dx, double dy, double dw, double dh)
        {
            if (pImg is ImageData)
            {
                var imageData = ((ImageData)pImg);
                string url = imageData.src;

                var image = new Image();
                var imageSource = new BitmapImage();
                if (_httpRegex.IsMatch(url))
                {
                    var client = new WebClient();
                    byte[] bytes = client.DownloadData(url);
                    using (var memoryStream = new MemoryStream(bytes))
                    {
                        imageSource.BeginInit();
                        imageSource.StreamSource = memoryStream;
                        imageSource.EndInit();
                    }
                }
                else
                {
                    imageSource = new BitmapImage();
                    imageSource.BeginInit();
                    imageSource.CacheOption = BitmapCacheOption.OnLoad;
                    imageSource.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    imageSource.UriSource = new Uri(url, UriKind.RelativeOrAbsolute);
                    imageSource.EndInit();
                }
                image.Source = imageSource;
                image.SetValue(System.Windows.Controls.Canvas.TopProperty, dy);
                image.SetValue(System.Windows.Controls.Canvas.LeftProperty, dx);
                if (dw > 0 && dh > 0)
                {
                    image.Width = dw;
                    image.Height = dh;
                }
                _surface.Children.Add(image);
                imageData.width = (uint)image.Width;
                imageData.height = (uint)image.Height;
            }
            if (OnPartialDraw != null)
            {
                OnPartialDraw();
            }
        }

        public void drawImage(object pImg, double dx, double dy)
        {
#pragma warning restore SYSLIB0014
            if (pImg is ImageData)
            {
                var imageData = ((ImageData)pImg);
                string url = imageData.src;
                Size size = LoadImageFromUrl(url, dy, dx);
                imageData.width = (uint)size.Width;
                imageData.height = (uint)size.Height;
            }
            if (pImg is IImageData)
            {
                LoadImageFromUrl(((IImageData)pImg).src, dy, dx);
            }
            if (pImg is string)
            {
                LoadImageFromUrl((string)pImg, dy, dx);
            }
            if (OnPartialDraw != null)
            {
                OnPartialDraw();
            }
        }

        public bool isPointInPath(double x, double y)
        {
            return _path.IsPointInPath(x, y);
        }

        public object createLinearGradient(double x0, double y0, double x1, double y1)
        {
            return new LinearCanvasGradient(x0, y0, x1, y1);
        }

        public object createPattern(object imageData, string repetition)
        {
            return new CanvasPattern(repetition, ((ImageData)imageData).src);
        }

        public object createRadialGradient(double x0, double y0, double r0, double x1, double y1, double r1)
        {
            return new PathCanvasGradient(x0, y0, r0, x1, y1, r1);
        }

        public object measureText(string text)
        {
            return _path.MeasureText(text, _font);
        }

        public object getImageData(double sx, double sy, double sw, double sh)
        {
            //get System.DrawingBitmap from WPF control
            ImageSource control = GetImageOfControl(_surface);
            Bitmap bitmap = BitmapSourceToBitmap((BitmapSource)control);
            //process getImageData as WinForm getImageData method
            if (double.IsNaN(sw) || double.IsInfinity(sw) || double.IsInfinity(sh) || double.IsNaN(sh)
                || double.IsNaN(sx) || double.IsInfinity(sx) || double.IsInfinity(sy) || double.IsNaN(sy))
            {
                throw new NotSupportedException(NOT_SUPPORTED_ERR);
            }
            var img = new ImageData(Convert.ToUInt32(sw), Convert.ToUInt32(sh));
            var arr = new List<object>();
            for (int y = 0; y < sh; y++)
            {
                for (int x = 0; x < sw; x++)
                {
                    int maxX = (int)sx + x;
                    if (maxX >= bitmap.Width)
                        maxX = bitmap.Width - 1;
                    int maxY = (int)sy + y;
                    if (maxY >= bitmap.Height)
                    {
                        maxY = bitmap.Height - 1;
                    }
                    System.Drawing.Color color = bitmap.GetPixel(maxX, maxY);
                    arr.AddRange(new List<object> { color.R, color.G, color.B, color.A });
                }
            }
            img.data = Utils.ConvertArrayToJSArray(arr.ToArray());
            return img;
        }

        public object createImageData(double sw, double sh)
        {
            if (double.IsNaN(sw) || double.IsInfinity(sw) || double.IsInfinity(sh) || double.IsNaN(sh))
            {
                throw new NotSupportedException(NOT_SUPPORTED_ERR);
            }
            var img = new ImageData(Convert.ToUInt32(sw), Convert.ToUInt32(sh));
            var arr = new List<object>();
            for (int x = 0; x < sw; x++)
            {
                for (int y = 0; y < sh; y++)
                {
                    arr.AddRange(new List<object> { 0, 0, 0, 0 });
                }
            }
            img.data = Utils.ConvertArrayToJSArray(arr.ToArray());
            return img;
        }

        public void putImageData(object imagedata, double dx, double dy)
        {
            if (double.IsNaN(dx) || double.IsInfinity(dx) || double.IsInfinity(dy) || double.IsNaN(dy))
            {
                throw new NotSupportedException(NOT_SUPPORTED_ERR);
            }
            if (!(imagedata is ImageData))
            {
                throw new Exception(TYPE_MISTMATCH_ERR);
            }
            var img = imagedata as ImageData;
            // We are using 32 bit color.
            int bytesPerPixel = 4;
            // Where we are going to store our pixel information.
            var pixels = new byte[img.height * img.width * bytesPerPixel];
            byte[] data = Utils.ConvertJSArrayToByteArray(img.data);
            for (int y = 0; y < img.height; y++)
            {
                for (int x = 0; x < img.width; x++)
                {
                    int index = y * (int)img.width * 4 + x * 4;
                    pixels[index] = data[index];
                    pixels[index + 1] = data[index + 1];
                    pixels[index + 2] = data[index + 2];
                    pixels[index + 3] = data[index + 3];
                }
            }
            // Where we are going to store our pixel information.
            // Calculate the stride of the bitmap
            int stride = (int)img.width * bytesPerPixel;
            BitmapSource source = BitmapSource.Create((int)img.width, (int)img.height, 96, 96, PixelFormats.Pbgra32,
                                                      null,
                                                      pixels, stride);
            source.Freeze();
            var image = new Image();
            image.Source = source;
            image.SetValue(System.Windows.Controls.Canvas.TopProperty, dy);
            image.SetValue(System.Windows.Controls.Canvas.LeftProperty, dx);
            _surface.Children.Add(image);
        }

        public void putImageData(object imagedata, double dx, double dy, double dirtyX, double dirtyY, double dirtyWidth,
                                 double dirtyHeight)
        {
            if (double.IsNaN(dx) || double.IsInfinity(dx) || double.IsInfinity(dy) || double.IsNaN(dy)
                || double.IsInfinity(dirtyX) || double.IsNaN(dirtyX) || double.IsInfinity(dirtyY) ||
                double.IsNaN(dirtyY)
                || double.IsInfinity(dirtyWidth) || double.IsNaN(dirtyWidth) || double.IsInfinity(dirtyHeight) ||
                double.IsNaN(dirtyHeight))
            {
                throw new NotSupportedException(NOT_SUPPORTED_ERR);
            }
            if (!(imagedata is ImageData))
            {
                throw new Exception(TYPE_MISTMATCH_ERR);
            }
            var img = imagedata as ImageData;
            if (dirtyWidth < 0)
            {
                dirtyWidth += dirtyX;
            }
            if (dirtyHeight < 0)
            {
                dirtyHeight += dirtyY;
            }
            if (dirtyX < 0)
                dirtyX = 0;
            if (dirtyY < 0)
                dirtyY = 0;
            if (dirtyWidth + dirtyX > img.width)
            {
                dirtyWidth = img.width - dirtyX;
            }
            if (dirtyHeight + dirtyY > img.height)
            {
                dirtyHeight = img.height - dirtyY;
            }
            if (dirtyHeight <= 0 || dirtyWidth <= 0)
                return;

            var _dirtyX = (int)Math.Truncate(dirtyX);
            var _dirtyY = (int)Math.Truncate(dirtyY);
            var _dirtyWidth = (int)Math.Truncate(dirtyWidth);
            var _dirtyHeight = (int)Math.Truncate(dirtyHeight);

            // We are using 32 bit color.
            int bytesPerPixel = 4;
            // Where we are going to store our pixel information.
            var pixels = new byte[_dirtyHeight * _dirtyWidth * bytesPerPixel];
            byte[] data = Utils.ConvertJSArrayToByteArray(img.data);
            for (int y = _dirtyY; y < _dirtyHeight + _dirtyY; y++)
            {
                for (int x = _dirtyX; x < _dirtyWidth + _dirtyX; x++)
                {
                    int index = y * (int)img.width * 4 + x * 4;
                    pixels[index] = data[index];
                    pixels[index + 1] = data[index + 1];
                    pixels[index + 2] = data[index + 2];
                    pixels[index + 3] = data[index + 3];
                }
            }
            // Where we are going to store our pixel information.
            // Calculate the stride of the bitmap
            int stride = _dirtyWidth * bytesPerPixel;
            BitmapSource source = BitmapSource.Create(_dirtyWidth, _dirtyHeight, 96, 96, PixelFormats.Pbgra32, null,
                                                      pixels, stride);
            var image = new Image();
            image.Source = source;
            image.SetValue(System.Windows.Controls.Canvas.TopProperty, dy);
            image.SetValue(System.Windows.Controls.Canvas.LeftProperty, dx);
            _surface.Children.Add(image);
        }

        public object createFilterChain()
        {
            return new SharpCanvas.StandardFilter.FilterSet.FilterChain();
        }

        public Bitmap GetBitmap()
        {
            if (_surface.Children.Count > 0 || _surface.Background != null)
            {
                if (_visible)
                {
                    ImageSource control = GetImageOfControl(_surface);
                    Bitmap bitmap = BitmapSourceToBitmap((BitmapSource)control);
                    return bitmap;
                }
                else
                {
                    return new Bitmap((int)_surface.Width, (int)_surface.Height);
                }
            }
            else
            {
                return new Bitmap(1, 1);
            }
        }

        /// <summary>
        /// Change size of canvas and underlying controls.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void ChangeSize(int width, int height, bool reset)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return current height of the surface
        /// </summary>
        /// <returns></returns>
        public int GetHeight()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return current width of the surface
        /// </summary>
        /// <returns></returns>
        public int GetWidth()
        {
            throw new NotImplementedException();
        }

        public bool IsVisible
        {
            get { return _visible; }
        }

        public void commit()
        {
            _path.Commit();
            _surface.UpdateLayout();
        }

        #endregion


        #region Implementation of IExpando

        public FieldInfo AddField(string name)
        {
            var expanededFields = new FieldInfo[_fields.Length + 1];
            //_fields.CopyTo(expanededFields, 0);
            //PrototypeFieldInfo field = new PrototypeFieldInfo(name);
            //expanededFields[_fields.Length] = field;
            //_fields = expanededFields;
            //_arbitraryFields.Add(name, field);
            // return field;
            throw new NotImplementedException();
        }

        public PropertyInfo AddProperty(string name)
        {
            var expanededFields = new PropertyInfo[_fields.Length + 1];
            //_properties.CopyTo(expanededFields, 0);
            //PrototypePropertyInfo property = new PrototypePropertyInfo(name);
            //expanededFields[_fields.Length] = property;
            //_properties = expanededFields;
            //_arbitraryProperties.Add(name, property);
            //return property;
            throw new NotImplementedException();
        }

        public MethodInfo AddMethod(string name, Delegate method)
        {
            //return new MyMethodInfo(name);
            throw new NotImplementedException();
        }

        public void RemoveMember(MemberInfo m)
        {
            throw new NotImplementedException();
        }

        #endregion

        private void CanvasRenderingContext2D_OnPartialDraw()
        {
            if (_visible && _container != null)
            {
                //if (_container is System.Windows.Forms.UserControl)//we have winforms environment
                //    ((System.Windows.Forms.UserControl)_container).Invalidate(true);
                if (_container is IHTMLPainter) //we have IE environment
                    ((IHTMLPainter)_container).ReDraw();
            }
        }

        /// <summary>
        /// Convert any control to a PngBitmapEncoder
        /// </summary>
        /// <param name="controlToConvert">The control to convert to an ImageSource</param>
        /// <returns>The returned ImageSource of the controlToConvert</returns>
        private static PngBitmapEncoder GetEncoderOfControl(FrameworkElement controlToConvert)
        {
            // save current canvas transform
            Transform transform = controlToConvert.LayoutTransform;

            // get size of control
            var sizeOfControl = new Size(controlToConvert.Width, controlToConvert.Height);

            if (!sizeOfControl.Height.Equals(double.NaN) && !sizeOfControl.Width.Equals(double.NaN))
            {
                // measure and arrange the control
                controlToConvert.Measure(sizeOfControl);

                // arrange the surface
                controlToConvert.Arrange(new Rect(sizeOfControl));
            }
            else
            {
                sizeOfControl = new Size(1, 1);
            }

            // craete and render surface and push bitmap to it
            var renderBitmap = new RenderTargetBitmap((Int32)sizeOfControl.Width, (Int32)sizeOfControl.Height, 96d,
                                                      96d, PixelFormats.Pbgra32);

            // now render surface to bitmap
            renderBitmap.Render(controlToConvert);

            // encode png data.
            //todo: investigate the best format for our needs
            var pngEncoder = new PngBitmapEncoder();

            // puch rendered bitmap into it
            pngEncoder.Frames.Add(BitmapFrame.Create(renderBitmap));

            // return encoder
            return pngEncoder;
        }

        /// <summary>
        /// Get an ImageSource of a control
        /// </summary>
        /// <param name="controlToConvert">The control to convert to an ImageSource</param>
        /// <returns>The returned ImageSource of the controlToConvert</returns>
        public static ImageSource GetImageOfControl(FrameworkElement controlToConvert)
        {
            // return first frame of image
            return GetEncoderOfControl(controlToConvert).Frames[0];
        }

        private void ApplyClip()
        {
            if (_sourceOutsideClip != null)
            {
                Geometry beforeClip = _surface.Clip;
                _surface.Clip = _clip;
                //we have to commit all the graphis before making static image from the control
                commit();
                ImageSource clippedImageSource = GetImageOfControl(_surface);
                _surface.Clip = beforeClip;
                var clippedImage = new Image();
                clippedImage.Source = clippedImageSource;
                var outsideClipImage = new Image();
                outsideClipImage.Source = _sourceOutsideClip;
                _surface.Clip = beforeClip;
                //_surface = new Canvas();                
                _surface.Children.Clear();
                _surface.Children.Add(outsideClipImage);
                _surface.Children.Add(clippedImage);
            }
        }

        private void ApplyShadows()
        {
            System.Drawing.Color drawingColor = ColorUtils.ParseColor(_shadowColor);
            var color = System.Windows.Media.Color.FromArgb(drawingColor.A, drawingColor.R, drawingColor.G, drawingColor.B);
            if (ColorUtils.isValidColor(_shadowColor) && color.A != 0
                && _shadowBlur != 0 && (_shadowOffsetX != 0 || _shadowOffsetY != 0))
            {
                commit();
                ImageSource img = GetImageOfControl(_surface);
                //_surface = new Canvas();
                _surface.Children.Clear();
                _surface.Background = new ImageBrush(img);

                shadow.Color = color;
                shadow.Opacity = _surface.Opacity;
                shadow.BlurRadius = _shadowBlur;
                shadow.Direction = ConvertRadiansToDegrees(Math.Atan(_shadowOffsetX / _shadowOffsetY)) - 90;
                shadow.ShadowDepth = Math.Sqrt(_shadowOffsetY * _shadowOffsetY + _shadowOffsetX * _shadowOffsetX);
                _surface.Effect = shadow;
            }
        }

        /// <summary>
        /// Converts radians to degrees
        /// </summary>
        private double ConvertRadiansToDegrees(double radians)
        {
            double degrees = (float)(180 / Math.PI) * radians;
            return (degrees);
        }

        private void ApplyGlobalCompositionNoShader()
        {
            if (_source != null)
            {
                //we have to commit all the graphis before making static image from the control
                commit();
                _destination = GetImageOfControl(_surface);
                Bitmap dest = BitmapSourceToBitmap((BitmapSource)_destination);
                Bitmap source = BitmapSourceToBitmap((BitmapSource)_source);
                switch (_globalCompositeOperation)
                {
                    case "source-over":
                        _compositier.setCompositeMode(CompositeMode.SourceOver);
                        break;
                    case "source-in":
                        _compositier.setCompositeMode(CompositeMode.SourceIn);
                        break;
                    case "source-out":
                        _compositier.setCompositeMode(CompositeMode.SourceOut);
                        break;
                    case "source-atop":
                        _compositier.setCompositeMode(CompositeMode.SourceATop);
                        break;
                    case "destination-over":
                        _compositier.setCompositeMode(CompositeMode.DestinationOver);
                        break;
                    case "destination-in":
                        _compositier.setCompositeMode(CompositeMode.DestinationIn);
                        break;
                    case "destination-out":
                        _compositier.setCompositeMode(CompositeMode.DestinationOut);
                        break;
                    case "destination-atop":
                        _compositier.setCompositeMode(CompositeMode.DestinationATop);
                        break;
                    case "lighter":
                        _compositier.setCompositeMode(CompositeMode.Lighter);
                        break;
                    case "darker":
                        _compositier.setCompositeMode(CompositeMode.Darker);
                        break;
                    case "copy":
                        _compositier.setCompositeMode(CompositeMode.Copy);
                        break;
                    case "xor":
                        _compositier.setCompositeMode(CompositeMode.XOR);
                        break;
                }

                source.MakeTransparent(System.Drawing.Color.White);
                dest.MakeTransparent(System.Drawing.Color.White);
                _compositier.blendImages(source, dest);
                _surface.Children.Clear();
                //_surface = new Canvas();
                _surface.Background = new ImageBrush(BitmapToBitmapSource(source));
            }
        }

        private void ApplyGlobalCompositionFactory()
        {
            if (_isGlobalComposition)
            {
                ApplyGlobalComposition(_effects);
            }
            else
            {
                ApplyGlobalCompositionNoShader();
            }
        }

        private void ApplyGlobalComposition(Assembly effects)
        {
            if (_source != null)
            {
                //we have to commit all the graphis before making static image from the control
                commit();
                _destination = GetImageOfControl(_surface);
                var dest = new ImageBrush(_destination);
                _surface.Children.Clear();
                //_surface = new Canvas();
                _surface.Background = new ImageBrush(_source);
                object effect = null;
                Type type = effects.GetType(effects.GetName().Name + ".SourceOver");
                switch (_globalCompositeOperation)
                {
                    case "source-over":
                        type = effects.GetType(effects.GetName().Name + ".SourceOver");
                        break;
                    case "x-chrome-source-in":
                        type = effects.GetType(effects.GetName().Name + ".ChromeSourceIn");
                        break;
                    case "source-in":
                        type = effects.GetType(effects.GetName().Name + ".SourceIn");
                        break;
                    case "x-chrome-source-out":
                        type = effects.GetType(effects.GetName().Name + ".ChromeSourceOut");
                        break;
                    case "source-out":
                        type = effects.GetType(effects.GetName().Name + ".SourceOut");
                        break;
                    case "source-atop":
                        type = effects.GetType(effects.GetName().Name + ".SourceATop");
                        break;
                    case "destination-over":
                        type = effects.GetType(effects.GetName().Name + ".DestinationOver");
                        break;
                    case "x-chrome-destination-in":
                        type = effects.GetType(effects.GetName().Name + ".ChromeDestinationIn");
                        break;
                    case "destination-in":
                        type = effects.GetType(effects.GetName().Name + ".DestinationIn");
                        break;
                    case "destination-out":
                        type = effects.GetType(effects.GetName().Name + ".DestinationOut");
                        break;
                    case "x-chrome-destination-atop":
                        type = effects.GetType(effects.GetName().Name + ".ChromeDestinationATop");
                        break;
                    case "destination-atop":
                        type = effects.GetType(effects.GetName().Name + ".DestinationATop");
                        break;
                    case "lighter":
                        type = effects.GetType(effects.GetName().Name + ".Lighter");
                        break;
                    case "darker":
                        type = effects.GetType(effects.GetName().Name + ".Darker");
                        break;
                    case "copy":
                        type = effects.GetType(effects.GetName().Name + ".Copy");
                        break;
                    case "xor":
                        type = effects.GetType(effects.GetName().Name + ".XOR");
                        break;
                }
                ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
                effect = constructor.Invoke(null);
                //effect.Input2 = dest;
                PropertyInfo input2 = effect.GetType().GetProperty("Input2");
                input2.SetValue(effect, dest, null);
                _surface.Effect = (ShaderEffect)effect;
                _source = null;
                _destination = null;
            }
        }

        private void Clip(Geometry clipGeometry)
        {
            if (clipGeometry == null)
            {
                clipGeometry = _path.GetClip();
            }
            _sourceOutsideClip = GetImageOfControl(_surface);
            _clip = clipGeometry;
        }

        private Size LoadImageFromUrl(string url, double dy, double dx)
        {
            var image = new Image();
            //load image into Bitmap
            Bitmap bmp = Utils.GetBitmapFromUrl(url);
            var ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Png);
            //convert Drawing.Bitmap to Media.BitmapImage
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = ms;
            bitmapImage.EndInit();
            bitmapImage.Freeze();
            //assign image's source
            image.Source = bitmapImage;
            image.SetValue(System.Windows.Controls.Canvas.TopProperty, dy);
            image.SetValue(System.Windows.Controls.Canvas.LeftProperty, dx);
            image.Width = bitmapImage.PixelWidth;
            image.Height = bitmapImage.PixelHeight;
            _surface.Children.Add(image);

            return new Size(bitmapImage.PixelWidth, bitmapImage.PixelHeight);
        }

        public Bitmap BitmapSourceToBitmap(BitmapSource srs)
        {
            Bitmap btm = null;
            int width = srs.PixelWidth;
            int height = srs.PixelHeight;
            int stride = width * ((srs.Format.BitsPerPixel + 7) / 8);
            var bits = new byte[height * stride];
            srs.CopyPixels(bits, stride, 0);
            unsafe
            {
                fixed (byte* pB = bits)
                {
                    var ptr = new IntPtr(pB);
                    btm = new Bitmap(
                        width,
                        height,
                        stride,
                        PixelFormat.Format32bppPArgb,
                        ptr);
                }
            }
            return btm;
        }

        public ImageSource BitmapToBitmapSource(Bitmap bitmap)
        {
            var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);

            var bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();
            return bi;
        }

        public Bitmap BytesToBitmap(byte[] byteArray)
        {
            using (var ms = new MemoryStream(byteArray))
            {
                var img = (Bitmap)System.Drawing.Image.FromStream(ms);
                return img;
            }
        }
    }
}