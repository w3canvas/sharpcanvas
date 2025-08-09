using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

using SharpCanvas.Interop;

// FIXME: This library has not been converted to use the ObjectWithPrototype class.
// FIXME: Used only with InvokeMember, currently only used with drawImage (bug?).
using SharpCanvas.Shared;
using Convert = System.Convert;

//namespace SharpCanvas.Drawing
namespace SharpCanvas.Forms
{
    
    [ComVisible(true),
     ComSourceInterfaces(typeof (ICanvasRenderingContext2D))]
    public class CanvasRenderingContext2D : ICanvasRenderingContext2D
    {
        #region Private Fields

// FIXME: Cleanup and move to Share.
        private const string CANVAS_STACK_UNDERFLOW =
            "restore() caused a a buffer underflow: There are no more saved states available to restore.";
        private const string INDEX_SIZE_ERR =
            "The specified offset is negative or greater than the number of characters in data, or if the specified count is negative";
        private const string NOT_SUPPORTED_ERR = "Some of the paramters are invalid";
        private const string TYPE_MISTMATCH_ERR = "Type mistmatch error";

        private const string FONT_REGEX = @"(?<size>\d+)(?<metric>\w+)\W+(?<font>\w+.*)";

        private readonly CanvasConfig _initialConfig;
        private Bitmap _surfaceBitmap;
        private readonly bool _visible = true;

        private Compositer _compositier;

        private FontFamily _family;
        private IFill _fill;

        private object _fillStyle = string.Empty;
        private string _font;
        private double _globalAlpha = -1;
        private string _globalCompositeOperation;
        private string _lineCap;
        private string _lineJoin;
        private double _lineWidth;
        private double _miterLimit;
        private Font _parsedFont = new Font("sans-serif", 10);
        private double _shadowBlur;
        private string _shadowColor;
        private double _shadowOffsetX;
        private double _shadowOffsetY;
        private Bitmap _source;
        private Pen _stroke;
        private object _strokeStyle = string.Empty;
        private string _textAlign = string.Empty;
        private string _textBaseLine = string.Empty;
        private Matrix _transformation;

        private GraphicsPath path;
        private Stack<CanvasState> stack;
        private Graphics surface;

        public event OnPartialDrawHanlder OnPartialDraw;

        private ICanvasProxy _canvasElement;

        private object _sync = new object();

        #endregion

        #region Properties

        public Graphics Surface
        {
            get { return surface; }
            set { surface = value; }
        }

        public object canvas
        {
            get { return _canvasElement; }
        }

        public virtual object prototype //#CanvasRenderingContext2D object. Its type is CanvasPrototypeContainer
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        object ICanvasRenderingContext2D.prototype()
        {
            throw new NotImplementedException();
        }

        public virtual object __proto__
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region Constructors

        public CanvasRenderingContext2D(Graphics s, Bitmap bitmap, Pen stroke, IFill fill, bool visible, ICanvasProxy canvasElement)
            :this(s, bitmap, stroke, fill, visible)
        {
            _canvasElement = canvasElement;
        }

        /// <summary>
        /// Create CanvasRenderingContext. Graphics and Bitmap passed into constructor should be related to each other
        /// </summary>
        /// <param name="s">Graphics object to get resolution and size data from</param>
        /// <param name="stroke">initial Pen configuration</param>
        /// <param name="fill">initial Brush configuration</param>
        /// <param name="visible">determing is this object be visible inside container</param>
        public CanvasRenderingContext2D(Graphics s, Bitmap bitmap, Pen stroke, IFill fill, bool visible)
        {
            var width = (int) s.VisibleClipBounds.Width;
            var height = (int) s.VisibleClipBounds.Height;
            _visible = visible;
            SetLineConfig(stroke);
            surface = s;
            _surfaceBitmap = bitmap;
            _stroke = stroke;
            _fill = fill;
            SetDefaultValues();
            //TODO: investigate how can we get all data from fill and stroke to CanvasRenderingContext2D properties
            //or just avoid such parameters in constructor
            strokeStyle = "rgba(" + stroke.Color.R + "," + stroke.Color.G + "," + stroke.Color.B + "," + stroke.Color.A +
                          ")";
            fillStyle = "rgba(" + fill.color.R + "," + fill.color.G + "," + fill.color.B + "," + fill.color.A + ")";
            _initialConfig = new CanvasConfig(stroke.Clone() as Pen, fill.Clone() as Fill);
            OnPartialDraw += CanvasRenderingContext2D_OnPartialDraw;
        }

        public CanvasRenderingContext2D()
        {
            // FIXME: Throw debug error.
            // MessageBox.Show("Empty constr");
        }

        private void CanvasRenderingContext2D_OnPartialDraw()
        {
            ApplyShadows();
            ApplyGlobalComposition();
            //there is no need to call RequestDraw on the container because container already subscribed to OnPartialDraw event
            //and will execute Redraw method once OnPartialDraw occurs
            if (_visible && _canvasElement != null)
            {
                ////if (_container is UserControl)//we have winforms environment
                ////    ((UserControl)_container).Invalidate();
                //if (_container is IECanvasHost.Interop.IHTMLPainter)//we have IE environment
                //    ((IECanvasHost.Interop.IHTMLPainter)_container).ReDraw();
                
                //_canvasElement.RealObject.RequestDraw();
            }
        }

        /// <summary>
        /// Put values of the stroke into set of canvas fields.
        /// </summary>
        private void SetLineConfig(Pen stroke)
        {
            _lineWidth = stroke.Width;
            _miterLimit = stroke.MiterLimit;
            _lineJoin = stroke.LineJoin.ToString();
        }

        /// <summary>
        /// Set default values to the canvas fields
        /// </summary>
        private void SetDefaultValues()
        {
            //When the context is created, the font of the context must be set to 10px sans-serif.
            _family = new FontFamily(GenericFontFamilies.Serif);
            _parsedFont = new Font(_family, 10, FontStyle.Regular, GraphicsUnit.Pixel);
            _font = _parsedFont.ToString();
            path = new GraphicsPath();
            path.FillMode = FillMode.Winding;
            stack = new Stack<CanvasState> {};
            //_fillStyle = "rgba(0,0,0,0)";
            _transformation = new Matrix();
            _globalAlpha = 1.0;
            _globalCompositeOperation = "source-over";
            _textAlign = "start";
            _textBaseLine = "alphabetic";
            lineWidth = 1.0;
            lineCap = "butt";
            lineJoin = "miter";
            miterLimit = 10.0;
            _shadowColor = "rgba(0,0,0,0)";
            _shadowOffsetX = 0;
            _shadowOffsetY = 0;
            _shadowBlur = 0;
            surface.SmoothingMode = SmoothingMode.HighQuality;
            surface.InterpolationMode = InterpolationMode.HighQualityBicubic;
            surface.PixelOffsetMode = PixelOffsetMode.HighQuality;
            surface.PageUnit = GraphicsUnit.Pixel;
            //scale(surface.DpiX/96, surface.DpiY/96);
            //surface.PageScale = surface.DpiX * 2/96;
            _compositier = new Compositer();
        }

        #endregion

        #region state

        public void save()
        {
            string fillStyle = string.Empty;
            string strokeStyle = string.Empty;
            if (_fillStyle is string)
                fillStyle = _fillStyle.ToString();
            if (_strokeStyle is string)
                strokeStyle = _strokeStyle.ToString();
            var state = new CanvasState(surface.Save(), _family, _font, _lineWidth, _miterLimit, _lineJoin,
                                        _lineCap, _globalCompositeOperation, _shadowOffsetX, _shadowOffsetY,
                                        _shadowBlur, _shadowColor, fillStyle, strokeStyle, _transformation.Clone());
            stack.Push(state);
        }

        public void restore()
        {
            if (stack.Count == 0)
                throw new Exception(CANVAS_STACK_UNDERFLOW);
            CanvasState state = stack.Pop();
            surface.Restore(state.GraphicsState);
            object ss = state.StrokeStyle;
            if (ss != null)
            {
                if ((ss is string && ss.ToString() != string.Empty) || !(ss is string))
                    strokeStyle = ss;
            }

            object fs = state.FillStyle;
            if (fs != null)
            {
                if ((fs is string && fs.ToString() != string.Empty) || !(fs is string))
                    fillStyle = fs;
            }
            _family = state.Family;
            _font = state.Font;
            _lineWidth = state.LineWidth;
            _miterLimit = state.MiterLimit;
            _lineJoin = state.LineJoin;
            _lineCap = state.LineCap;
            _globalCompositeOperation = state.GlobalCompositeOperation;
            _shadowOffsetX = state.ShadowOffsetX;
            _shadowOffsetY = state.ShadowOffsetY;
            _shadowBlur = state.ShadowBlur;
            _shadowColor = state.ShadowColor;
            _transformation = state.Transformation;
        }

