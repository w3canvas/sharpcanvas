using System.Drawing;
using System.Drawing.Drawing2D;

namespace SharpCanvas.Context.Drawing2D
{
    public class CanvasState
    {
        //private Pen _stroke;
        //private fill _fill;
        private readonly FontFamily _family;
        //private font _parsedFont = new font("sans-serif", 10);

        private readonly object _fillStyle = string.Empty;
        private readonly string _font;
        private readonly string _globalCompositeOperation;
        private readonly GraphicsState _graphicsState;
        private readonly string _lineCap;
        private readonly string _lineJoin;
        private readonly double _lineWidth;
        private readonly double _miterLimit;
        private readonly double _shadowBlur;
        private readonly string _shadowColor;
        private readonly double _shadowOffsetX;
        private readonly double _shadowOffsetY;
        private readonly object _strokeStyle = string.Empty;
        private readonly string _textAlign = string.Empty;
        private readonly string _textBaseLine = string.Empty;
        private double _globalAlpha = -1;

        public CanvasState(GraphicsState state, FontFamily family, string font, double lineWidth, double miterLimit,
                           string lineJoin, string lineCap, string globalCompositeOperation, double shadowOffsetX,
                           double shadowOffsetY, double shadowBlur, string shadowColor, string fillStyle,
                           string strokeStyle, Matrix transformation)
        {
            _graphicsState = state;
            _family = family;
            _font = font;
            _lineWidth = lineWidth;
            _miterLimit = miterLimit;
            _lineJoin = lineJoin;
            _lineCap = lineCap;
            _globalCompositeOperation = globalCompositeOperation;
            _shadowOffsetX = shadowOffsetX;
            _shadowOffsetY = shadowOffsetY;
            _shadowBlur = shadowBlur;
            _shadowColor = shadowColor;
            _fillStyle = fillStyle;
            _strokeStyle = strokeStyle;
            Transformation = transformation;
        }

        public Matrix Transformation { get; set; }

        public GraphicsState GraphicsState
        {
            get { return _graphicsState; }
        }

        public FontFamily Family
        {
            get { return _family; }
        }

        public string Font
        {
            get { return _font; }
        }

        public object FillStyle
        {
            get { return _fillStyle; }
        }

        public object StrokeStyle
        {
            get { return _strokeStyle; }
        }

        public double LineWidth
        {
            get { return _lineWidth; }
        }

        public double MiterLimit
        {
            get { return _miterLimit; }
        }

        public string LineJoin
        {
            get { return _lineJoin; }
        }

        public string LineCap
        {
            get { return _lineCap; }
        }

        public string GlobalCompositeOperation
        {
            get { return _globalCompositeOperation; }
        }

        public double GlobalAlpha
        {
            get { return _globalAlpha; }
        }

        public double ShadowOffsetX
        {
            get { return _shadowOffsetX; }
        }

        public double ShadowOffsetY
        {
            get { return _shadowOffsetY; }
        }

        public double ShadowBlur
        {
            get { return _shadowBlur; }
        }

        public string ShadowColor
        {
            get { return _shadowColor; }
        }

        public string TextAlign
        {
            get { return _textAlign; }
        }

        public string TextBaseLine
        {
            get { return _textBaseLine; }
        }
    }
}