using System.Windows.Media;
// using System.Linq;

namespace SharpCanvas.Media
{
    public class CanvasStyle
    {
        private readonly CanvasPath _path;
        private Brush _fill;
        private double _globalAlpha;
        private PenLineCap _lineCap;
        private Brush _stroke;
        private PenLineJoin _strokeLineJoin;
        private float _strokeMiterLimit;
        private double _strokeWidth;

        public CanvasStyle(CanvasPath path)
        {
            _path = path;
        }

        public CanvasStyle(CanvasPath path, Brush fill, Brush stroke, double strokeWidth, PenLineJoin strokeLineJoin,
                           PenLineCap lineCap, float strokeMiterLimit, double globalAlpha)
        {
            _path = path;
            _fill = fill;
            _stroke = stroke;
            _strokeWidth = strokeWidth;
            _strokeLineJoin = strokeLineJoin;
            _lineCap = lineCap;
            _globalAlpha = globalAlpha;
            _strokeMiterLimit = strokeMiterLimit;
        }

        public Brush Fill
        {
            get { return _fill; }
            set
            {
                _fill = value;
                _path.CommitAndApplyStyle(this);
            }
        }

        public Brush Stroke
        {
            get { return _stroke; }
            set
            {
                _stroke = value;
                _path.CommitAndApplyStyle(this);
            }
        }

        public double StrokeWidth
        {
            get { return _strokeWidth; }
            set
            {
                _strokeWidth = value;
                _path.CommitAndApplyStyle(this);
            }
        }

        public PenLineJoin StrokeLineJoin
        {
            get { return _strokeLineJoin; }
            set
            {
                _strokeLineJoin = value;
                _path.CommitAndApplyStyle(this);
            }
        }

        public PenLineCap LineCap
        {
            get { return _lineCap; }
            set
            {
                _lineCap = value;
                _path.CommitAndApplyStyle(this);
            }
        }

        public float StrokeMiterLimit
        {
            get { return _strokeMiterLimit; }
            set
            {
                _strokeMiterLimit = value;
                _path.CommitAndApplyStyle(this);
            }
        }

        public double GlobalAlpha
        {
            get { return _globalAlpha; }
            set
            {
                _globalAlpha = value;
                _path.CommitAndApplyStyle(this);
            }
        }
    }
}