        #endregion

        #region transformations (default transform is the identity matrix)

        public void translate(double x, double y)
        {
            _transformation.Translate((float) x, (float) y);
        }

        public void rotate(double angle)
        {
            _transformation.Rotate((float) ConvertRadiansToDegrees(angle), MatrixOrder.Prepend);
        }

        public void scale(double x, double y)
        {
            _transformation.Scale((float) x, (float) y);
        }

        //seems dx and dy doesnt need to be shifted by _dx and _dy
        /// <summary>
        /// The transform(m11, m12, m21, m22, dx, dy) method must multiply the current transformation matrix with the matrix described by:
        /// m11 	m21 	dx
        /// m12 	m22 	dy
        /// 0 	0 	1
        /// </summary>
        public void transform(double m11, double m12, double m21, double m22, double dx, double dy)
        {
            var matrix = new Matrix((float) m11, (float) m12, (float) m21, (float) m22, (float) dx, (float) dy);
            _transformation.Multiply(matrix);
        }

        //seems dx and dy doesnt need to be shifted by _dx and _dy
        public void setTransform(double m11, double m12, double m21, double m22, double dx, double dy)
        {
            _transformation.Reset();
            transform((float) m11, (float) m12, (float) m21, (float) m22, (float) dx, (float) dy);
        }

        #endregion

        #region compositing

        /// <summary>
        /// The value must be in the range from 0.0 (fully transparent) to 1.0 (no additional transparency). 
        /// If an attempt is made to set the attribute to a value outside this range, including Infinity and Not-a-Number (NaN) values, the attribute must 
        /// retain its previous value. When the context is created, the globalAlpha attribute must initially have the value 1.0.
        /// </summary>
        public double globalAlpha
        {
            get { return _globalAlpha; }
            set
            {
                if (value >= 0.0 && value <= 1.0)
                {
                    _globalAlpha = value;
                    var argb = (int) Math.Floor(_globalAlpha*255);
                    _fill.color = Color.FromArgb(argb, _fill.color);
                    _stroke.Color = Color.FromArgb(argb, _stroke.Color);
                }
            }
        }

        /// <summary>
        /// Support only two types of composition mode
        /// When the context is created, the globalCompositeOperation  attribute must initially have the value source-over.
        /// </summary>
        public string globalCompositeOperation
        {
            get { return _globalCompositeOperation; }
            set
            {
                _globalCompositeOperation = value;
                surface.Flush();
                _source = (Bitmap) _surfaceBitmap.Clone();
                //_source.save(@"c:\Work\Kiia\trunk\WinFormContext2D\WinFormContext2D\bin\Debug\source.bmp");
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
                surface.Clear(Color.FromArgb(0, 0, 0, 0));
                //surface.Clear(Color.Transparent);
                surface.Flush();
            }
        }

        #endregion

        #region colors and styles

        public object fillStyle
        {
            get { return _fillStyle; }
            set
            {
                _fillStyle = value;
                if (_fillStyle is string)
                {
                    _fill.color = ColorUtils.ParseColor((string) _fillStyle);
                }
                if (_fillStyle is LinearCanvasGradient)
                {
                    _fill.brush = (Brush) ((LinearCanvasGradient) _fillStyle).GetBrush();
                }
                if (_fillStyle is PathCanvasGradient)
                {
                    _fill.brush = (Brush) ((PathCanvasGradient) _fillStyle).GetBrush();
                }
                if (_fillStyle is CanvasPattern)
                {
                    _fill.brush = ((CanvasPattern) _fillStyle).GetBrush(_transformation);
                }
            }
        }

        public object strokeStyle
        {
            get { return _strokeStyle; }
            set
            {
                _strokeStyle = value;
                if (_strokeStyle is string)
                {
                    _stroke.Color = ColorUtils.ParseColor((string) _strokeStyle);
                }
                if (_strokeStyle is Pen)
                {
                    _stroke = (Pen) _strokeStyle;
                }
                if (_strokeStyle is LinearCanvasGradient)
                {
                    // _stroke.Brush = ((LinearCanvasGradient) value).GetBrush();
                }
            }
        }

        public object createLinearGradient(double x0, double y0, double x1, double y1)
        {
            PointF[] points = InternalTransform(x0, y0, x1, y1);
            return new LinearCanvasGradient(points[0], points[1]);
        }

        public object createRadialGradient(double x0, double y0, double r0, double x1, double y1, double r1)
        {
            if (r0 < 0 || r1 < 0)
                throw new Exception(INDEX_SIZE_ERR);
            PointF[] points = InternalTransform(x0, y0, x1, y1);
            //PointF[] points = new PointF[]{new PointF((float)x0, (float)y0), new PointF((float)x1, (float)y1)  };
            return new PathCanvasGradient(points[0], (float) r0,
                                          points[1], (float) r1, path);
        }

        public object createPattern(object imageData, string repetition)
        {
            if (imageData is ICanvasProxy)
            {
                ICanvasRenderingContext2D context2D = ((ICanvasProxy)imageData).RealObject.getCanvas();
                return new CanvasPattern(repetition, context2D);
            }
            return new CanvasPattern(repetition, ((ImageData) imageData).src);
        }

        #endregion

