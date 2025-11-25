using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using SharpCanvas.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using SharpCanvas.Interop;
using SharpCanvas.Common;
using SharpCanvas.StandardFilter;
using SharpCanvas.Shared;
using SharpCanvas.Host.Prototype;

using Convert = System.Convert;

//namespace SharpCanvas.Drawing
namespace SharpCanvas.Context.Drawing2D
{
    public delegate void OnPartialDrawHandler();

    [ComVisible(true),
     ComSourceInterfaces(typeof (ICanvasRenderingContext2D))]
    public class CanvasRenderingContext2D : ObjectWithPrototype, ICanvasRenderingContext2D
    {
        #region Private Fields

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

        public event OnPartialDrawHandler OnPartialDraw;

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

        public FontFaceSet fonts { get; } = new FontFaceSet();
        object ICanvasRenderingContext2D.fonts => fonts;

        public new object prototype()
        {
            return base.prototype;
        }

        public object __proto__
        {
            get { return base.prototype; }
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
        /// <param name="visible">determining is this object be visible inside container</param>
        public CanvasRenderingContext2D(Graphics s, Bitmap bitmap, Pen stroke, IFill fill, bool visible)
            : base(Guid.NewGuid())
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
            : base(Guid.NewGuid())
        {
            throw new NotSupportedException("This constructor is not supported and should not be used.");
        }

        private void CanvasRenderingContext2D_OnPartialDraw()
        {
            ApplyShadows();
            // ApplyGlobalComposition(); // TODO: Method implementation missing
            //there is no need to call RequestDraw on the container because container already subscribed to OnPartialDraw event
            //and will execute Redraw method once OnPartialDraw occurs
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
                throw new Exception(ErrorMessages.CANVAS_STACK_UNDERFLOW);
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

        /// <summary>
        /// Adds a translation transformation to the current matrix by moving the canvas origin by x horizontally and y vertically.
        /// </summary>
        /// <param name="x">Distance to move in the horizontal direction. Positive values move right, negative values move left</param>
        /// <param name="y">Distance to move in the vertical direction. Positive values move down, negative values move up</param>
        /// <remarks>
        /// This method modifies the transformation matrix. Subsequently drawn shapes will be affected by this translation.
        /// To reset the transformation, use resetTransform() or setTransform().
        /// </remarks>
                public void translate(double x, double y)
        {
            _transformation.Translate((float) x, (float) y);
        }

        /// <summary>
        /// Adds a rotation to the transformation matrix. The rotation angle is in radians.
        /// The rotation center point is always the canvas origin. To change the center point, use translate() before calling rotate().
        /// </summary>
        /// <param name="angle">The rotation angle, in radians, clockwise. To convert degrees to radians: radians = (Math.PI/180)*degrees</param>
        /// <remarks>
        /// The rotation is applied around the current origin (0, 0). To rotate around a different point,
        /// translate to that point, rotate, then translate back.
        /// </remarks>
                public void rotate(double angle)
        {
            _transformation.Rotate((float) GeometryUtils.ConvertRadiansToDegrees(angle), MatrixOrder.Prepend);
        }

        /// <summary>
        /// Adds a scaling transformation to the canvas units horizontally and/or vertically.
        /// </summary>
        /// <param name="x">Scaling factor in the horizontal direction. A value of 1.0 means no scaling. Negative values flip horizontally</param>
        /// <param name="y">Scaling factor in the vertical direction. A value of 1.0 means no scaling. Negative values flip vertically</param>
        /// <remarks>
        /// By default, one unit on the canvas is one pixel. A scaling transformation modifies this behavior.
        /// For instance, a scaling factor of 0.5 results in a unit size of 0.5 pixels; shapes are thus drawn at half the normal size.
        /// A scaling factor of 2.0 increases the unit size to 2 pixels; shapes are thus drawn at twice the normal size.
        /// </remarks>
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
                if (!double.IsNaN(value) && !double.IsInfinity(value) && value >= 0.0 && value <= 1.0)
                {
                    _globalAlpha = value;
                    var argb = (int) Math.Floor(_globalAlpha*255);
                    _fill.color = Color.FromArgb(argb, _fill.color);
                    _stroke.Color = Color.FromArgb(argb, _stroke.Color);
                }
            }
        }

