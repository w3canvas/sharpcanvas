using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using SharpCanvas.Common;

namespace SharpCanvas.Context.Drawing2D
{
    public class PathCanvasGradient : ILinearCanvasGradient //IPathCanvasGradient
    {
        private const string INDEX_SIZE_ERR =
            "The specified offset is negative or greater than the number of characters in data, or if the specified count is negative";

        private const string SYNTAX_ERR = "Syntax error";
        private readonly List<Color> _colors = new List<Color>();

        private readonly float _endRadius;
        private readonly List<float> _positions = new List<float>();
        private readonly float _startRadius;
        private PointF _end;
        private PointF _start;

        public PathCanvasGradient(PointF _start, PointF _end)
        {
            this._start = _start;
            this._end = _end;
        }

        public PathCanvasGradient(PointF _start, PointF _end, GraphicsPath path)
        {
            this._start = _start;
            this._end = _end;
            this.path = path;
        }

        public PathCanvasGradient(PointF _start, float r0, PointF _end, float r1, GraphicsPath path)
        {
            this._start = _start;
            this._end = _end;
            this.path = path;
            _startRadius = r0;
            _endRadius = r1;
        }

        public PathCanvasGradient()
        {
        }


        public GraphicsPath path { get; set; }

        public PointF start
        {
            get { return _start; }
            set { _start = value; }
        }

        public PointF end
        {
            get { return _end; }
            set { _end = value; }
        }

        #region ILinearCanvasGradient Members

        public void addColorStop(double offset, string color)
        {
            if (!ColorUtils.isValidColor(color))
                throw new Exception(SYNTAX_ERR);
            if (offset < 0 || offset > 1)
                throw new Exception(INDEX_SIZE_ERR);
            _colors.Add(ColorUtils.ParseColor(color));
            _positions.Add((float) offset);
        }

        public object GetBrush()
        {
            var colorBlend = new ColorBlend(_colors.Count);
            if (_positions[_positions.Count - 1] != 1.0)
            {
                _positions.Add(1.0f);
                _colors.Add(_colors[_colors.Count - 1]);
            }
            if (_positions[0] != 0.0)
            {
                _positions.Insert(0, 0.0f);
                _colors.Insert(0, _colors[0]);
            }
            var path = new GraphicsPath();
            path.AddEllipse(_end.X - _endRadius, _end.Y - _endRadius, _endRadius*2, _endRadius*2);

            var gradient = new PathGradientBrush(path);
            gradient.CenterColor = _colors[0];
            gradient.CenterPoint = _start;
            gradient.FocusScales = new PointF(_startRadius/_endRadius, _startRadius/_endRadius);

            _colors.Reverse();
            for (int i = 0; i < _positions.Count; i++)
            {
                _positions[i] = 1 - _positions[i];
            }
            _positions.Reverse();
            colorBlend.Colors = _colors.ToArray();
            colorBlend.Positions = _positions.ToArray();
            //avoid using color blend for now
            gradient.InterpolationColors = colorBlend;
            return gradient;
        }

        #endregion
    }
}