        #region line caps/joins

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
                    if (value == 1.0)
                        value = 1.6;
                    _lineWidth = value;
                    _stroke.Width = (float) _lineWidth;
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
                        _stroke.LineJoin = LineJoin.Round;
                        _lineJoin = value;
                        break;
                    case "bevel":
                        _stroke.LineJoin = LineJoin.Bevel;
                        _lineJoin = value;
                        break;
                    case "miter":
                        _stroke.LineJoin = LineJoin.MiterClipped;
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
                    _stroke.MiterLimit = (float) _miterLimit;
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
                        _stroke.StartCap = LineCap.Flat;
                        _stroke.EndCap = LineCap.Flat;
                        _lineCap = value;
                        break;
                    case "round":
                        _stroke.StartCap = LineCap.Round;
                        _stroke.EndCap = LineCap.Round;
                        _lineCap = value;
                        break;
                    case "square":
                        _stroke.StartCap = LineCap.Square;
                        _stroke.EndCap = LineCap.Square;
                        _lineCap = value;
                        break;
                }
            }
        }

        #endregion

        #region shadows

        /// <summary>
        /// The shadowOffsetX  and shadowOffsetY  attributes specify the distance that the shadow will be offset in the positive horizontal and positive vertical distance respectively. 
        /// Their values are in coordinate space units. They are not affected by the current transformation matrix.
        /// When the context is created, the shadow offset attributes must initially have the value 0.
        /// </summary>
        public double shadowOffsetX
        {
            get { return _shadowOffsetX; }
            set { _shadowOffsetX = value; }
        }

        /// <summary>
        /// The shadowOffsetX  and shadowOffsetY  attributes specify the distance that the shadow will be offset in the positive horizontal and positive vertical distance respectively. 
        /// Their values are in coordinate space units. They are not affected by the current transformation matrix.
        /// When the context is created, the shadow offset attributes must initially have the value 0.
        /// </summary>
        public double shadowOffsetY
        {
            get { return _shadowOffsetY; }
            set { _shadowOffsetY = value; }
        }

        /// <summary>
        /// The shadowBlur  attribute specifies the size of the blurring effect. (The units do not map to coordinate space units, and are not affected by the current transformation matrix.)
        /// When the context is created, the shadowBlur attribute must initially have the value 0.
        /// Can be set, to change the blur level. Values that are not finite numbers greater than or equal to zero are ignored.
        /// </summary>
        public double shadowBlur
        {
            get { return _shadowBlur; }
            set
            {
                if (value >= 0)
                {
                    _shadowBlur = value;
                }
            }
        }

        /// <summary>
        /// When the context is created, the shadowColor attribute initially must be fully-transparent black.
        /// On setting, the new value must be parsed as a CSS <color>  value and the color assigned. 
        /// If the value is not a valid color, then it must be ignored, and the attribute must retain its previous value.
        /// </summary>
        public string shadowColor
        {
            get { return _shadowColor; }
            set
            {
                if (ColorUtils.isValidColor(value))
                    _shadowColor = value;
            }
        }

        #endregion

        #region rects

        /// <summary>
        /// The clearRect(x, y, w, h) method must clear the pixels in the specified rectangle that also intersect the current clipping region to a fully transparent black, erasing any previous image. 
        /// If either height or width are zero, this method has no effect.
        /// </summary>
        public void clearRect(double x, double y, double w, double h)
        {
            if (w == 0 || h == 0)
                return;
            //var brush = new SolidBrush(Color.FromArgb(0, 255, 255, 255));
            Color transparent = Color.White;
            var brush = new SolidBrush(transparent);
            PointF[] points = new []{new PointF((float) x, (float) y),
                                               new PointF((float) (x + w), (float) y),
                                               new PointF((float) (x + w), (float) (y + h)),
                                               new PointF((float) x, (float) (y + h))};
            if (points[0].X == 0 && points[0].Y == 0 &&
                points[1].X == 0 && points[1].Y == h &&
                points[2].X == w && points[2].Y == h &&
                points[3].X == w && points[3].Y == 0) //if cear all surface
            {
                surface.Clear(transparent);
            }
            else //do partial clear rect
            {
                surface.FillPolygon(brush, new[]
                                               {
                                                   points[0],
                                                   points[1],
                                                   points[2],
                                                   points[3]
                                               }, FillMode.Winding);
            }

            //update surface
            if (OnPartialDraw != null)
            {
                OnPartialDraw();
            }
        }

        /// <summary>
        /// The strokeRect(x, y, w, h) method must stroke the specified rectangle's path using the strokeStyle, lineWidth, lineJoin, and (if appropriate) miterLimit attributes. If both height and width are zero, this method has no effect, since there is no path to stroke (it's a point). 
        /// If only one of the two is zero, then the method will draw a line instead (the path for the outline is just a straight line along the non-zero dimension).
        /// </summary>
        public void strokeRect(double x, double y, double width, double height)
        {
            if (width == 0 && height == 0)
                return;

            double y2 = y + height;
            PointF[] points = InternalTransform(x, y, x, y2, x + width, y2, x + width, y);
            surface.InterpolationMode = InterpolationMode.HighQualityBicubic;
            if (_strokeStyle is LinearCanvasGradient)
            {
                _stroke = new Pen(transformStrokePoints(_strokeStyle as LinearCanvasGradient, points));
                _stroke.Width = (float) _lineWidth;
                _stroke.MiterLimit = (float) _miterLimit;
            }
            surface.DrawPolygon(_stroke, new[
                                             ]
                                             {
                                                 points[0],
                                                 points[1],
                                                 points[2],
                                                 points[3]
                                             });

            if (OnPartialDraw != null)
            {
                OnPartialDraw();
            }
        }

        /// <summary>
        /// The fillRect(x, y, w, h) method must paint the specified rectangular area using the fillStyle. 
        /// If either height or width are zero, this method has no effect.
        /// </summary>
        public void fillRect(double x, double y, double width, double height)
        {
            if (width == 0 || height == 0)
                return;
            PointF[] points = InternalTransform(x, y, x, y + height, x + width, y + height, x + width, y);
            if (_fillStyle is LinearCanvasGradient)
                _fill.brush = transformStrokePoints(_fillStyle as LinearCanvasGradient, points);

            surface.FillPolygon(_fill.brush, new[
                                                 ]
                                                 {
                                                     points[0],
                                                     points[1],
                                                     points[2],
                                                     points[3]
                                                 }, FillMode.Winding);
            if (OnPartialDraw != null)
            {
                OnPartialDraw();
            }
        }

        /// <summary>
        /// The isPointInPath(x, y) method must return true if the point given by the x and y coordinates passed to the method, when treated as coordinates 
        /// in the canvas coordinate space unaffected by the current transformation, is inside the current path as determined by the non-zero winding number 
        /// rule; and must return false otherwise. Points on the path itself are considered to be inside the path. If either of the arguments is infinite or 
        /// NaN, then the method must return false.
        /// </summary>
        public bool isPointInPath(double x, double y)
        {
            return path.IsVisible((float) x, (float) y);
        }

        private Brush transformStrokePoints(LinearCanvasGradient style, PointF[] points)
        {
            if (points.Length > 4)
            {
                //optimize polygon by wrapping it in rectangle
                PointF mostLeft = points[0];
                foreach (PointF point in points)
                {
                    if (point.X < mostLeft.X)
                        mostLeft = point;
                }
                PointF mostRight = points[0];
                foreach (PointF point in points)
                {
                    if (point.X > mostRight.X)
                        mostRight = point;
                }
                PointF mostTop = points[0];
                foreach (PointF point in points)
                {
                    if (point.Y < mostTop.Y)
                        mostTop = point;
                }
                PointF mostBottom = points[0];
                foreach (PointF point in points)
                {
                    if (point.Y > mostBottom.Y)
                        mostBottom = point;
                }
                var wrapper = new PointF[4];
                wrapper[0] = new PointF(mostLeft.X, mostTop.Y);
                wrapper[1] = new PointF(mostRight.X, mostTop.Y);
                wrapper[2] = new PointF(mostRight.X, mostBottom.Y);
                wrapper[3] = new PointF(mostLeft.X, mostBottom.Y);
                points = wrapper;
            }
            var intersections = new List<PointF>();
            //find intersection points
            for (int i = 0; i < points.Length - 1; i++)
            {
                if (GeometryUtils.LineAndLineIntersects(points[i], points[i + 1], style.start, style.end))
                {
                    intersections.Add(GeometryUtils.GetIntersectionPoint(points[i], points[i + 1], style.start,
                                                                         style.end));
                }
            }
            if (GeometryUtils.LineAndLineIntersects(points[0], points[points.Length - 1], style.start, style.end))
            {
                intersections.Add(GeometryUtils.GetIntersectionPoint(points[0], points[points.Length - 1], style.start,
                                                                     style.end));
            }
            //find intersection points with max distance between them
            PointF start = intersections[0];
            PointF end = intersections[0];
            foreach (PointF a in intersections)
            {
                foreach (PointF b in intersections)
                {
                    if (GeometryUtils.Distance(start, end) < GeometryUtils.Distance(a, b))
                    {
                        start = a;
                        end = b;
                    }
                }
            }
            //swap start and end if needed
            bool leftToRight = style.start.X <= style.end.X;
            bool topToDown = style.start.Y <= style.end.Y;
            if (leftToRight && start.X > end.X)
            {
                PointF tmp = start;
                start = end;
                end = tmp;
            }
            if (topToDown && start.Y > end.Y)
            {
                PointF tmp = start;
                start = end;
                end = tmp;
            }
            //if newly found start or end point are closer to the center than initial start or end
            //then it means we shouldn't stretch this direction
            if (leftToRight && start.X > style.start.X)
            {
                start = style.start;
            }
            if (topToDown && start.Y > style.start.Y)
            {
                start = style.start;
            }
            if (leftToRight && end.X < style.end.X)
            {
                end = style.end;
            }
            if (topToDown && end.Y < style.end.Y)
            {
                end = style.end;
            }

            float before = GeometryUtils.Distance(start, style.start);
            float it = GeometryUtils.Distance(style.start, style.end);
            float after = GeometryUtils.Distance(style.end, end);
            float whole = GeometryUtils.Distance(start, end);
            //move first color
            style.Positions.Insert(0, before/whole);
            style.Colors.Insert(0, style.Colors[0]);
            //move other points
            float shift = before/whole;
            for (int i = 1; i < style.Positions.Count; i++)
            {
                style.Positions[i] = style.Positions[i]*(it/whole) + shift;
            }
            //move last point
            style.Positions.Add(1);
            style.Colors.Add(style.Colors[style.Colors.Count - 1]);
            //stretch gradient
            //shift start point one pixel back and end point one pixel forward
            //TODO: investigate do we need to shift for 1px always or use current lineWidth
            LineEquation e = GeometryUtils.BuildLineEqualtionByTwoPoints(start, end);
            var shiftStartX = (float) _lineWidth;
            float shiftEndX = (float) _lineWidth*(-1);
            if (start.X < end.X)
            {
                shiftStartX = (float) _lineWidth*(-1);
                shiftEndX = (float) _lineWidth;
            }
            var shiftStartY = (float) _lineWidth;
            float shiftEndY = (float) _lineWidth*(-1);
            if (start.Y < end.Y)
            {
                shiftStartY = (float) _lineWidth*(-1);
                shiftEndY = (float) _lineWidth;
            }
            if (!e.IsXConstant)
            {
                start = e.getPointWithX(start.X + shiftStartX);
            }
            else
            {
                start = e.getPointWithY(start.Y + shiftStartY);
            }
            if (!e.IsXConstant)
            {
                end = e.getPointWithX(end.X + shiftEndX);
            }
            else
            {
                end = e.getPointWithY(end.Y + shiftEndY);
            }

            style.start = new PointF(start.X, start.Y);
            style.end = new PointF(end.X, end.Y);
            return (Brush) style.GetBrush();
        }

        private void ApplyShadows()
        {
            if (ColorUtils.isValidColor(_shadowColor) && ColorUtils.ParseColor(_shadowColor).A != 0
                && _shadowBlur != 0 && (_shadowOffsetX != 0 || _shadowOffsetY != 0))
            {
                var blur = new GaussianBlur((int) _shadowBlur);
                var clone = (Bitmap) _surfaceBitmap.Clone();
                clone.MakeTransparent(Color.Transparent);
                Graphics image = Graphics.FromImage(clone);
                image.Clear(Color.Transparent);
                image.DrawImage(_surfaceBitmap, (float) _shadowOffsetX, (float) _shadowOffsetY);
                Bitmap shadow = blur.ProcessImage(clone, ColorUtils.ParseColor(_shadowColor));
                shadow.MakeTransparent(Color.Transparent);

                image.Flush();
                if (shadow != null)
                {
                    surface.Flush();
                    var dest = (Bitmap) _surfaceBitmap.Clone();
                    shadow.MakeTransparent(Color.Transparent);
                    dest.MakeTransparent(Color.Transparent);
                    var cmp = new Compositer();
                    cmp.setCompositeMode(CompositeMode.SourceOver);
                    cmp.blendImages(shadow, dest);
                    surface.Clear(Color.Transparent);
                    surface.DrawImage(shadow, 0, 0);
                }
            }
        }

        /// <summary>
        /// There is no such method in the spec. What is the purpose of maxWidth?
        /// </summary>
        public void StrokeRect(double x, double y, double width, double height, object maxWidth)
        {
            strokeRect(x, y, width, height);
        }

        /// <summary>
        /// There is no such method in the spec. What is the purpose of maxWidth?
        /// </summary>
        public void FillRect(double x, double y, double width, double height, object maxWidth)
        {
            fillRect(x, y, width, height);
        }

        #endregion

        #region path API

        /// <summary>
        /// The beginPath()  method must empty the list of subpaths so that the context once again has zero subpaths.
        /// </summary>
        public void beginPath()
        {
            //we need to close all previous figures in order to be sure that there are no open figures
            path.CloseAllFigures();
            //we don't want to copy previous figure, so we need to reset the path
            path.Reset();
            //don't want to loose default FillMode value after reset
            path.FillMode = FillMode.Winding;
            path.StartFigure();
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
            //TODO: test with creating  new subpath with single point
            path.CloseFigure();
        }

        /// <summary>
        /// The moveTo(x, y) method must create a new subpath with the specified point as its first (and only) point.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void moveTo(double x, double y)
        {
            path.StartFigure();
            //transform passed coordinates accoring to internal transformation matrix
            PointF[] points = InternalTransform(x, y);
            path.AddLine(points[0], points[0]);
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
            //we've initialized path already, so no check is required
            //transform passed coordinates accoring to internal transformation matrix
            PointF[] points = InternalTransform(x, y);
            if (path.PointCount > 0)
                path.AddLine(path.GetLastPoint(), points[0]);
            else
            {
                moveTo(x, y);
            }
        }

        /// <summary>
        /// The quadraticCurveTo(cpx, cpy, x, y) method must ensure there is a subpath for (cpx, cpy), and then must connect the 
        /// last point in the subpath to the given point (x, y) using a quadratic Bézier curve with control point (cpx, cpy), 
        /// and must then add the given point (x, y) to the subpath.
        /// </summary>
        /// <param name="cpx"></param>
        /// <param name="cpy"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void quadraticCurveTo(double cpx, double cpy, double x, double y)
        {
            PointF[] transformedPoints = InternalTransform(cpx, cpy, x, y);
            //method must ensure there is a subpath for (cpx, cpy)
            if (path.PointCount == 0)
            {
                moveTo(cpx, cpy);
            }
            //declare three initial points in single array for convinience
            var qp = new[]
                         {
                             path.GetLastPoint(), transformedPoints[0],
                             transformedPoints[1]
                         };
            PointF cp0 = qp[0];
            PointF cp3 = qp[2];
            var cp1 = new PointF();
            cp1.X = qp[0].X + 2f/3f*(qp[1].X - qp[0].X);
            cp1.Y = qp[0].Y + 2f/3f*(qp[1].Y - qp[0].Y);
            var cp2 = new PointF();
            cp2.X = cp1.X + 1f/3F*(qp[2].X - qp[0].X);
            cp2.Y = cp1.Y + 1f/3F*(qp[2].Y - qp[0].Y);
            path.AddBezier(cp0, cp1, cp2, cp3);
        }

        /// <summary>
        /// The bezierCurveTo(cp1x, cp1y, cp2x, cp2y, x, y) method must ensure there is a subpath for (cp1x, cp1y), and then must 
        /// connect the last point in the subpath to the given point (x, y) using a cubic Bézier curve with control points (cp1x, cp1y) 
        /// and (cp2x, cp2y). Then, it must add the point (x, y) to the subpath.
        /// </summary>
        public void bezierCurveTo(double cp1x, double cp1y, double cp2x, double cp2y, double x, double y)
        {
            PointF[] points = InternalTransform(cp1x, cp1y, cp2x, cp2y, x, y);
            //method must ensure there is a subpath for (cp1x, cp1y)
            if (path.PointCount == 0)
            {
                moveTo(cp1x, cp1y);
            }
            path.AddBezier(path.GetLastPoint(), points[0], points[1], points[2]);
        }

        /// <summary>
        /// The arcTo(x1, y1, x2, y2, radius)  method must first ensure there is a subpath for (x1, y1). Then, the behavior depends on the arguments and the last point in the subpath, as described below.
        /// Negative values for radius must cause the implementation to raise an INDEX_SIZE_ERR exception.
        /// Let the point (x0, y0) be the last point in the subpath.
        /// If the point (x0, y0) is equal to the point (x1, y1), or if the point (x1, y1) is equal to the point (x2, y2), or if the radius radius is zero, then the method must add the point (x1, y1) to the subpath, and connect that point to the previous point (x0, y0) by a straight line.
        /// Otherwise, if the points (x0, y0), (x1, y1), and (x2, y2) all lie on a single straight line, then the method must add the point (x1, y1) to the subpath, and connect that point to the previous point (x0, y0) by a straight line.
        /// </summary>
        public void arcTo(double px1, double py1, double px2, double py2, double pradius)
        {
            float x1 = (float) px1, x2 = (float) px2, y1 = (float) py1, y2 = (float) py2, radius = (float) pradius;
            if (radius < 0)
                throw new Exception(INDEX_SIZE_ERR);
            PointF point = path.GetLastPoint();
            //transfrom points
            PointF[] points = InternalTransform(x1, y1, x2, y2);
            x1 = points[0].X;
            y1 = points[0].Y;
            x2 = points[1].X;
            y2 = points[1].Y;
            float x0 = point.X;
            float y0 = point.Y;
            if (radius == 0 || (x0 == x1 && y0 == y1) || (x1 == x2 && y1 == y2))
            {
                moveTo(x1, y1);
                lineTo(x0, y0);
                return;
            }

            //find angle between two lines (p0, p1) and (p1, p2)
            var v01 = new PointF(x0 - x1, y0 - y1);
            var v12 = new PointF(x2 - x1, y2 - y1);
            var cosA = (float) ((v01.X*v12.X + v01.Y*v12.Y)/
                                (Math.Sqrt(Math.Pow(v01.X, 2) + Math.Pow(v01.Y, 2))*
                                 Math.Sqrt(Math.Pow(v12.X, 2) + Math.Pow(v12.Y, 2))));
            var a = (float) Math.Acos(cosA);
            if (Math.Abs(a - Math.PI) < 0.00001 || a == 0)
            {
                //all three points are on the straight line
                moveTo(x1, y1);
                lineTo(x0, y0);
                return;
            }
            //find distance from point p1(x1, y1) to intersection with circle (arc)
            var d = (float) (radius/Math.Tan(a/2d));
            //tangent point of the line (p0, p1)
            PointF t01 = FindTangentPoint(x1, y1, x0, y0, d);
            PointF t12 = FindTangentPoint(x1, y1, x2, y2, d);
            lineTo(t01.X, t01.Y);
            DrawArcBetweenTwoPoints(t01.X, t01.Y, t12.X, t12.Y, radius, Math.PI - a);
            //from point (x0, y0) to t01            
            moveTo(t12.X, t12.Y);
        }


        /// <summary>
        /// The rect(x, y, w, h) method must create a new subpath containing just the four points (x, y), (x+w, y), (x+w, y+h), 
        /// (x, y+h), with those four points connected by straight lines, and must then mark the subpath as closed. 
        /// It must then create a new subpath with the point (x, y) as the only point in the subpath.
        /// </summary>
        public void rect(double x, double y, double width, double height)
        {
            path.StartFigure();
            PointF[] points = InternalTransform(x, y, x + width, y, x + width, y + height, x, y + height);
            path.AddPolygon(new[]
                                {
                                    points[0],
                                    points[1],
                                    points[2],
                                    points[3]
                                });
            path.CloseFigure();
        }

        /// <summary>
        /// The arc(x, y, radius, startAngle, endAngle, anticlockwise) method draws an arc. 
        /// If the context has any subpaths, then the method must add a straight line from the last point in the subpath to the
        /// start point of the arc. In any case, it must draw the arc between the start point of the arc and the end point of the
        /// arc, and add the start and end points of the arc to the subpath.
        /// </summary>
        public void arc(double x, double y, double radius, double startAngle, double endAngle, bool anticlockwise)
        {
            if (radius < 0)
                throw new Exception(INDEX_SIZE_ERR);
            //transform passed coordinates accoring to internal transformation matrix
            PointF[] points = InternalTransform(x, y);
            x = points[0].X;
            y = points[0].Y;
            bool isStartAfterEnd = startAngle > endAngle;
            double radians = 0;
            if (isStartAfterEnd && anticlockwise)
            {
                radians = startAngle - endAngle;
            }
            else if(!isStartAfterEnd && !anticlockwise)
            {
                radians = endAngle - startAngle;
            }
            else
            {
                radians = 2*Math.PI - (Math.Abs(endAngle) + Math.Abs(startAngle));
            }
            if (radians == 0)
                radians = 2*Math.PI;
            int direction = anticlockwise ? -1 : 1;
            path.AddArc((float) (x - radius), (float) (y - radius), (float) radius*2,
                        (float) radius*2, (float) ConvertRadiansToDegrees(startAngle),
                        (float)
                        ConvertRadiansToDegrees(direction * radians));
        }

        /// <summary>
        /// The fill()  method must fill all the subpaths of the current path, using fillStyle, and using the non-zero winding number rule. 
        /// Open subpaths must be implicitly closed when being filled (without affecting the actual subpaths).
        /// </summary>
        public void fill()
        {
            //TODO:Thus, if two overlapping but otherwise independent subpaths have opposite windings, they cancel out and result in no fill. If they have the same winding, that area just gets painted once.
            GraphicsPath p = (GraphicsPath)path.Clone();
            p.CloseAllFigures();
            //in case with LinearCanvasGradient we have to stretch gradient in order to avoid it's repeating while drawing the shape
            if (_fillStyle is LinearCanvasGradient)
                _fill.brush = transformStrokePoints(_fillStyle as LinearCanvasGradient, p.PathPoints);
            surface.FillPath(_fill.brush, p);
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
            //in case with LinearCanvasGradient we have to stretch gradient in order to avoid it's repeating while drawing the polygon
            if (_strokeStyle is LinearCanvasGradient)
            {
                _stroke = new Pen(transformStrokePoints(_strokeStyle as LinearCanvasGradient, path.PathPoints));
                _stroke.Width = (float) _lineWidth;
                _stroke.MiterLimit = (float) _miterLimit;
            }
            surface.DrawPath(_stroke, (GraphicsPath)path.Clone());
            if (OnPartialDraw != null)
            {
                OnPartialDraw();
            }
        }

        /// <summary>
        /// The clip()  method must create a new clipping region by calculating the intersection of the current clipping region and the area described by the current path, using the non-zero winding number rule. 
        /// Open subpaths must be implicitly closed when computing the clipping region, without affecting the actual subpaths. The new clipping region replaces the current clipping region.
        /// When the context is initialized, the clipping region must be set to the rectangle with the top left corner at (0,0) and the width and height of the coordinate space.
        /// </summary>
        public void clip()
        {
            path.CloseAllFigures();
            surface.Clip = new Region(path);
        }

        private void DrawArcBetweenTwoPoints(double x1, double y1, double x2, double y2, double radius,
                                             double sweepAngle)
        {
            //define coordinates of center of circle
            //length of chord
            double l = Math.Sqrt(Math.Pow((x1 - x2), 2) + Math.Pow((y1 - y2), 2));
            //distance between chord and center of circle
            double d = Math.Sqrt(Math.Abs(radius*radius - l*l/4d));
            double x = (x1 + x2)/2d;
            double y = (y1 + y2)/2d;
            //find coordinates of circle's center
            double rX = x + d*(y1 - y2)/l;
            double rY = y + d*(x2 - x1)/l;
            //find angles
            double a1 = Math.Asin((Math.Abs(y1 - rY)/radius));
            double a2 = Math.Asin((Math.Abs(y2 - rY)/radius));
            //adjust angles
            a1 = AdjustAngle(a1, x1, y1, rX, rY);
            a2 = AdjustAngle(a2, x2, y2, rX, rY);
            int sector1 = GetSectorNumber(x1, y1, rX, rY);
            int sector2 = GetSectorNumber(x2, y2, rX, rY);
            int sectorDifference = Math.Abs(sector1 - sector2);
            if (sectorDifference <= 1) //if angles in the same or in neighborhood sectors
            {
                //draw from min angle to max angle. a1 should be min angle
                if (a1 > a2)
                {
                    double a = a1;
                    a1 = a2;
                    a2 = a;
                }
            }
            else
            {
                //draw from max angle to min angle. a1 should be max angle
                if (a1 < a2)
                {
                    double a = a1;
                    a1 = a2;
                    a2 = a;
                }
            }
            surface.DrawArc(_stroke, (float) (rX - radius), (float) (rY - radius), (float) radius*2,
                            (float) radius*2,
                            (float) ConvertRadiansToDegrees(a1),
                            (float) ConvertRadiansToDegrees(sweepAngle));
        }

        private PointF FindTangentPoint(float x1, float y1, float x0, float y0, float d)
        {
            PointF t01;
            //find point on line (p0, p1) on distance d from point p
            float dx = d*Math.Abs(x0 - x1)/(GeometryUtils.Distance(new PointF(x0, y0), new PointF(x1, y1)));
            float x;
            if (x0 < x1)
            {
                //means x0 left from x1
                x = x1 - dx;
            }
            else //means x0 right from x1
            {
                x = x1 + dx;
            }
            float y;
            float dy = d*Math.Abs(y0 - y1)/(GeometryUtils.Distance(new PointF(x0, y0), new PointF(x1, y1)));
            if (y0 < y1)
            {
                //means y0 uppper y1
                y = y1 - dy;
            }
            else
            {
                //means y0 down y1
                y = y1 + dy;
            }
            t01 = new PointF(x, y);
            return t01;
        }

        private double AdjustAngle(double a, double x, double y, double rX, double rY)
        {
            switch (GetSectorNumber(x, y, rX, rY))
            {
                case 0:
                    return Math.PI*2 - a;
                case 1:
                    return Math.PI + a;
                case 2:
                    return Math.PI - a;
                case 3:
                    return a;
            }
            return a;
        }

        private int GetSectorNumber(double x, double y, double rX, double rY)
        {
            if (x >= rX && y < rY)
                return 0;
            if (x < rX && y <= rY)
                return 1;
            if (x <= rX && y > rY)
                return 2;
            if (x > rX && y >= rY)
                return 3;
            throw new Exception("Invalid coordinates");
        }

        private void ApplyGlobalComposition()
        {
            if (_source != null)
            {
                lock (_sync)
                {
                    surface.Flush();
                    var dest = (Bitmap) _surfaceBitmap.Clone();
                    _source.MakeTransparent(Color.Transparent);
                    dest.MakeTransparent(Color.Transparent);
                    _compositier.blendImages(_source,
                                             dest);
                    surface.Clear(Color.Transparent);
                    surface.DrawImage(_source, 0, 0);
                }
            }
        }

        #endregion

        #region text

        private readonly List<string> validAlignValues = new List<string> {"start", "end", "left", "right", "center"};

        private readonly List<string> validBaselineValues = new List<string>
                                                                {
                                                                    "top",
                                                                    "hanging",
                                                                    "middle",
                                                                    "alphabetic",
                                                                    "ideographic",
                                                                    "bottom"
                                                                };

        /// <summary>
        /// The font IDL attribute, on setting, must be parsed the same way as the 'font' property of CSS (but without supporting property-independent style 
        /// sheet syntax like 'inherit'), and the resulting font must be assigned to the context, with the 'line-height' component forced to 'normal', with 
        /// the 'font-size' component converted to CSS pixels, and with system fonts being computed to explicit values. If the new value is syntactically 
        /// incorrect (including using property-independent style sheet syntax like 'inherit' or 'initial'), then it must be ignored, without assigning a 
        /// new font value. 
        /// </summary>
        public string font
        {
            get { return _font; }
            set
            {
                _font = value;
                Font font = FontUtils.ParseFont(_font, surface.DpiX);
                if (font == null)
                {
                    //When the context is created, the font of the context must be set to 10px sans-serif.
                    if (_parsedFont == null)
                        _parsedFont = new Font("sans-serif", 10);
                }
                else
                {
                    _parsedFont = font;
                }
            }
        }

        /// <summary>
        /// The textAlign IDL attribute, on getting, must return the current value. On setting, if the value is one of start, end,
        /// left, right, or center, then the value must be changed to the new value. Otherwise, the new value must be ignored. 
        /// When the context is created, the textAlign attribute must initially have the value start.
        /// </summary>
        public string textAlign
        {
            get { return _textAlign; }
            set
            {
                if (validAlignValues.Contains(value))
                    _textAlign = value;
            }
        }

        /// <summary>
        /// The textBaseline  IDL attribute, on getting, must return the current value. On setting, if the value is one of
        /// top, hanging, middle, alphabetic, ideographic, or bottom, then the value must be changed to the new value. 
        /// Otherwise, the new value must be ignored. When the context is created, the textBaseline attribute must 
        /// initially have the value alphabetic.
        /// </summary>
        public string textBaseLine
        {
            get { return _textBaseLine; }
            set
            {
                if (validBaselineValues.Contains(value))
                    _textBaseLine = value;
            }
        }

        /// <summary>
        /// The measureText()  method takes one argument, text. When the method is invoked, the user agent must replace all the 
        /// space characters in text with U+0020 SPACE characters, and then must form a hypothetical infinitely wide CSS line 
        /// box containing a single inline box containing the text text, with all the properties at their initial values except 
        /// the 'font' property of the inline element set to the current font of the context, as given by the font attribute, and 
        /// must then return a new TextMetrics object with its width attribute set to the width of that inline box, in CSS pixels.
        /// </summary>
        public object measureText(string text)
        {
            return FontUtils.MeasureText(text, surface, _parsedFont);
        }

        /// <summary>
        /// Parses the font from the string. Coefficient allows to adjust size.
        /// todo: move to utils
        /// </summary>
        /// <summary>
        /// The fillText() and strokeText()  methods take three or four arguments, text, x, y, and optionally maxWidth, and 
        /// render the given text at the given (x, y) coordinates ensuring that the text isn't wider than maxWidth if specified, 
        /// using the current font, textAlign, and textBaseline  values.
        /// </summary>
        public void fillText(string text, double x, double y)
        {
            PointF[] points = InternalTransform(x, y);
            PointF startPoint = FontUtils.ApplyTextAlign(points[0], _textAlign, text, surface, _parsedFont);
            startPoint = FontUtils.ApplyBaseLine(startPoint, _parsedFont, _textBaseLine, surface.DpiX);
            //new PointF(points[0].X - 50, points[0].Y);
            var textPath = new GraphicsPath();
            textPath.AddString(
                text, _parsedFont.FontFamily,
                (int) FontStyle.Regular, _parsedFont.Size, startPoint,
                StringFormat.GenericTypographic);
            surface.FillPath(_fill.brush, textPath);
            if (OnPartialDraw != null)
            {
                OnPartialDraw();
            }
        }

        /// <summary>
        /// The fillText() and strokeText()  methods take three or four arguments, text, x, y, and optionally maxWidth, and 
        /// render the given text at the given (x, y) coordinates ensuring that the text isn't wider than maxWidth if specified, 
        /// using the current font, textAlign, and textBaseline  values.
        /// </summary>
        public void strokeText(string text, double x, double y)
        {
            PointF[] points = InternalTransform(x, y);
            PointF startPoint = FontUtils.ApplyTextAlign(points[0], _textAlign, text, surface, _parsedFont);
            startPoint = FontUtils.ApplyBaseLine(startPoint, _parsedFont, _textBaseLine, surface.DpiX);
            var textPath = new GraphicsPath();
            textPath.AddString(
                text, _parsedFont.FontFamily,
                (int) FontStyle.Regular, _parsedFont.Size, startPoint,
                StringFormat.GenericTypographic);
            surface.DrawPath(new Pen(_stroke.Brush), textPath);
            if (OnPartialDraw != null)
            {
                OnPartialDraw();
            }
        }

        /// <summary>
        /// If the maxWidth argument was specified and the hypothetical width of the inline box in the hypothetical line box is 
        /// greater than maxWidth CSS pixels, then change font to have a more condensed font (if one is available or if a 
        /// reasonably readable one can be synthesized by applying a horizontal scale factor to the font) or a smaller font, 
        /// and return to the previous step. 
        /// </summary>
        public void FillText(string text, double x, double y, object maxWidth)
        {
            double max = double.Parse(maxWidth.ToString());
            //reduce font's size while the text will fit maxWidth
            while (((TextMetrics) measureText(text)).width > max || _parsedFont.Size <= 0)
            {
                _parsedFont = new Font(_parsedFont.FontFamily.Name, _parsedFont.Size - 1);
            }
            fillText(text, x, y);
        }

        public void StrokeText(string text, double x, double y, object maxWidth)
        {
            double max = double.Parse(maxWidth.ToString());
            //reduce font's size while the text will fit maxWidth
            while (((TextMetrics) measureText(text)).width > max || _parsedFont.Size <= 0)
            {
                _parsedFont = new Font(_parsedFont.FontFamily.Name, _parsedFont.Size - 1);
            }
            strokeText(text, x, y);
        }

        #endregion

        #region drawing images

        /// <summary>
        /// Can accept Image class instance, HTMLCanvasElement class instance or url as a source (imageData)
        /// </summary>
        public void drawImage(object imageData, double sx, double sy, double sw, double sh, double dx, double dy,
                              double dw, double dh)
        {
            Bitmap curBitmap;
            if (imageData is Image)
            {
                curBitmap = (imageData as Image).getImage() as Bitmap;
            }
            //TODO: remove IContextProxy check
            else if (imageData is IHTMLCanvasElement && !(imageData is IContextProxy))
            {
                //if we got HTMLCanvasElement than we have to get canvas element and request bitmap without dependence on _visible parameter
                ICanvasRenderingContext2D context2D = (imageData as IHTMLCanvasElement).getCanvas();
                if (context2D is IContextProxy)
                {
                    context2D = ((IContextProxy)context2D).GetRealObject();
                }
                curBitmap =
                    ((CanvasRenderingContext2D)context2D).GetIndependentBitmap();
            }
            else if (imageData is ImageData)
            {
                string url = ((ImageData) imageData).src;
                curBitmap = Utils.GetBitmapFromUrl(url);
                //lazy parameters initialization
                ((ImageData) imageData).width = Convert.ToUInt32(curBitmap.Width);
                ((ImageData) imageData).height = Convert.ToUInt32(curBitmap.Height);
            }
            else if (imageData is IImage)
            {
                curBitmap = (Bitmap)((IImage)imageData).getImage();
            }
            else
            {
                string url = imageData.ToString();
                curBitmap = Utils.GetBitmapFromUrl(url);
            }
            var rect = new RectangleF((float) sx, (float) sy, (float) sw, (float) sh);
            Bitmap cropped = curBitmap.Clone(rect, curBitmap.PixelFormat);
            PointF[] points = InternalTransform(dx, dy);
            surface.DrawImage(cropped, points[0].X, points[0].Y, (float) dw, (float) dh);
            if (OnPartialDraw != null)
                OnPartialDraw();
        }

        /// <summary>
        /// Can accept Image class instance, HTMLCanvasElement class instance or url as a source (imageData)
        /// </summary>
        /// <param name="imageData">Source to get data from</param>
        /// <param name="dx">x-coordinate</param>
        /// <param name="dy">y-cordinate</param>
        public void drawImage(object imageData, double dx, double dy)
        {
            PointF[] points = InternalTransform(dx, dy);
            Bitmap curBitmap = null;
            if (imageData is Image)
            {
                curBitmap = (imageData as Image).getImage() as Bitmap;
            }
            //TODO: remove IContextProxy check
            else if (imageData is IHTMLCanvasElement && !(imageData is IContextProxy))
            {
                //if we got HTMLCanvasElement than we have to get canvas element and request bitmap without dependence on _visible parameter
                ICanvasRenderingContext2D context2D = (imageData as IHTMLCanvasElement).getCanvas();
                if(context2D is IContextProxy)
                {
                    context2D = ((IContextProxy)context2D).GetRealObject();
                }
                curBitmap =
                    ((CanvasRenderingContext2D) context2D).GetIndependentBitmap();
            }
            else if (imageData is ImageData)
            {
                string url = ((ImageData) imageData).src;
                curBitmap = Utils.GetBitmapFromUrl(url);
                //lazy parameters initialization
                ((ImageData) imageData).width = Convert.ToUInt32(curBitmap.Width);
                ((ImageData) imageData).height = Convert.ToUInt32(curBitmap.Height);
            }
            else if(imageData is IImage)
            {
                curBitmap = (Bitmap)((IImage) imageData).getImage();
            }
            else if(imageData is string)
            {
                string url = imageData.ToString();
                curBitmap = Utils.GetBitmapFromUrl(url);
            }
            if (curBitmap != null)
            {
                surface.DrawImage(curBitmap, points[0].X, points[0].Y);
                if (OnPartialDraw != null)
                    OnPartialDraw();
            }
        }

        /// <summary>
        /// Can accept Image class instance, HTMLCanvasElement class instance or url as a source (imageData)
        /// </summary>
        public void drawImage(object imageData, double dx, double dy, double dw, double dh)
        {
            PointF[] points = InternalTransform(dx, dy);
            Bitmap curBitmap;
            if (imageData is Image)
            {
                curBitmap = (imageData as Image).getImage() as Bitmap;
            }
                //TODO: remove IContextProxy check
            else if (imageData is IHTMLCanvasElement && !(imageData is IContextProxy))
            {
                //if we got HTMLCanvasElement than we have to get canvas element and request bitmap without dependence on _visible parameter
                ICanvasRenderingContext2D context2D = (imageData as IHTMLCanvasElement).getCanvas();
                if (context2D is IContextProxy)
                {
                    context2D = ((IContextProxy)context2D).GetRealObject();
                }
                curBitmap =
                    ((CanvasRenderingContext2D)context2D).GetIndependentBitmap();
            }
            else if (imageData is ImageData)
            {
                string url = ((ImageData) imageData).src;
                curBitmap = Utils.GetBitmapFromUrl(url);
                //lazy parameters initialization
                ((ImageData) imageData).width = Convert.ToUInt32(curBitmap.Width);
                ((ImageData) imageData).height = Convert.ToUInt32(curBitmap.Height);
            }
            else if (imageData is IImage)
            {
                curBitmap = (Bitmap)((IImage)imageData).getImage();
            }
            else
            {
                string url = imageData.ToString();
                curBitmap = Utils.GetBitmapFromUrl(url);
            }
            if (dw == 0 || dh == 0) //means they are ommited
            {
                dw = curBitmap.Width;
                dh = curBitmap.Height;
            }
            surface.DrawImage(curBitmap, points[0].X, points[0].Y, (float) dw, (float) dh);
            if (OnPartialDraw != null)
                OnPartialDraw();
        }

        public void commit()
        {
            //do nothing
        }

        #endregion

        #region pixel manipulation

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
                    arr.AddRange(new List<object> {0, 0, 0, 0});
                }
            }
            img.data = Utils.ConvertArrayToJSArray(arr.ToArray());
            return img;
        }

        /// <summary>
        /// Return an ImageData object representing the underlying pixel data for the area of the canvas denoted by the rectangle 
        /// whose corners are the four points (sx, sy), (sx+sw, sy), (sx+sw, sy+sh), (sx, sy+sh), in canvas coordinate space units. 
        /// Pixels outside the canvas must be returned as transparent black. Pixels must be returned as non-premultiplied alpha values.
        /// </summary>
        public object getImageData(double sx, double sy, double sw, double sh)
        {
            if (double.IsNaN(sw) || double.IsInfinity(sw) || double.IsInfinity(sh) || double.IsNaN(sh)
                || double.IsNaN(sx) || double.IsInfinity(sx) || double.IsInfinity(sy) || double.IsNaN(sy))
            {
                throw new NotSupportedException(NOT_SUPPORTED_ERR);
            }
            var img = new ImageData(Convert.ToUInt32(sw), Convert.ToUInt32(sh));
            if (sw == 1 && sh == 1)
            {
//safe mode will be faster
                ProcessImageSafe(sw, sh, sx, sy, img);
            }
            else
            {
//unsafe mode will be faster
                ProcessImageUnSafe(sw, sh, sx, sy, img);
            }

            return img;
        }

        public object createFilterChain()
        {
            return new FilterChain();
        }

        /// <summary>
        /// If canvas is invisible than we should return empty bitmap, otherwise return bitmap we've drawn on.
        /// </summary>
        /// <returns>Bitmap to draw on parent element</returns>
        public Bitmap GetBitmap()
        {
            if (_visible)
                return _surfaceBitmap;
            else return new Bitmap((int) surface.VisibleClipBounds.Width, (int) surface.VisibleClipBounds.Height);
            //return _surfaceBitmap;
            //return _bitmap;
        }

        public bool IsVisible
        {
            get { return _visible; }
        }

        /// <summary>
        /// Writes data from ImageData structures back to the canvas.
        /// </summary>
        public void putImageData(object imagedata, double dx, double dy, double dirtyX, double dirtyY,
                                 double dirtyWidth, double dirtyHeight)
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

            var _dirtyX = (int) Math.Truncate(dirtyX);
            var _dirtyY = (int) Math.Truncate(dirtyY);
            var _dirtyWidth = (int) Math.Truncate(dirtyWidth);
            var _dirtyHeight = (int) Math.Truncate(dirtyHeight);

            var bmp = new Bitmap((int) img.width, (int) img.height);
            byte[] data = Utils.ConvertJSArrayToByteArray(img.data);
            for (int y = _dirtyY; y < _dirtyHeight + _dirtyY; y++)
            {
                for (int x = _dirtyX; x < _dirtyWidth + _dirtyY; x++)
                {
                    int index = y*(int) img.width*4 + x*4;
                    bmp.SetPixel(x, y,
                                 Color.FromArgb(data[index + 3], data[index + 0], data[index + 1],
                                                data[index + 2]));
                }
            }
            surface.DrawImage(bmp, (int) Math.Truncate(dx), (int) Math.Truncate(dy));
            if (OnPartialDraw != null)
            {
                OnPartialDraw();
            }
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


            var bmp = new Bitmap((int) img.width, (int) img.height);
            byte[] data = Utils.ConvertJSArrayToByteArray(img.data);
            //for (int y = 0; y < img.height; y++)
            //{
            //    for (int x = 0; x < img.width; x++)
            //    {
            //        int index = y * (int)img.width * 4 + x * 4;
            //        bmp.SetPixel(x, y,
            //                         Color.FromArgb(data[index + 3], data[index + 0], data[index + 1],
            //                                        data[index + 2]));
            //    }
            //}
            Utils.CopyBytesToBitmap(data, (int) img.width, (int) img.height, ref bmp);
            surface.DrawImage(bmp, (int) Math.Truncate(dx), (int) Math.Truncate(dy));
            if (OnPartialDraw != null)
            {
                OnPartialDraw();
            }
        }

        private void ProcessImageUnSafe(double sw, double sh, double sx, double sy, ImageData img)
        {
            byte[] bytes = Utils.CopyBitmapToBytes((int) sx, (int) sy, (int) sw, (int) sh, _surfaceBitmap);
            var objects = new object[bytes.Length];
            bytes.CopyTo(objects, 0);

            img.data = Utils.ConvertArrayToJSArray(objects);
        }

        private void ProcessImageSafe(double sw, double sh, double sx, double sy, ImageData img)
        {
            var arr = new List<object>();
            for (int y = 0; y < sh; y++)
            {
                for (int x = 0; x < sw; x++)
                {
                    int maxX = (int) sx + x;
                    if (maxX >= _surfaceBitmap.Width)
                        maxX = _surfaceBitmap.Width - 1;
                    int maxY = (int) sy + y;
                    if (maxY >= _surfaceBitmap.Height)
                    {
                        maxY = _surfaceBitmap.Height - 1;
                    }
                    Color color = _surfaceBitmap.GetPixel(maxX, maxY);
                    arr.AddRange(new List<object> {color.R, color.G, color.B, color.A});
                }
            }
            img.data = Utils.ConvertArrayToJSArray(arr.ToArray());
        }

        /// <summary>
        /// Return bitmap without dependence on _visible variable
        /// </summary>
        /// <returns>Bitmap for internal usage</returns>
        public Bitmap GetIndependentBitmap()
        {
            return _surfaceBitmap;
        }

        /// <summary>
        /// Get part of image from surface
        /// </summary>
        public Bitmap GetImage(int width, int height)
        {
            return new Bitmap(width, height, surface);
        }

        public void ChangeSize(int width, int height, bool reset)
        {
            int imgWidth = Math.Min(width, GetWidth());
            int imgHeight = Math.Min(height, GetHeight());
            using (Bitmap currentImage = GetIndependentBitmap().Clone(new Rectangle(0, 0, imgWidth, imgHeight), _surfaceBitmap.PixelFormat))
            {
                _surfaceBitmap.Dispose();
                _surfaceBitmap = new Bitmap(width, height);
                surface = Graphics.FromImage(_surfaceBitmap);
                //TODO: or Color.White??
                surface.Clear(Color.Transparent);
                if (!reset)
                {
                    //scale image to fit new sizes
                    surface.DrawImage(currentImage.GetThumbnailImage(width, height, null, IntPtr.Zero), 0, 0);
                }
            }
        }

        public int GetHeight()
        {
            return (int)surface.VisibleClipBounds.Height;
        }

        public int GetWidth()
        {
            return (int)surface.VisibleClipBounds.Width;
        }

        #endregion

        #region Utils

        //todo: move it to geometry utils
        /// <summary>
        /// Converts radians to degrees
        /// </summary>
        private double ConvertRadiansToDegrees(double radians)
        {
            double degrees = (float) (180/Math.PI)*radians;
            return (degrees);
        }

        /// <summary>
        /// Reset Canvas fields to their initial value.
        /// </summary>
        private void reset()
        {
            SetDefaultValues();
            _stroke = _initialConfig.Stroke;
            _fill = _initialConfig.Fill;
            SetLineConfig(_initialConfig.Stroke);
        }

        /// <summary>
        /// InternalTransform set of methods allows to apply current tansformation matrix to the set of points.
        /// For convenience of use, points accepted as separate pair of coordinates (x, y)
        /// </summary>
        private PointF[] InternalTransform(double x, double y, double x1, double y1, double x2, double y2, double x3,
                                           double y3)
        {
            return new[]
                       {
                           InternalTransform(x, y)[0], InternalTransform(x1, y1)[0], InternalTransform(x2, y2)[0],
                           InternalTransform(x3, y3)[0]
                       };
        }

        private PointF[] InternalTransform(double x, double y, double x1, double y1, double x2, double y2)
        {
            return new[] {InternalTransform(x, y)[0], InternalTransform(x1, y1)[0], InternalTransform(x2, y2)[0]};
        }

        private PointF[] InternalTransform(double x, double y, double x1, double y1)
        {
            return new[] {InternalTransform(x, y)[0], InternalTransform(x1, y1)[0]};
        }

        private PointF[] InternalTransform(double x, double y)
        {
            var points = new[] {new PointF((float) x, (float) y)};
            _transformation.TransformPoints(points);
            return points;
        }

        #endregion

        #region Implementation of IReflect