        /// <summary>
        /// Gets or sets the global composite operation mode for drawing operations.
        ///
        /// Supported modes in System.Drawing implementation:
        /// - source-over, source-in, source-out, source-atop
        /// - destination-over, destination-in, destination-out, destination-atop
        /// - lighter, darker, copy, xor
        ///
        /// Note: The following modes from the HTML5 Canvas spec are NOT supported in the System.Drawing backend
        /// (use SkiaSharp backend for full support):
        /// - multiply, screen, overlay, darken, lighten
        /// - color-dodge, color-burn, hard-light, soft-light
        /// - difference, exclusion, hue, saturation, color, luminosity
        ///
        /// When the context is created, the globalCompositeOperation attribute must initially have the value "source-over".
        /// </summary>
        public string globalCompositeOperation
        {
            get { return _globalCompositeOperation; }
            set
            {
                _globalCompositeOperation = value;
                surface.Flush();
                _source = (Bitmap) _surfaceBitmap.Clone();
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

        /// <summary>
        /// Creates a linear gradient object with a starting point of (x0, y0) and an end point of (x1, y1).
        /// The gradient is created in the coordinate space of the canvas and is affected by the current transformation matrix.
        /// </summary>
        /// <param name="x0">The x-axis coordinate of the start point</param>
        /// <param name="y0">The y-axis coordinate of the start point</param>
        /// <param name="x1">The x-axis coordinate of the end point</param>
        /// <param name="y1">The y-axis coordinate of the end point</param>
        /// <returns>A LinearCanvasGradient object that can be used as a fillStyle or strokeStyle</returns>
        /// <remarks>
        /// Use addColorStop() on the returned gradient object to define the colors and stops.
        /// The gradient will be painted relative to the current transformation matrix at the time the gradient is created.
        /// </remarks>
                public object createLinearGradient(double x0, double y0, double x1, double y1)
        {
            PointF[] points = InternalTransform(x0, y0, x1, y1);
            return new LinearCanvasGradient(points[0], points[1]);
        }

        /// <summary>
        /// Creates a radial gradient object with two circles defined by their center points and radii.
        /// The gradient begins at the first circle (x0, y0, r0) and extends to the second circle (x1, y1, r1).
        /// </summary>
        /// <param name="x0">The x-axis coordinate of the start circle's center</param>
        /// <param name="y0">The y-axis coordinate of the start circle's center</param>
        /// <param name="r0">The radius of the start circle (must be non-negative)</param>
        /// <param name="x1">The x-axis coordinate of the end circle's center</param>
        /// <param name="y1">The y-axis coordinate of the end circle's center</param>
        /// <param name="r1">The radius of the end circle (must be non-negative)</param>
        /// <returns>A PathCanvasGradient object that can be used as a fillStyle or strokeStyle</returns>
        /// <exception cref="Exception">Thrown if r0 or r1 is negative (INDEX_SIZE_ERR)</exception>
        /// <remarks>
        /// Use addColorStop() on the returned gradient object to define the colors and stops.
        /// The gradient is created in the coordinate space of the canvas and is affected by the current transformation matrix.
        /// </remarks>
                public object createRadialGradient(double x0, double y0, double r0, double x1, double y1, double r1)
        {
            if (r0 < 0 || r1 < 0)
                throw new Exception(ErrorMessages.INDEX_SIZE_ERR);
            PointF[] points = InternalTransform(x0, y0, x1, y1);
            //PointF[] points = new PointF[]{new PointF((float)x0, (float)y0), new PointF((float)x1, (float)y1)  };
            return new PathCanvasGradient(points[0], (float) r0,
                                          points[1], (float) r1, path);
        }

        /// <summary>
        /// Creates a pattern using the specified image and repetition mode.
        /// The pattern can be used as a fillStyle or strokeStyle.
        /// </summary>
        /// <param name="imageData">The image to use as the pattern. Can be an ImageData object or ICanvasProxy</param>
        /// <param name="repetition">How to repeat the pattern. Valid values: "repeat", "repeat-x", "repeat-y", "no-repeat"</param>
        /// <returns>A CanvasPattern object that can be used as a fillStyle or strokeStyle</returns>
        /// <remarks>
        /// The pattern is created in the coordinate space of the canvas and will be affected by the transformation matrix
        /// when it is applied as a style.
        /// </remarks>
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
                if (!double.IsNaN(value) && !double.IsInfinity(value) && value > 0)
                {
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
                if (!double.IsNaN(value) && !double.IsInfinity(value) && value > 0)
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
                if (!double.IsNaN(value) && !double.IsInfinity(value) && value >= 0)
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
            Color transparent = Color.FromArgb(0, 0, 0, 0);
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

            surface.FillPolygon(_fill.brush, new[]
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

        private Brush transformStrokePoints(LinearCanvasGradient gradient, PointF[] points)
        {
            return (Brush)gradient.GetBrush();
        }

        private void ApplyShadows()
        {
            if (!string.IsNullOrEmpty(_shadowColor) && _shadowColor != "rgba(0,0,0,0)"
                && _shadowBlur != 0 && (_shadowOffsetX != 0 || _shadowOffsetY != 0))
            {
                var blur = new GaussianBlur((int) _shadowBlur);
                var clone = (Bitmap) _surfaceBitmap.Clone();
                clone.MakeTransparent(Color.Transparent);
                using (Graphics image = Graphics.FromImage(clone))
                {
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
        }

        // Path API methods
        public void beginPath()
        {
            path?.Dispose();
            path = new GraphicsPath();
        }

        public void closePath()
        {
            path.CloseFigure();
        }

        public void moveTo(double x, double y)
        {
            PointF[] points = InternalTransform(x, y);
            path.StartFigure();
            // Store the point as the start of a new figure
            // The next LineTo or curve operation will use this as the start point
        }

        public void lineTo(double x, double y)
        {
            PointF[] points = InternalTransform(x, y);
            if (path.PointCount == 0)
            {
                path.AddLine(0, 0, points[0].X, points[0].Y);
            }
            else
            {
                var lastPoint = path.GetLastPoint();
                path.AddLine(lastPoint, points[0]);
            }
        }

        public void quadraticCurveTo(double cpx, double cpy, double x, double y)
        {
            PointF[] points = InternalTransform(cpx, cpy, x, y);
            if (path.PointCount == 0)
            {
                path.StartFigure();
            }
            // System.Drawing doesn't have native quadratic bezier, so we approximate with cubic
            var lastPoint = path.PointCount > 0 ? path.GetLastPoint() : new PointF(0, 0);
            // Convert quadratic to cubic: CP1 = P0 + 2/3 * (CP - P0), CP2 = P1 + 2/3 * (CP - P1)
            var cp1x = lastPoint.X + 2.0f / 3.0f * (points[0].X - lastPoint.X);
            var cp1y = lastPoint.Y + 2.0f / 3.0f * (points[0].Y - lastPoint.Y);
            var cp2x = points[1].X + 2.0f / 3.0f * (points[0].X - points[1].X);
            var cp2y = points[1].Y + 2.0f / 3.0f * (points[0].Y - points[1].Y);
            path.AddBezier(lastPoint, new PointF(cp1x, cp1y), new PointF(cp2x, cp2y), points[1]);
        }

        public void bezierCurveTo(double cp1x, double cp1y, double cp2x, double cp2y, double x, double y)
        {
            PointF[] points = InternalTransform(cp1x, cp1y, cp2x, cp2y, x, y);
            if (path.PointCount == 0)
            {
                path.StartFigure();
            }
            var lastPoint = path.PointCount > 0 ? path.GetLastPoint() : new PointF(0, 0);
            path.AddBezier(lastPoint, points[0], points[1], points[2]);
        }

        public void arcTo(double x1, double y1, double x2, double y2, double radius)
        {
            if (radius < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(radius), "Radius cannot be negative");
            }

            if (radius == 0)
            {
                lineTo(x1, y1);
                return;
            }

            PointF[] points = InternalTransform(x1, y1, x2, y2);
            PointF p0 = path.PointCount > 0 ? path.GetLastPoint() : new PointF(0, 0);
            PointF p1 = points[0];
            PointF p2 = points[1];

            DrawArcBetweenTwoPoints(p0, p1, p2, (float)radius);
        }

        public void arc(double x, double y, double r, double startAngle, double endAngle, bool anticlockwise = false)
        {
            if (r < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(r), "Radius cannot be negative");
            }

            PointF[] points = InternalTransform(x, y);
            float cx = points[0].X;
            float cy = points[0].Y;
            float radius = (float)r;

            var startDegrees = (float)(startAngle * 180 / Math.PI);
            var endDegrees = (float)(endAngle * 180 / Math.PI);

            float sweepAngle;
            if (anticlockwise)
            {
                sweepAngle = startDegrees - endDegrees;
                if (sweepAngle <= 0)
                {
                    sweepAngle += 360;
                }
                sweepAngle = -sweepAngle;
            }
            else
            {
                sweepAngle = endDegrees - startDegrees;
                if (sweepAngle <= 0)
                {
                    sweepAngle += 360;
                }
            }

            var startX = (float)(cx + radius * Math.Cos(startAngle));
            var startY = (float)(cy + radius * Math.Sin(startAngle));

            if (path.PointCount == 0)
            {
                path.StartFigure();
                path.AddLine(startX, startY, startX, startY);
            }
            else
            {
                var lastPoint = path.GetLastPoint();
                path.AddLine(lastPoint, new PointF(startX, startY));
            }

            path.AddArc(cx - radius, cy - radius, radius * 2, radius * 2, startDegrees, sweepAngle);
        }

        public void rect(double x, double y, double w, double h)
        {
            PointF[] points = InternalTransform(x, y, x + w, y + h);
            path.AddRectangle(new RectangleF(points[0].X, points[0].Y, points[1].X - points[0].X, points[1].Y - points[0].Y));
        }

        public void fill()
        {
            surface.FillPath(_fill.brush, path);
            if (OnPartialDraw != null)
            {
                OnPartialDraw();
            }
        }

        public void fill(object pathObj)
        {
            if (pathObj is GraphicsPath gPath)
            {
                surface.FillPath(_fill.brush, gPath);
                if (OnPartialDraw != null)
                {
                    OnPartialDraw();
                }
            }
        }

        public void stroke()
        {
            surface.DrawPath(_stroke, path);
            if (OnPartialDraw != null)
            {
                OnPartialDraw();
            }
        }

        public void stroke(object pathObj)
        {
            if (pathObj is GraphicsPath gPath)
            {
                surface.DrawPath(_stroke, gPath);
                if (OnPartialDraw != null)
                {
                    OnPartialDraw();
                }
            }
        }

        public void clip()
        {
            surface.SetClip(path, CombineMode.Intersect);
        }

        public void clip(object pathObj)
        {
            if (pathObj is GraphicsPath gPath)
            {
                surface.SetClip(gPath, CombineMode.Intersect);
            }
        }

        public bool isPointInPath(double x, double y)
        {
            PointF[] points = InternalTransform(x, y);
            return path.IsVisible(points[0]);
        }

        public object measureText(string text)
        {
            if (_parsedFont == null)
            {
                ParseFont(_font);
            }
            var size = surface.MeasureString(text, _parsedFont);
            return new { width = (double)size.Width };
        }

        // Text properties
        public string font
        {
            get => _font;
            set
            {
                _font = value;
                ParseFont(value);
            }
        }

        public string textAlign
        {
            get => _textAlign;
            set => _textAlign = value;
        }

        public string textBaseLine
        {
            get => _textBaseLine;
            set => _textBaseLine = value;
        }

        private void ParseFont(string fontString)
        {
            if (string.IsNullOrEmpty(fontString))
            {
                _parsedFont = new Font("sans-serif", 10, FontStyle.Regular, GraphicsUnit.Pixel);
                return;
            }

            try
            {
                // Simple font parsing - format is typically "size family" like "10px Arial"
                var parts = fontString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 0)
                {
                    _parsedFont = new Font("sans-serif", 10, FontStyle.Regular, GraphicsUnit.Pixel);
                    return;
                }

                float size = 10;
                string family = "sans-serif";
                FontStyle style = FontStyle.Regular;

                // Parse size (e.g., "10px", "12pt")
                if (parts.Length > 0)
                {
                    var sizeStr = parts[0].ToLower();
                    if (sizeStr.EndsWith("px"))
                    {
                        if (float.TryParse(sizeStr.Substring(0, sizeStr.Length - 2), out float parsedSize))
                            size = parsedSize;
                    }
                    else if (sizeStr.EndsWith("pt"))
                    {
                        if (float.TryParse(sizeStr.Substring(0, sizeStr.Length - 2), out float parsedSize))
                            size = parsedSize * 96f / 72f; // Convert pt to px
                    }
                    else if (float.TryParse(sizeStr, out float parsedSize))
                    {
                        size = parsedSize;
                    }
                }

                // Parse family (remaining parts)
                if (parts.Length > 1)
                {
                    family = string.Join(" ", parts.Skip(1));

                    // Check for style keywords
                    if (family.IndexOf("bold", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        style |= FontStyle.Bold;
                        family = Regex.Replace(family, "bold", "", RegexOptions.IgnoreCase).Trim();
                    }
                    if (family.IndexOf("italic", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        style |= FontStyle.Italic;
                        family = Regex.Replace(family, "italic", "", RegexOptions.IgnoreCase).Trim();
                    }

                    // Remove quotes if present
                    family = family.Trim('"', '\'', ' ');
                    if (string.IsNullOrEmpty(family))
                        family = "sans-serif";
                }

                _parsedFont = new Font(family, size, style, GraphicsUnit.Pixel);
            }
            catch
            {
                // Fallback to default font if parsing fails
                _parsedFont = new Font("sans-serif", 10, FontStyle.Regular, GraphicsUnit.Pixel);
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
            using (var textPath = new GraphicsPath())
            {
                textPath.AddString(
                    text, _parsedFont.FontFamily,
                    (int) FontStyle.Regular, _parsedFont.Size, startPoint,
                    StringFormat.GenericTypographic);
                surface.FillPath(_fill.brush, textPath);
            }
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
            using (var textPath = new GraphicsPath())
            {
                textPath.AddString(
                    text, _parsedFont.FontFamily,
                    (int) FontStyle.Regular, _parsedFont.Size, startPoint,
                    StringFormat.GenericTypographic);
                surface.DrawPath(new Pen(_stroke.Brush), textPath);
            }
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
        /// Draws an image onto the canvas at the specified position using float coordinates.
        /// </summary>
        /// <param name="imageData">The image to draw</param>
        /// <param name="dx">The x-coordinate of the destination position</param>
        /// <param name="dy">The y-coordinate of the destination position</param>
        public void drawImage(object imageData, float dx, float dy)
        {
            drawImage(imageData, (double)dx, (double)dy);
        }

        /// <summary>
        /// Draws an image, canvas, or video onto the canvas with advanced clipping and scaling.
        /// This method allows drawing a portion of the source image (defined by sx, sy, sw, sh)
        /// into a destination rectangle on the canvas (defined by dx, dy, dw, dh).
        ///
        /// The current transformation matrix is applied to the destination coordinates.
        /// </summary>
        /// <param name="imageData">The source image. Can accept Image class instance, HTMLCanvasElement class instance, or URL as a source</param>
        /// <param name="sx">The x-axis coordinate of the top-left corner of the sub-rectangle of the source image to draw</param>
        /// <param name="sy">The y-axis coordinate of the top-left corner of the sub-rectangle of the source image to draw</param>
        /// <param name="sw">The width of the sub-rectangle of the source image to draw</param>
        /// <param name="sh">The height of the sub-rectangle of the source image to draw</param>
        /// <param name="dx">The x-axis coordinate in the destination canvas at which to place the top-left corner of the source image</param>
        /// <param name="dy">The y-axis coordinate in the destination canvas at which to place the top-left corner of the source image</param>
        /// <param name="dw">The width to draw the image in the destination canvas (allows scaling)</param>
        /// <param name="dh">The height to draw the image in the destination canvas (allows scaling)</param>
        /// <remarks>
        /// This method uses the current imageSmoothingEnabled and imageSmoothingQuality settings for interpolation.
        /// </remarks>
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
            var originalInterpolationMode = surface.InterpolationMode;
            surface.InterpolationMode = GetInterpolationMode();
            surface.DrawImage(cropped, points[0].X, points[0].Y, (float) dw, (float) dh);
            surface.InterpolationMode = originalInterpolationMode;
            if (OnPartialDraw != null)
                OnPartialDraw();
        }

        /// <summary>
        /// Draws an image onto the canvas at the specified position with its natural dimensions.
        /// The current transformation matrix is applied to the destination coordinates.
        /// </summary>
        /// <param name="imageData">The source image. Can accept Image class instance, HTMLCanvasElement class instance, or URL as a string</param>
        /// <param name="dx">The x-axis coordinate in the destination canvas at which to place the top-left corner of the source image</param>
        /// <param name="dy">The y-axis coordinate in the destination canvas at which to place the top-left corner of the source image</param>
        /// <remarks>
        /// This method uses the current imageSmoothingEnabled and imageSmoothingQuality settings for interpolation.
        /// The image is drawn at its natural width and height.
        /// </remarks>
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
                var originalInterpolationMode = surface.InterpolationMode;
                surface.InterpolationMode = GetInterpolationMode();
                surface.DrawImage(curBitmap, points[0].X, points[0].Y);
                surface.InterpolationMode = originalInterpolationMode;
                if (OnPartialDraw != null)
                    OnPartialDraw();
            }
        }

        /// <summary>
        /// Draws an image onto the canvas at the specified position and scales it to the specified dimensions.
        /// The current transformation matrix is applied to the destination coordinates.
        /// </summary>
        /// <param name="imageData">The source image. Can accept Image class instance, HTMLCanvasElement class instance, or URL as a string</param>
        /// <param name="dx">The x-axis coordinate in the destination canvas at which to place the top-left corner of the source image</param>
        /// <param name="dy">The y-axis coordinate in the destination canvas at which to place the top-left corner of the source image</param>
        /// <param name="dw">The width to draw the image in the destination canvas (0 uses natural width)</param>
        /// <param name="dh">The height to draw the image in the destination canvas (0 uses natural height)</param>
        /// <remarks>
        /// This method uses the current imageSmoothingEnabled and imageSmoothingQuality settings for interpolation.
        /// If dw or dh is 0, the natural dimensions of the image are used instead.
        /// </remarks>
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
            var originalInterpolationMode = surface.InterpolationMode;
            surface.InterpolationMode = GetInterpolationMode();
            surface.DrawImage(curBitmap, points[0].X, points[0].Y, (float) dw, (float) dh);
            surface.InterpolationMode = originalInterpolationMode;
            if (OnPartialDraw != null)
                OnPartialDraw();
        }

        public void commit()
        {
            //do nothing
        }

        private InterpolationMode GetInterpolationMode()
        {
            if (!imageSmoothingEnabled)
            {
                return InterpolationMode.NearestNeighbor;
            }
            return imageSmoothingQuality switch
            {
                "high" => InterpolationMode.HighQualityBicubic,
                "medium" => InterpolationMode.Bilinear,
                "low" => InterpolationMode.Low,
                _ => InterpolationMode.Low,
            };
        }

        #endregion

        #region pixel manipulation

        /// <summary>
        /// Creates a new, blank ImageData object with the specified dimensions.
        /// All pixels are preset to transparent black (rgba(0, 0, 0, 0)).
        /// </summary>
        /// <param name="sw">The width of the new ImageData object</param>
        /// <param name="sh">The height of the new ImageData object</param>
        /// <returns>A new ImageData object with the specified width and height, initialized to transparent black</returns>
        /// <exception cref="NotSupportedException">Thrown if sw or sh are NaN or Infinity</exception>
        /// <remarks>
        /// The returned ImageData object's data property is a one-dimensional array containing the image data
        /// in RGBA order, with integer values between 0 and 255 (inclusive).
        /// </remarks>
        public object createImageData(double sw, double sh, object settings = null)
        {
            if (double.IsNaN(sw) || double.IsInfinity(sw) || double.IsInfinity(sh) || double.IsNaN(sh))
            {
                throw new NotSupportedException(ErrorMessages.NOT_SUPPORTED_ERR);
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

            if (settings != null)
            {
                try
                {
                    dynamic dynSettings = settings;
                    var cs = dynSettings.colorSpace as string;
                    if (!string.IsNullOrEmpty(cs))
                    {
                        img.colorSpace = cs;
                    }
                }
                catch
                {
                    // Ignore errors reading settings
                }
            }

            return img;
        }

        /// <summary>
        /// Return an ImageData object representing the underlying pixel data for the area of the canvas denoted by the rectangle 
        /// whose corners are the four points (sx, sy), (sx+sw, sy), (sx+sw, sy+sh), (sx, sy+sh), in canvas coordinate space units. 
        /// Pixels outside the canvas must be returned as transparent black. Pixels must be returned as non-premultiplied alpha values.
        /// </summary>
        public object getImageData(double sx, double sy, double sw, double sh, object settings = null)
        {
            if (double.IsNaN(sw) || double.IsInfinity(sw) || double.IsInfinity(sh) || double.IsNaN(sh)
                || double.IsNaN(sx) || double.IsInfinity(sx) || double.IsInfinity(sy) || double.IsNaN(sy))
            {
                throw new NotSupportedException(ErrorMessages.NOT_SUPPORTED_ERR);
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

            if (settings != null)
            {
                try
                {
                    dynamic dynSettings = settings;
                    var cs = dynSettings.colorSpace as string;
                    if (!string.IsNullOrEmpty(cs))
                    {
                        img.colorSpace = cs;
                    }
                }
                catch
                {
                    // Ignore errors reading settings
                }
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
        public byte[] GetBitmap()
        {
            Bitmap bitmapToConvert;
            if (_visible)
            {
                bitmapToConvert = _surfaceBitmap;
            }
            else
            {
                bitmapToConvert = new Bitmap((int) surface.VisibleClipBounds.Width, (int) surface.VisibleClipBounds.Height);
            }

            using (var stream = new System.IO.MemoryStream())
            {
                // HACK: In the Linux environment, we need to do this trick to get the image to save
                // correctly. See https://stackoverflow.com/questions/33631405/system-drawing-save-adding-black-background-to-png
                using (Bitmap bmp = new Bitmap(bitmapToConvert)) {
                    bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                }
                return stream.ToArray();
            }
        }

        public bool IsVisible
        {
            get { return _visible; }
        }

        /// <summary>
        /// Paints data from the given ImageData object onto the canvas.
        /// If a dirty rectangle is provided, only the pixels from that rectangle are painted.
        /// The globalAlpha and globalCompositeOperation values are ignored when using putImageData.
        /// </summary>
        /// <param name="imagedata">An ImageData object containing the array of pixel values</param>
        /// <param name="dx">The x-axis coordinate of the top-left corner of the rectangle to paint</param>
        /// <param name="dy">The y-axis coordinate of the top-left corner of the rectangle to paint</param>
        /// <param name="dirtyX">The x-axis coordinate of the top-left corner of the rectangle from imagedata to paint (default: 0)</param>
        /// <param name="dirtyY">The y-axis coordinate of the top-left corner of the rectangle from imagedata to paint (default: 0)</param>
        /// <param name="dirtyWidth">The width of the rectangle from imagedata to paint (default: imagedata width)</param>
        /// <param name="dirtyHeight">The height of the rectangle from imagedata to paint (default: imagedata height)</param>
        /// <exception cref="NotSupportedException">Thrown if any parameter is NaN or Infinity</exception>
        /// <exception cref="Exception">Thrown if imagedata is not an ImageData object</exception>
        /// <remarks>
        /// This method is not affected by the canvas transformation matrix.
        /// The dirty rectangle allows you to specify a subset of the imagedata to be painted for performance optimization.
        /// </remarks>
        public void putImageData(object imagedata, double dx, double dy, double dirtyX, double dirtyY,
                                 double dirtyWidth, double dirtyHeight)
        {
            if (double.IsNaN(dx) || double.IsInfinity(dx) || double.IsInfinity(dy) || double.IsNaN(dy)
                || double.IsInfinity(dirtyX) || double.IsNaN(dirtyX) || double.IsInfinity(dirtyY) ||
                double.IsNaN(dirtyY)
                || double.IsInfinity(dirtyWidth) || double.IsNaN(dirtyWidth) || double.IsInfinity(dirtyHeight) ||
                double.IsNaN(dirtyHeight))
            {
                throw new NotSupportedException(ErrorMessages.NOT_SUPPORTED_ERR);
            }
            if (!(imagedata is ImageData))
            {
                throw new Exception(ErrorMessages.TYPE_MISTMATCH_ERR);
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

        /// <summary>
        /// Paints data from the given ImageData object onto the canvas at the specified position.
        /// The entire ImageData is painted. The globalAlpha and globalCompositeOperation values are ignored.
        /// </summary>
        /// <param name="imagedata">An ImageData object containing the array of pixel values</param>
        /// <param name="dx">The x-axis coordinate of the top-left corner where to paint the imagedata</param>
        /// <param name="dy">The y-axis coordinate of the top-left corner where to paint the imagedata</param>
        /// <exception cref="NotSupportedException">Thrown if dx or dy are NaN or Infinity</exception>
        /// <exception cref="Exception">Thrown if imagedata is not an ImageData object</exception>
        /// <remarks>
        /// This method is not affected by the canvas transformation matrix.
        /// The pixels are copied directly to the canvas without any compositing or alpha blending.
        /// </remarks>
                public void putImageData(object imagedata, double dx, double dy)
        {
            if (double.IsNaN(dx) || double.IsInfinity(dx) || double.IsInfinity(dy) || double.IsNaN(dy))
            {
                throw new NotSupportedException(ErrorMessages.NOT_SUPPORTED_ERR);
            }
            if (!(imagedata is ImageData))
            {
                throw new Exception(ErrorMessages.TYPE_MISTMATCH_ERR);
            }
            var img = imagedata as ImageData;

            var bmp = new Bitmap((int) img.width, (int) img.height);
            byte[] data = Utils.ConvertJSArrayToByteArray(img.data);
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

        #region MDN Properties

        public string direction { get; set; } = "ltr";
        public string filter { get; set; } = "none";
        public string fontKerning { get; set; } = "auto";
        public string fontStretch { get; set; } = "normal";
        public string fontVariantCaps { get; set; } = "normal";
        public bool imageSmoothingEnabled { get; set; } = true;
        public string imageSmoothingQuality { get; set; } = "low";
        public string lang { get; set; } = "en-US";
        public string letterSpacing { get; set; } = "0px";
        private double _lineDashOffset = 0.0;
        public double lineDashOffset
        {
            get => _lineDashOffset;
            set
            {
                _lineDashOffset = value;
                _stroke.DashOffset = (float)value;
            }
        }
        public string textRendering { get; set; } = "auto";
        public string wordSpacing { get; set; } = "0px";

        #endregion

        #region Utils

        public void resetTransform()
        {
            setTransform(1, 0, 0, 1, 0, 0);
        }

        /// <summary>
        /// Returns a copy of the current transformation matrix as a DOMMatrix object.
        /// </summary>
        /// <returns>A DOMMatrix object representing the current transformation matrix</returns>
        /// <remarks>
        /// The returned matrix can be used to save the current transformation state
        /// and restore it later using setTransform().
        /// </remarks>
                public object getTransform()
        {
            var elements = _transformation.Elements;
            return new Shared.DOMMatrix(elements[0], elements[1], elements[2], elements[3], elements[4], elements[5]);
        }

        public void reset()
        {
            _reset();
        }

        public bool isContextLost()
        {
            return false;
        }

        public void drawFocusIfNeeded(object element)
        {
            // No-op
        }

        public void ellipse(double x, double y, double radiusX, double radiusY, double rotation, double startAngle, double endAngle, bool anticlockwise)
        {
            // TODO: This implementation ignores startAngle, endAngle, and anticlockwise. It draws a full ellipse.
            using (var ellipsePath = new GraphicsPath())
            {
                ellipsePath.AddEllipse((float)(x - radiusX), (float)(y - radiusY), (float)(radiusX * 2), (float)(radiusY * 2));

                if (rotation != 0)
                {
                    using (var matrix = new Matrix())
                    {
                        matrix.RotateAt((float)(GeometryUtils.ConvertRadiansToDegrees(rotation)), new PointF((float)x, (float)y));
                        ellipsePath.Transform(matrix);
                    }
                }
                path.AddPath(ellipsePath, false);
            }
        }

        public void roundRect(double x, double y, double w, double h, object radii)
        {
            // TODO: Implement. This requires manual path construction with arcs and lines, which is non-trivial.
            // For now, this will draw a regular rectangle.
            rect(x, y, w, h);
        }

        private double[] _lineDash = new double[0];

        private void UpdateLineDash()
        {
            if (_lineDash == null || _lineDash.Length == 0)
            {
                _stroke.DashStyle = DashStyle.Solid;
            }
            else
            {
                _stroke.DashStyle = DashStyle.Custom;
                var intervals = _lineDash.Select(d => (float)d).ToArray();
                if (intervals.Length % 2 != 0)
                {
                    intervals = intervals.Concat(intervals).ToArray();
                }
                _stroke.DashPattern = intervals;
            }
        }

        public void setLineDash(object segments)
        {
            if (segments is System.Collections.IEnumerable enumerable)
            {
                var list = new List<double>();
                foreach (var item in enumerable)
                {
                    list.Add(System.Convert.ToDouble(item));
                }
                _lineDash = list.ToArray();
            }
            else
            {
                _lineDash = new double[0];
            }
            UpdateLineDash();
        }

        public object getLineDash()
        {
            return _lineDash;
        }

        public object createConicGradient(double startAngle, double x, double y)
        {
            throw new NotImplementedException();
        }

        public bool isPointInStroke(double x, double y)
        {
            return path.IsOutlineVisible((float)x, (float)y, _stroke);
        }

        public object getContextAttributes()
        {
            return new ContextAttributes();
        }

        //todo: move it to geometry utils

        /// <summary>
        /// Reset Canvas fields to their initial value.
        /// </summary>
        private void _reset()
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

        private void DrawArcBetweenTwoPoints(PointF p0, PointF p1, PointF p2, float radius)
        {
            // Calculate vectors
            double dx1 = p1.X - p0.X;
            double dy1 = p1.Y - p0.Y;
            double dx2 = p2.X - p1.X;
            double dy2 = p2.Y - p1.Y;

            // Normalize
            double len1 = Math.Sqrt(dx1 * dx1 + dy1 * dy1);
            double len2 = Math.Sqrt(dx2 * dx2 + dy2 * dy2);
            
            if (len1 < 1e-6 || len2 < 1e-6) return;

            double ux1 = dx1 / len1;
            double uy1 = dy1 / len1;
            double ux2 = dx2 / len2;
            double uy2 = dy2 / len2;

            // Angle between vectors
            double cosA = ux1 * ux2 + uy1 * uy2;
            // Clamp
            if (cosA > 1.0) cosA = 1.0;
            if (cosA < -1.0) cosA = -1.0;
            double angle = Math.Acos(cosA);

            // Tangent distance
            double tanA = Math.Tan((Math.PI - angle) / 2);
            double dist = radius * tanA;

            // Tangent points
            // t1 is on p0->p1, distance 'dist' from p1
            double t1x = p1.X - ux1 * dist;
            double t1y = p1.Y - uy1 * dist;
            
            // t2 is on p1->p2, distance 'dist' from p1
            double t2x = p1.X + ux2 * dist;
            double t2y = p1.Y + uy2 * dist;

            // Center of circle
            // Cross product of u1 and u2: u1.x*u2.y - u1.y*u2.x
            double cross = ux1 * uy2 - uy1 * ux2;
            double sign = cross >= 0 ? 1 : -1;
            
            // Perpendicular vector pointing towards center
            double px = -uy1 * sign;
            double py = ux1 * sign;
            
            double cx = t1x + px * radius;
            double cy = t1y + py * radius;

            // Start and end angles
            double startAngle = Math.Atan2(t1y - cy, t1x - cx);
            double endAngle = Math.Atan2(t2y - cy, t2x - cx);

            // Sweep angle
            double startDeg = startAngle * 180 / Math.PI;
            double endDeg = endAngle * 180 / Math.PI;
            double sweepDeg = endDeg - startDeg;

            if (sign > 0) // Clockwise
            {
                if (sweepDeg < 0) sweepDeg += 360;
            }
            else // Counter-clockwise
            {
                if (sweepDeg > 0) sweepDeg -= 360;
            }

            // Draw line to t1
            // Note: We use AddLine with transformed coordinates (which is what we calculated)
            // But AddLine expects points.
            path.AddLine(p0, new PointF((float)t1x, (float)t1y));
            
            // Draw arc
            path.AddArc((float)(cx - radius), (float)(cy - radius), (float)(radius * 2), (float)(radius * 2), (float)startDeg, (float)sweepDeg);
        }

        #endregion

        public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args,
                                   ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
        {
            if (name == "drawImage")
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
                return null;
            }
            return base.InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
        }

    }
}