// FIXME: Wrap IExpando from Host. This one targets JScript.

        //IExpando

        private readonly SortedList<string, FieldInfo> _arbitraryFields = new SortedList<string, FieldInfo>();
        private readonly SortedList<string, PropertyInfo> _arbitraryProperties = new SortedList<string, PropertyInfo>();
        private FieldInfo[] _fields;
        private MethodInfo[] _methods;
        private PropertyInfo[] _properties;

        private object _myApply = new object();
        private string _propertyToExecute;

        public object myApply
        {
            get { return _myApply; }
            set { _myApply = value; }
        }

        public MethodInfo GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types,
                                    ParameterModifier[] modifiers)
        {
            return typeof (CanvasRenderingContext2D).GetMethod(name, bindingAttr, binder, types, modifiers);
        }

        public MethodInfo GetMethod(string name, BindingFlags bindingAttr)
        {
            return typeof (CanvasRenderingContext2D).GetMethod(name, bindingAttr);
        }

        public MethodInfo[] GetMethods(BindingFlags bindingAttr)
        {
            return typeof (CanvasRenderingContext2D).GetMethods(bindingAttr);
        }

        public FieldInfo GetField(string name, BindingFlags bindingAttr)
        {
            return typeof (CanvasRenderingContext2D).GetField(name, bindingAttr);
        }

        public FieldInfo[] GetFields(BindingFlags bindingAttr)
        {
            return typeof (CanvasRenderingContext2D).GetFields(bindingAttr);
        }

        public PropertyInfo GetProperty(string name, BindingFlags bindingAttr)
        {
            if (_arbitraryProperties.ContainsKey(name))
                return _arbitraryProperties[name];
            return typeof (CanvasRenderingContext2D).GetProperty(name, bindingAttr);
        }

        public PropertyInfo GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType,
                                        Type[] types, ParameterModifier[] modifiers)
        {
            if (_arbitraryProperties.ContainsKey(name))
                return _arbitraryProperties[name];
            return typeof (CanvasRenderingContext2D).GetProperty(name, bindingAttr, binder, returnType, types, modifiers);
        }

        public PropertyInfo[] GetProperties(BindingFlags bindingAttr)
        {
            return typeof (CanvasRenderingContext2D).GetProperties(bindingAttr);
        }

        public MemberInfo[] GetMember(string name, BindingFlags bindingAttr)
        {
            if (_arbitraryProperties.ContainsKey(name))
            {
                _propertyToExecute = name;
                return typeof (CanvasRenderingContext2D).GetMember("InvokeArbirtaryProperty");
            }
            return typeof (CanvasRenderingContext2D).GetMember(name, bindingAttr);
        }

        public MemberInfo[] GetMembers(BindingFlags bindingAttr)
        {
            return typeof (CanvasRenderingContext2D).GetMembers(bindingAttr);
        }

        public object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args,
                                   ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
        {
            if (_arbitraryFields.ContainsKey(name))
            {
                return InvokeArbitraryMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
            }

            if (name != "drawImage")
            {
                return typeof (CanvasRenderingContext2D).InvokeMember(name, invokeAttr, binder, target, args,
                                                                      modifiers, culture,
                                                                      namedParameters);
            }
            else
            {
                switch (args.Length)
                {
                    case 3:
                        drawImage(args[0], Convert.ToDouble(args[1]), Convert.ToDouble(args[2]));
                        break;
                    case 5:
                        drawImage(args[0], Convert.ToDouble(args[1]), Convert.ToDouble(args[2]),
                                  Convert.ToDouble(args[3]), Convert.ToDouble(args[4]));
                        break;
                    case 9:
                        drawImage(args[0], Convert.ToDouble(args[1]), Convert.ToDouble(args[2]),
                                  Convert.ToDouble(args[3]), Convert.ToDouble(args[4]), Convert.ToDouble(args[5]),
                                  Convert.ToDouble(args[6]), Convert.ToDouble(args[7]), Convert.ToDouble(args[8]));
                        break;
                }
            }
            return null;
        }

        public Type UnderlyingSystemType
        {
            get { return typeof (CanvasRenderingContext2D); }
        }

        public void InvokeArbirtaryProperty(params object[] args)
        {
            if (!string.IsNullOrEmpty(_propertyToExecute))
            {
                //ScriptFunction sf = (ScriptFunction)((PrototypePropertyInfo)_arbitraryProperties[_propertyToExecute]).Value;
                //sf.Invoke(this, args);
                //_propertyToExecute = string.Empty;
            }
        }

        public object InvokeArbitraryMember(string name, BindingFlags attr, Binder binder, object target, object[] args,
                                            ParameterModifier[] modifiers, CultureInfo culture, string[] parameters)
        {
            if (attr == (BindingFlags.PutDispProperty | BindingFlags.OptionalParamBinding) ||
                attr == (BindingFlags.SetProperty | BindingFlags.OptionalParamBinding))
            {
                _arbitraryFields[name].SetValue(this, args[0]);
            }
            if (attr == (BindingFlags.GetProperty | BindingFlags.OptionalParamBinding))
            {
                return _arbitraryFields[name].GetValue(this).ToString();
            }
            if (attr == (BindingFlags.InvokeMethod | BindingFlags.OptionalParamBinding))
            {
                object function = _arbitraryFields[name].GetValue(this);
                //_myApply.GetType().InvokeMember("", BindingFlags.InvokeMethod, null, _myApply,
                //                                new[] {function, this, Microsoft.JScript.GlobalObject.Array.ConstructArray(args)});
            }
            return null;
        }

        #endregion


    }
}